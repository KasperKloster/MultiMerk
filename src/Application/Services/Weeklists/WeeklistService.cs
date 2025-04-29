using Application.DTOs.Authentication;
using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
using Application.Repositories;
using Application.Repositories.ApplicationUsers;
using Application.Repositories.Weeklists;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Authentication;
using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.Factories;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Weeklists;

public class WeeklistService : IWeeklistService
{
    private readonly IWeeklistRepository _weeklistRepository;
    private readonly IXlsFileService _xlsFileService;
    private readonly IProductRepository _productRepository;
    private readonly IWeeklistTaskRepository _weeklistTaskRepository;
    private readonly IWeeklistTaskLinkRepository _weeklistTaskLinkRepository;
    private readonly IWeeklistUserRoleAssignmentRepository _weeklistUserRoleAssignmentRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public WeeklistService(IWeeklistRepository weeklistRepository, IXlsFileService xlsFileService, IProductRepository productRepository, IWeeklistTaskRepository weeklistTaskRepository, IWeeklistTaskLinkRepository weeklistTaskLinkRepository, IWeeklistUserRoleAssignmentRepository weeklistUserRoleAssignmentRepository, IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager)
    {
        _weeklistRepository = weeklistRepository;
        _xlsFileService = xlsFileService;
        _productRepository = productRepository;
        _weeklistTaskRepository = weeklistTaskRepository;
        _weeklistTaskLinkRepository = weeklistTaskLinkRepository;
        _weeklistUserRoleAssignmentRepository = weeklistUserRoleAssignmentRepository;
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
    }

    public async Task<List<WeeklistDto>> GetAllWeeklistsAsync()
    {
        try
        {
            var weeklists = await _weeklistRepository.GetAllWeeklists();
            var weeklistDtos = new List<WeeklistDto>();

            foreach (var w in weeklists)
            {
                var taskLinkDtos = new List<WeeklistTaskLinkDto>();

                foreach (var link in w.WeeklistTaskLinks)
                {
                    try
                    {
                        ApplicationUserDto? assignedUserDto = null;

                        if (link.AssignedUser != null)
                        {
                            var roles = await _userManager.GetRolesAsync(link.AssignedUser);
                            assignedUserDto = new ApplicationUserDto
                            {
                                Id = link.AssignedUser.Id,
                                Name = link.AssignedUser.Name,
                                UserRole = roles.FirstOrDefault() ?? string.Empty
                            };
                        }

                        var taskLinkDto = new WeeklistTaskLinkDto
                        {
                            WeeklistTaskId = link.WeeklistTaskId,
                            WeeklistTask = link.WeeklistTask != null ? new WeeklistTaskDto
                            {
                                Id = link.WeeklistTask.Id,
                                Name = link.WeeklistTask.Name
                            } : null,
                            WeeklistTaskStatusId = link.WeeklistTaskStatusId,
                            Status = link.WeeklistTaskStatus != null ? new WeeklistTaskStatusDto
                            {
                                Id = link.WeeklistTaskStatus.Id,
                                Status = link.WeeklistTaskStatus.Status
                            } : null,
                            AssignedUser = assignedUserDto
                        };

                        taskLinkDtos.Add(taskLinkDto);
                    }
                    catch (Exception innerEx)
                    {
                        throw new Exception($"Error building task link DTO for Weeklist ID: {w.Id}, TaskLink ID: {link.WeeklistTaskId}", innerEx);
                    }
                }

                weeklistDtos.Add(new WeeklistDto
                {
                    Id = w.Id,
                    Number = w.Number,
                    OrderNumber = w.OrderNumber,
                    Supplier = w.Supplier,
                    WeeklistTasks = taskLinkDtos
                });
            }

            return weeklistDtos;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not fetch weeklists", ex);
        }
    }

    public async Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist)
    {
        FilesResult result;
        try
        {
            result = _xlsFileService.GetProductsFromXls(file);
            if (!result.Success)
            {
                return result;
            }
            // If no products are found
            if (result.Products == null || result.Products.Count == 0)
            {
                return FilesResult.Fail("No products found in the file.");
            }
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occured while getting products from .xls: {ex.Message}");
        }

        // Insert weeklist first into DB, so we can get the ID
        try
        {
            await _weeklistRepository.AddAsync(weeklist);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occured while saving weeklist to database: {ex.Message}");
        }

        // Associate products with the saved weeklist
        foreach (var product in result.Products)
        {
            product.WeeklistId = weeklist.Id; // Assign foreign key
        }

        // Insert products into DB
        try
        {
            await _productRepository.AddRangeAsync(result.Products);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occured while saving products to database: {ex.Message}");
        }

        // Getting all Weeklisttasks
        List<WeeklistTask> allWeeklistTasks;
        try
        {
            allWeeklistTasks = await _weeklistTaskRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occured while getting all WeeklistTasks: {ex.Message}");
        }



        // Get mapping of TaskId -> Role        
        var roleAssignments = await _weeklistUserRoleAssignmentRepository.GetAsync();
        var taskIdToRole = roleAssignments.ToDictionary(x => x.WeeklistTaskId, x => x.UserRole);
        // Get one user per role (e.g., first admin, first warehouse etc.)
        var userRoleToUserId = new Dictionary<string, string>();
        foreach (var role in taskIdToRole.Values.Distinct())
        {
            var user = await _applicationUserRepository.GetFirstUserByRoleAsync(role); // custom method
            if (user != null)
            {
                userRoleToUserId[role] = user.Id;
            }
        }

        // Map all Weeklisttasks to WeeklistTaskLink
        // All task should have status "Awaiting". Except the first task, that should be "Ready"
        // int defaultStatusId = 1; // Default status ID - "Awaiting".
        // int firstTaskId = 1; // WeeklistTask: Give EAN
        // int readyStatusId = 2; // WeeklistTaskStatus: Ready                      
        var weeklistTaskLinks = WeeklistTaskLinkFactory.CreateLinks(
            weeklist.Id,
            allWeeklistTasks,
            firstTaskId: 1,
            readyStatusId: 2,
            defaultStatusId: 1,
            userRoleToUserId,
            taskIdToRole
        );

        // Save WeeklistTaskLinks
        try
        {
            await _weeklistTaskLinkRepository.AddWeeklistTaskLinksAsync(weeklistTaskLinks);
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occurred while saving WeeklistTaskLinks: {ex.Message}");
        }

        // Return success        
        return FilesResult.SuccessResult();
    }

}
