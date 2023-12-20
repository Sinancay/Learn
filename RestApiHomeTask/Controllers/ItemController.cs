using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Consumes("application/json")]
public class ItemController : ControllerBase
{
    NpgsqlConnection conn = new NpgsqlConnection("Host=localhost; Database=HomeTaskDB; Username=postgres; Password=adminS;");

    [HttpGet(Name = "GetItemList")]
    public IEnumerable<ItemVMModel> Get()
    {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand("Select * From items", conn);
        // Execute the query and obtain a result set
        NpgsqlDataReader reader = command.ExecuteReader();

        List<ItemVMModel> listOfItems = new List<ItemVMModel>();
        
        while (reader.Read())
        {
          ItemVMModel temp =  new ItemVMModel
            {
                Id = reader["id"].ToString(),
                Name = reader["name"].ToString(),
                Categoryid = reader["categoryId"].ToString(),
                Details = reader["details"].ToString()
            }; // Remember Type Casting is required here it has to be according to database column data type
            listOfItems.Add(temp);
        }
        reader.Close();
        
        command.Dispose();
        conn.Close();
        return listOfItems.ToArray();
    }
    [HttpPost(Name = "ListItemsFilters")]
    public IEnumerable<ItemVMModel> ListItemsFilters(ItemFilter filter)
    {
         conn.Open();
         var whereClauseCategoryId = filter.Categoryid != null ?  $"WHERE categoryid = '{filter.Categoryid}'" : " ";
         var orderBy = filter.PageNumber != null && filter.PageSize != null ?  ", ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS RN"  :  ""; 
         var addedOrderBy = filter.PageNumber != null && filter.PageSize != null ? "ORDER BY RN" : "";
         var pageSettings = filter.PageNumber != null && filter.PageSize != null ? $"OFFSET {filter.PageNumber} ROWS FETCH NEXT {filter.PageSize} ROW ONLY" : " ";

        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"Select * {orderBy} From items {whereClauseCategoryId} {addedOrderBy} {pageSettings} ", conn);
        // Execute the query and obtain a result set
        NpgsqlDataReader reader = command.ExecuteReader();

        List<ItemVMModel> listOfItems = new List<ItemVMModel>();
        
        while (reader.Read())
        {
          ItemVMModel temp =  new ItemVMModel
            {
                Id = reader["id"].ToString(),
                Name = reader["name"].ToString(),
                Categoryid = reader["categoryId"].ToString(),
                Details = reader["details"].ToString()
            }; // Remember Type Casting is required here it has to be according to database column data type
            listOfItems.Add(temp);
        }
        reader.Close();
        
        command.Dispose();
        conn.Close();
        return listOfItems.ToArray();
    }

     [HttpPost(Name = "CreateNewItem")]
    public void CreateItem(ItemVMModel item)
    {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO items (id, name, categoryid, details) VALUES ('{item.Id}', '{item.Name}', '{item.Categoryid}', '{item.Details}');", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();
        
        command.Dispose();
        conn.Close();
    }

    [HttpPost(Name = "UpdateItem")]
    public void UpdateItem(ItemVMModel item)
     {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"UPDATE items SET  name = '{item.Name}, categoryid= '{item.Categoryid}, details= '{item.Details}' WHERE id = '{item.Id}';", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();
            
        command.Dispose();
        conn.Close();
    }

     [HttpDelete(Name = "DeleteItem")]
    public void DeleteItem(int item)
     {
        
        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"DELETE FROM item WHERE id = '{item}';", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();
            
        command.Dispose();
        conn.Close();
    }
    
    
}
