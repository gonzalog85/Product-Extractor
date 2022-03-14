using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Extractor.Models
{
    public class ProductObject
    {
        public string Code { get; set; }
        public string Sku { get; set; }
        public decimal Stock { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public decimal Iva { get; set; }
        public decimal Ii { get; set; }
    }
}
