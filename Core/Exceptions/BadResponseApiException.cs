using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Extractor.Exceptions
{
    public class BadResponseApiException : Exception
    {
        public BadResponseApiException() : base("Error ocurrido en la respuesta del API")
        {

        }

        public BadResponseApiException(string message) : base(message)
        {

        }
    }
}
