
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            List<GroceryListItem> groceryListItems = _groceryListItemsRepository.GetAll().Where(g => g.ProductId == productId).ToList(); // Get all GroceryListItems with the given ProductId
            List<BoughtProducts> boughtProducts = new List<BoughtProducts>(); // Client, GroceryList, Product
            
            foreach (GroceryListItem g in groceryListItems) // Fill Client, GroceryList, Product
            {
                GroceryList? groceryList = _groceryListRepository.Get(g.GroceryListId); // Get GroceryList from GroceryListId
                if (groceryList == null) continue;
                Client? client = _clientRepository.Get(groceryList.ClientId); // Get Client from GroceryList.ClientId
                Product? product = _productRepository.Get(g.ProductId); // Get Product from GroceryListItem.ProductId

                if (product == null || client == null) 
                    continue;
                boughtProducts.Add(new BoughtProducts(client, groceryList, product)); // Add to list
            }
            
            return boughtProducts;
        }
    }
}
