using Application.DTOs.Authentication;
using Application.DTOs.Weeklists;
using Application.Repositories;
using Application.Repositories.ApplicationUsers;
using Application.Repositories.Weeklists;
using Application.Services.Interfaces.Files;
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

    public WeeklistService(IWeeklistRepository weeklistRepository,
    IXlsFileService xlsFileService,
    IProductRepository productRepository,
    IWeeklistTaskRepository weeklistTaskRepository,
    IWeeklistTaskLinkRepository weeklistTaskLinkRepository,
    IWeeklistUserRoleAssignmentRepository weeklistUserRoleAssignmentRepository,
    IApplicationUserRepository applicationUserRepository,
    UserManager<ApplicationUser> userManager)
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

                    taskLinkDtos.Add(new WeeklistTaskLinkDto
                    {
                        WeeklistTaskId = link.WeeklistTaskId,
                        WeeklistTask = link.WeeklistTask is null ? null : new WeeklistTaskDto
                        {
                            Id = link.WeeklistTask.Id,
                            Name = link.WeeklistTask.Name
                        },
                        WeeklistTaskStatusId = link.WeeklistTaskStatusId,
                        Status = link.WeeklistTaskStatus is null ? null : new WeeklistTaskStatusDto
                        {
                            Id = link.WeeklistTaskStatus.Id,
                            Status = link.WeeklistTaskStatus.Status
                        },
                        AssignedUser = assignedUserDto
                    });
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
        try
        {
            // Parse products from file
            var result = _xlsFileService.GetProductsFromXls(file);
            if (!result.Success)
            {
                return result;
            }

            if (result.Products is null || result.Products.Count == 0)
            {
                return FilesResult.Fail("No products found in the file.");
            }

            // Save weeklist
            await _weeklistRepository.AddAsync(weeklist);

            // Assign foreign keys
            result.Products.ForEach(p => p.WeeklistId = weeklist.Id);
            await _productRepository.AddRangeAsync(result.Products);

            // Fetch tasks and roles
            var tasks = await _weeklistTaskRepository.GetAllAsync();
            var roleAssignments = await _weeklistUserRoleAssignmentRepository.GetAsync();
            var taskIdToRole = roleAssignments.ToDictionary(x => x.WeeklistTaskId, x => x.UserRole);

            // Resolve one user per role
            var userRoleToUserId = new Dictionary<string, string>();
            foreach (var role in taskIdToRole.Values.Distinct())
            {
                var user = await _applicationUserRepository.GetFirstUserByRoleAsync(role);
                if (user != null)
                {
                    userRoleToUserId[role] = user.Id;
                }
            }

            // Create and save task links
            var links = WeeklistTaskLinkFactory.CreateLinks(weeklist.Id, tasks, userRoleToUserId, taskIdToRole);
            await _weeklistTaskLinkRepository.AddWeeklistTaskLinksAsync(links);

            return FilesResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occurred while creating the weeklist: {ex.Message}");
        }
    }

    public async Task<WeeklistDto> GetWeeklistAsync(int weeklistId)
    {
        try
        {
            Weeklist weeklist = await _weeklistRepository.GetWeeklist(weeklistId: weeklistId);
            WeeklistDto weeklistDto = new WeeklistDto
            {
                Id = weeklist.Id,
                Number = weeklist.Number,
                OrderNumber = weeklist.OrderNumber,
                Supplier = weeklist.Supplier,
                ShippingNumber = weeklist.ShippingNumber,
            };
            return weeklistDto;

        }
        catch (Exception ex)
        {
            throw new Exception("Could not fetch weeklist", ex);
        }
    }
}
