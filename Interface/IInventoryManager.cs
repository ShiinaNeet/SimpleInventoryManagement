using SimpleInventoryManagement.DTO;
using SimpleInventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryManagement.Interface
{
    public interface IInventoryManager
    {
        List<Product> ListProducts();
        void AddProduct(Product product);
        void RemoveProduct(int productId);
        void UpdateProduct(UpdateProductRequestDTO productRequestDTO);
        double GetTotalValue();
    }
}
