using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public class Departament: ADepartament
    {
        public override List<AClient> Clients { get => Bank.Clients.Where(x => (x is Client && (x as Client).isVIP == false)).ToList(); }

        public Departament(Bank Bank, string Name) : base(Bank, Name) { }
    }
}
