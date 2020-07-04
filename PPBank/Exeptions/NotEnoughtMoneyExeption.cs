using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    class NotEnoughtMoneyExeption: Exception
    {
        public override string Message => "Not enought money for action";
    }
}
