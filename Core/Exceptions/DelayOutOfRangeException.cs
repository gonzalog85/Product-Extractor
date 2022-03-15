using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class DelayOutOfRangeException : Exception
    {
        public DelayOutOfRangeException() : base("Diley fuera de rango permitido. No debe ser menor a 3 minutos")
        {

        }

        public DelayOutOfRangeException(string message) : base(message)
        {

        }
    }
}
