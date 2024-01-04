using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;
using RestApiHomeTask.Services;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes("application/json")]
public class CategoryController : ControllerBase
{
    private readonly IOperationService _serviceProvider;

    public CategoryController(IOperationService serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpGet(Name = "GetListOfCategories")]
    public IEnumerable<CategoryVMModel> Get()
    {
        List<CategoryVMModel> listOfCategories = _serviceProvider.GetAllCategories();

        return listOfCategories;
    }

    [HttpPost(Name = "CreateNewCategory")]
    public void CreateCategory(CategoryVMModel item)
    {
        _serviceProvider.CreateCategory(item);
    }

    [HttpPut(Name = "UpdateCategory")]
    public void UpdateCategory(CategoryVMModel item)
     {
          _serviceProvider.UpdateCategory(item);
    }

     [HttpDelete("{id}")]
    public async void DeleteCategory(int id)
     {
        _serviceProvider.DeleteCategory(id);
    }
    
}
