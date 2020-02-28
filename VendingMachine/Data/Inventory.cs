using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Data
{
    public class Inventory
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public interface IInventoryProvider
    {
        List<Inventory> GetInventories();
    }

    public class InventoryProvider : IInventoryProvider
    {
        private List<Inventory> _inventories;

        public InventoryProvider()
        {
            _inventories = new List<Inventory>
            {
                new Inventory
                {
                    ProductId = 1,
                    Quantity = 5
                },
                new Inventory
                {
                    ProductId = 2,
                    Quantity = 5
                },
                new Inventory
                {
                    ProductId = 3,
                    Quantity = 5
                },
                new Inventory
                {
                    ProductId = 4,
                    Quantity = 5
                },
                new Inventory
                {
                    ProductId = 5,
                    Quantity = 5
                },
                new Inventory
                {
                    ProductId = 6,
                    Quantity = 5
                }
            };
        }

        public List<Inventory> GetInventories()
        {
            return _inventories;
        }

        public void SellProduct(int productId)
        {
            foreach (Inventory inventory in _inventories.Where(x => x.ProductId == productId))
            {
                inventory.Quantity--;
            }
        }
    }
}