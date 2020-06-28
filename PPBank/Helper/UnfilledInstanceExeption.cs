using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    class UnfilledInstanceExeption: Exception
    {
        public override string Message => "Class instance is empty";
    }
}
