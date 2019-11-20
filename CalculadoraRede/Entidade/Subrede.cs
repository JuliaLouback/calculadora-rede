using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraRede.Entidade
{
    class Subrede
    {
        public string MascaraCustomizada { get; set; }

        public int NumeroSubrede { get; set; }

        public string Broadcast { get; set; }

        public string IdRede { get; set; }

        public int TotalHost { get; set; }
    }
}
