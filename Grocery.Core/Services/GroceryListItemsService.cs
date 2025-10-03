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
            Dictionary<int, int> productCount = new Dictionary<int, int>();
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            foreach (GroceryListItem g in groceryListItems)
            {
                if (productCount.ContainsKey(g.ProductId))
                {
                    productCount[g.ProductId] += g.Amount;
                }
                else
                {
                    productCount.Add(g.ProductId, g.Amount);
                }
            }
    
            int i = 1;

            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();
            foreach (KeyValuePair<int, int> g in productCount.OrderByDescending(g => g.Value).Take(topX))
            {
                Product? product = _productRepository.Get(g.Key);
                if (product == null)
                    continue;
                bestSellingProducts.Add(new BestSellingProducts(product.Id, product.Name, product.Stock, g.Value, i++));
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
