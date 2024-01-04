using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;
using RestApiHomeTask.Services;
using Microsoft.EntityFrameworkCore;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes("application/json")]
public class ItemController : ControllerBase
{
    private readonly IOperationService _serviceProvider;

    public ItemController(IOperationService serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }


    [HttpGet(Name = "GetItemList")]
    public IEnumerable<ItemVMModel> GetAllItems()
    {
        List<ItemVMModel> listOfItems = _serviceProvider.GetAllItems();

        return listOfItems;
    }
    [HttpPost("ItemFilter")]
    public IEnumerable<ItemVMModel> ListItemsFilters(ItemFilter filter)
    {
         List<ItemVMModel> listOfItems = _serviceProvider.ListItemsFilters(filter);

         return listOfItems;
    }

    [HttpPost(Name = "CreateItem")]
    public void CreateItem(ItemVMModel item)
    {
       _serviceProvider.CreateItem(item);
    }

    [HttpPut(Name = "UpdateItem")]
    public void UpdateItem(ItemVMModel item)
    {
        _serviceProvider.UpdateItem(item);
    }

     [HttpDelete("{id}")]
    public void DeleteItem(int id)
    {
      _serviceProvider.DeleteItem(id);
    }
    
    
}
