using RestApiHomeTask;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace RestApiHomeTask.Services;

public class OperationService : IOperationService
{
    private readonly HomeTaskDbContext ht_context;
    public OperationService(HomeTaskDbContext context)
    {
         ht_context = context;
    }

    public List<CategoryVMModel> GetAllCategories()
    {
         List<Category> categoryData = ht_context.Categories.ToList();

        List<CategoryVMModel> listOfCategories = new List<CategoryVMModel>();
        
        foreach (Category item in categoryData)
        {
          CategoryVMModel temp =  new CategoryVMModel
            {
                Id = item.Id,
                Name = item.Name
            };
          listOfCategories.Add(temp);
        }

        return listOfCategories;
    }

    public void CreateCategory(CategoryVMModel item)
    {
       var temp = new Category(){
            Id = item.Id,
            Name = item.Name
        };
        ht_context.Categories.Add(temp);

        ht_context.SaveChanges();
    }

    public void UpdateCategory(CategoryVMModel item)
     {
        var entity = ht_context.Categories.FirstOrDefault(x => x.Id == item.Id);

        if(entity != null){
            entity.Name = item.Name;
            ht_context.SaveChanges();
        }


    }

    public async void DeleteCategory(int id)
     {
        await DeleteRelatedItems(id);

        var entity = ht_context.Categories.FirstOrDefault(x => x.Id == id.ToString());

        if (entity != null) {
            ht_context.Categories.Remove(entity);
            ht_context.SaveChanges();
        }
    }
    
    public async Task DeleteRelatedItems(int categoryId){
            var entityList = ht_context.Items.Where(x => x.Categoryid == categoryId.ToString()).ToList();
            foreach(var entity in entityList){
                if (entity != null) {
                    ht_context.Items.Remove(entity);
                    ht_context.SaveChanges();
                }
            }
            
    }

     public List<ItemVMModel> GetAllItems()
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

        return listOfItems;
    }
    public List<ItemVMModel> ListItemsFilters(ItemFilter filter)
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
        
         
         return listOfItems;
    }
    public void CreateItem(ItemVMModel item)
    {
        ht_context.Items.Add(new Item(){
            Id =  item.Id,
            Name = item.Name,
            Categoryid = item.Categoryid,
            Details = item.Details
        });

        ht_context.SaveChanges();
    }
    public void UpdateItem(ItemVMModel item)
     {
        var entity = ht_context.Items.FirstOrDefault(x => x.Id == item.Id);
        if(entity != null){
            entity.Name = item.Name;
            entity.Categoryid = item.Categoryid;
            entity.Details = item.Details;
            ht_context.SaveChanges();
        }
       
    }
    public void DeleteItem(int id)
     {
        var entity = ht_context.Items.FirstOrDefault(x => x.Id == id.ToString());

        if (entity != null) {
        ht_context.Items.Remove(entity);
        ht_context.SaveChanges();
        }
    }
}
