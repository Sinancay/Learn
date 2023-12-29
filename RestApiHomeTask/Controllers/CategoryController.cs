using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RestApiHomeTask;

namespace RestApiHomeTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Consumes("application/json")]
public class CategoryController : ControllerBase
{
    private readonly HomeTaskDbContext ht_context;
    public CategoryController(HomeTaskDbContext context)
    {
         ht_context = context;
    }

    [HttpGet(Name = "GetListOfCategories")]
    public IEnumerable<CategoryVMModel> Get()
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

        return listOfCategories.ToArray();
    }

     [HttpPost(Name = "CreateNewCategory")]
    public void CreateCategory(CategoryVMModel item)
    {
        using (var dbContextTransaction = ht_context.Database.BeginTransaction())
        {
                ht_context.Categories.Add(new Category(){
                    Id=   item.Id,
                    Name= item.Name
                });
        }
        ht_context.SaveChanges();
    }

    [HttpPost(Name = "UpdateCategory")]
    public void UpdateCategory(CategoryVMModel item)
     {
        var entity = ht_context.Categories.FirstOrDefault(x => x.Id == item.Id);

        entity.Name = item.Name;

        ht_context.SaveChanges();
    }

     [HttpDelete("{id}")]
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
    
}
