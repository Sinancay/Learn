using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;
using Microsoft.EntityFrameworkCore;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Consumes("application/json")]
public class ItemController : ControllerBase
{
    private readonly HomeTaskDbContext ht_context;
    public ItemController(HomeTaskDbContext context)
    {
         ht_context = context;
    }

    [HttpGet(Name = "GetItemList")]
    public IEnumerable<ItemVMModel> Get()
    {
        List<Item> itemsData = ht_context.Items.ToList();

        List<ItemVMModel> listOfItems = new List<ItemVMModel>();
        
        foreach (Item item in itemsData)
        {
          ItemVMModel temp =  new ItemVMModel
            {
                Id = item.Id,
                Name = item.Name,
                Categoryid = item.Categoryid,
                Details = item.Details
            };
          listOfItems.Add(temp);
        }
        return listOfItems.ToArray();
    }
    [HttpPost(Name = "ListItemsFilters")]
    public IEnumerable<ItemVMModel> ListItemsFilters(ItemFilter filter)
    {
         var whereClauseCategoryId = (filter.Categoryid != null && filter.Categoryid != 0) ?  $"WHERE categoryid = '{filter.Categoryid}'" : " ";
         var orderBy = (filter.PageNumber != null && filter.PageNumber != 0) && (filter.PageSize != null && filter.PageSize != 0) ?  ", ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS RN"  :  ""; 
         var addedOrderBy = (filter.PageNumber != null && filter.PageNumber != 0) && (filter.PageSize != null  && filter.PageSize != 0) ? "ORDER BY RN" : "";
         var pageSettings = (filter.PageNumber != null  && filter.PageNumber != 0) && (filter.PageSize != null && filter.PageSize != 0) ? $"OFFSET '{filter.PageNumber}' ROWS FETCH NEXT '{filter.PageSize}' ROW ONLY" : " ";
       
         List<ItemVMModel> listOfItems = new List<ItemVMModel>();

         if(orderBy != "" && addedOrderBy != "" && pageSettings != "" && whereClauseCategoryId != ""){ // zero and null value not acceptable
          
            var ItemsFiltered = ht_context.Items
                .FromSqlRaw($"Select * {orderBy} From items {whereClauseCategoryId} {addedOrderBy} {pageSettings}")
                .ToList(); 


            foreach (Item item in ItemsFiltered)
            {
                ItemVMModel temp =  new ItemVMModel
                {
                        Id = item.Id,
                        Name = item.Name,
                        Categoryid = item.Categoryid,
                        Details = item.Details
                };
                listOfItems.Add(temp);
            }
         }
        
         
         return listOfItems.ToArray();
    }

    [HttpPost(Name = "CreateNewItem")]
    public void CreateItem(ItemVMModel item)
    {
        using (var dbContextTransaction = ht_context.Database.BeginTransaction())
        {
                ht_context.Items.Add(new Item(){
                    Id =  item.Id,
                    Name = item.Name,
                    Categoryid = item.Categoryid,
                    Details = item.Details
                });
        }
        ht_context.SaveChanges();
    }

    [HttpPost(Name = "UpdateItem")]
    public void UpdateItem(ItemVMModel item)
     {
        var entity = ht_context.Items.FirstOrDefault(x => x.Id == item.Id);

        entity.Name = item.Name;
        entity.Categoryid = item.Categoryid;
        entity.Details = item.Details;

        ht_context.SaveChanges();
    }

     [HttpDelete("{id}")]
    public void DeleteItem(int id)
     {
        var entity = ht_context.Items.FirstOrDefault(x => x.Id == id.ToString());

        if (entity != null) {
        ht_context.Items.Remove(entity);
        ht_context.SaveChanges();
        }
    }
    
    
}
