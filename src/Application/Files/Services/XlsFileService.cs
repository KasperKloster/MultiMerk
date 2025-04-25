using Application.Files.Interfaces;
using Application.Repositories;
using Application.Repositories.Weeklists;
using Domain.Models.Files;
using Domain.Models.Products;
using Domain.Models.Weeklists.Entities;
using Domain.Models.Weeklists.WeeklistTaskLinks;
using Domain.Models.Weeklists.WeeklistTasks;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Services;

public class XlsFileService : IXlsFileService
{
    private readonly IFileParser _fileparser;
    private readonly IProductRepository _productRepository;
    private readonly IWeeklistRepository _weeklistRepository;
    private readonly IWeeklistTaskRepository _weeklistTaskRepository;
    private readonly IWeeklistTaskLinkRepository _weeklistTaskLinkRepository;

    public XlsFileService(IFileParser fileparser, IProductRepository productRepository, IWeeklistRepository weeklistRepository, IWeeklistTaskRepository weeklistTaskRepository, IWeeklistTaskLinkRepository weeklistTaskLinkRepository)
    {
        _fileparser = fileparser;
        _productRepository = productRepository;
        _weeklistRepository = weeklistRepository;
        _weeklistTaskRepository = weeklistTaskRepository;
        _weeklistTaskLinkRepository = weeklistTaskLinkRepository;
    }

    public async Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist)
    {
        // Is an .xls file
        bool HasXlsFileExtension = HasValidXlsFileExtension(file);
        if(!HasXlsFileExtension) {
            return FilesResult.Fail(message: "Invalid file extension.");
        }

        // Getting products from .xls
        List<Product> products = _fileparser.GetProductsFromXls(file);
        if (products.Count == 0) 
        {
            return FilesResult.Fail("No products found in the file.");
        }

        // Insert weeklist first into DB, so we can get the ID
        try{            
            await _weeklistRepository.AddAsync(weeklist);
        } 
        catch (Exception ex) 
        {            
            return FilesResult.Fail($"An error occured while saving weeklist to database: {ex.Message}");
        }

        // Associate products with the saved weeklist
        foreach (var product in products)
        {
            product.WeeklistId = weeklist.Id; // Assign foreign key
        }

        // Insert products into DB
        try{
            await _productRepository.AddRangeAsync(products);
        } 
        catch (Exception ex) 
        {
            return FilesResult.Fail($"An error occured while saving products to database: {ex.Message}");
        }

        // Getting all Weeklisttasks
        List<WeeklistTask> allWeeklistTasks;
        try  {            
            allWeeklistTasks = await _weeklistTaskRepository.GetAllAsync();            
        } 
        catch (Exception ex)
        {
            return FilesResult.Fail($"An error occured while getting all WeeklistTasks: {ex.Message}");
        }
        
        // Map all Weeklisttasks to WeeklistTaskLink
        // All task should be "Awaiting". Except the first task, that should be "Ready"
        int defaultStatusId = 1; // Default status ID - "Awaiting".
        int firstTaskId = 1; // WeeklistTask: Give EAN
        int readyStatusId = 2; // WeeklistTaskStatus: Ready                
        List<WeeklistTaskLink> weeklistTaskLinks = allWeeklistTasks.Select(task => new WeeklistTaskLink{
            WeeklistId = weeklist.Id,
            WeeklistTaskId = task.Id,
            WeeklistTaskStatusId = task.Id == firstTaskId ? readyStatusId : defaultStatusId
        }).ToList();
            
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
    
    private static bool HasValidXlsFileExtension(IFormFile file)
    {
        string FileExtension = Path.GetExtension(file.FileName);
        if(FileExtension != ".xls") {
            return false;
        }
        return true;
    }
}