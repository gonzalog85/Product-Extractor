using System.Collections.Generic;

namespace Product_Extractor.Models
{
    public class RootObject
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public List<ProductObject> Products { get; set; }
    }
}
