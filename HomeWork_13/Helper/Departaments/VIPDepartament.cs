using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_13
{ 
    public class VIPDepartament: ADepartament
    {
        public override List<AClient> Clients { get => Bank.Clients.Where(x => (x is Client && (x as Client).isVIP == true)).ToList(); }

        public VIPDepartament(Bank Bank, string Name) : base(Bank, Name) { }
    }
}
