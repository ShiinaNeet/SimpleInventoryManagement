using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryManagement.DTO
{
    public class UpdateProductRequestDTO
    {
        public required int ProductId { get; set; }
        public required int newQuantity { get; set; }
    }
}
