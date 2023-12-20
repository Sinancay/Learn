using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Consumes("application/json")]
public class CategoryController : ControllerBase
{
    NpgsqlConnection conn = new NpgsqlConnection("Host=localhost; Database=HomeTaskDB; Username=postgres; Password=adminS;");

    [HttpGet(Name = "GetListOfCategories")]
    public IEnumerable<CategoryVMModel> Get()
    {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand("Select * From category", conn);
        // Execute the query and obtain a result set
        NpgsqlDataReader reader = command.ExecuteReader();

        List<CategoryVMModel> listOfCategory = new List<CategoryVMModel>();
        
        while (reader.Read())
        {
          CategoryVMModel temp =  new CategoryVMModel
            {
                Id = reader["id"].ToString(),
                Name = reader["name"].ToString()
            }; // Remember Type Casting is required here it has to be according to database column data type
            listOfCategory.Add(temp);
        }
        reader.Close();
        
        command.Dispose();
        conn.Close();
        return listOfCategory.ToArray();
    }

     [HttpPost(Name = "CreateNewCategory")]
    public void CreateCategory(CategoryVMModel item)
    {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO category (id, name) VALUES ('{item.Id}', '{item.Name}');", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();
        
        command.Dispose();
        conn.Close();
    }

    [HttpPost(Name = "UpdateCategory")]
    public void UpdateCategory(CategoryVMModel item)
     {

        conn.Open();
        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"UPDATE category SET  name = '{item.Name}' WHERE id = '{item.Id}';", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();
            
        command.Dispose();
        conn.Close();
    }

     [HttpDelete(Name = "DeleteCategory")]
    public async void DeleteCategory(int item)
     {
        await DeleteRelatedItems(item);
        conn.Open();

        // Passing PostGre SQL Function Name
        NpgsqlCommand command = new NpgsqlCommand($"DELETE FROM category WHERE id = '{item}';", conn);
        // Execute the query and obtain a result set
        command.ExecuteReader();

        command.Dispose();
        conn.Close();
    }
    
    public async Task DeleteRelatedItems(int categoryId){
        conn.Open();

        NpgsqlCommand callChildData = new NpgsqlCommand($"DELETE FROM items WHERE categoryid = '{categoryId}';", conn);
        callChildData.ExecuteReader();
        callChildData.Dispose();
        conn.Close();
    }
    
}
