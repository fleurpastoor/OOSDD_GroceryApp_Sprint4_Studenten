using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            Dictionary<int, int> productCount = new Dictionary<int, int>(); // ProductId, Count
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            foreach (GroceryListItem g in groceryListItems) // Count products
            {
                if (productCount.ContainsKey(g.ProductId)) // If product already exists, increment count
                {
                    productCount[g.ProductId] += g.Amount;
                }
                else // If product does not exist, add to dictionary
                {
                    productCount.Add(g.ProductId, g.Amount);
                }
            }
    
            int i = 1;

            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();
            foreach (KeyValuePair<int, int> kvp in productCount.OrderByDescending(kvp => kvp.Value).Take(topX)) // Select topX kvp with highest count
            {
                Product? product = _productRepository.Get(kvp.Key); // Get product from ProductId (kvp.key)
                if (product == null)
                    continue;
                bestSellingProducts.Add(new BestSellingProducts(product.Id, product.Name, product.Stock, kvp.Value, i++));
            }
            return bestSellingProducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
