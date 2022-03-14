using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Extractor.Models
{
    public class RootObject
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public List<ProductObject> Products { get; set; }
    }
}
