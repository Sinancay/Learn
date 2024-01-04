using RestApiHomeTask;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace RestApiHomeTask.Services;

public interface IOperationService
{
    List<CategoryVMModel> GetAllCategories();
    public void CreateCategory(CategoryVMModel item);
    public void UpdateCategory(CategoryVMModel item);
    public void DeleteCategory(int id);

    List<ItemVMModel> GetAllItems();
    List<ItemVMModel> ListItemsFilters(ItemFilter filter);
    public void CreateItem(ItemVMModel item);
    public void UpdateItem(ItemVMModel item);
    public void DeleteItem(int id);
}
