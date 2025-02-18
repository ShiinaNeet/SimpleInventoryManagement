using SimpleInventoryManagement.DTO;
using SimpleInventoryManagement.Interface;
using SimpleInventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryManagement.Service
{
    public class InventoryManager : IInventoryManager
    {
        private List<Product> _products = new();
        public List<Product> ListProducts()
        {
            return _products;
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                Console.WriteLine($"Add Product Method: Missing Product parameter.");
                throw new ArgumentNullException(nameof(product));
            }
            if (_products.Contains(product))
            {
                throw new ArgumentException("Product already exists in inventory.");
            }

            product.ProductId = GenerateProductId();
            _products.Add(product);

            Console.WriteLine("Product Added Successfully.");
        }

        public void RemoveProduct(int productId)
        {
            if (productId == 0)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            var productToRemove = _products.FirstOrDefault(x => x.ProductId == productId);
            if (productToRemove != null)
            {
                _products.Remove(productToRemove);
                Console.WriteLine("Product Removed Successfully.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        //I have used DTO instead of int productId & int newQuantity.
        //It is much better to use DTO for future proofing as other Product class properties can be updated in the future.
        public void UpdateProduct(UpdateProductRequestDTO product)
        {
            if (product == null)
            {
                Console.WriteLine($"Update Product Method: Missing Product parameter.");
                throw new ArgumentNullException(nameof(product));
            }
            var productToUpdate = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (productToUpdate == null)
            {
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine("Product not found.");
                Console.WriteLine(new string('=', Console.WindowWidth));
                return;
            }
            productToUpdate.ProductId = product.ProductId;
            productToUpdate.QuantityInStock = product.newQuantity;
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine("Product Updated Successfully.");
            Console.WriteLine(new string('=', Console.WindowWidth));
        }

        public double GetTotalValue()
        {
            return _products.Sum(x => x.Price * x.QuantityInStock);
        }

        private static int GenerateProductId()
        {
            return Math.Abs((int)(DateTime.Now.Ticks + Random.Shared.Next(0, int.MaxValue)));
        }
    }
}
