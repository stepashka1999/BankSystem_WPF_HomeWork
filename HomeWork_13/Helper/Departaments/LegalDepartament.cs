using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_13
{
    public class LegalDepartament: ADepartament
    {
        public override List<AClient> Clients { get => Bank.Clients.Where( x => (x is Entity)).ToList(); }

        public LegalDepartament(Bank Bank, string Name) : base(Bank, Name) { }
    }
}
