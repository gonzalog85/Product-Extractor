using System.Collections.Generic;

namespace Infrastructure.Data.Models
{
    public class RootObject
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public List<ProductObject> Products { get; set; }
    }
}
