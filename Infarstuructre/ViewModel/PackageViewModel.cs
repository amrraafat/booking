using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class PackageViewModel
    {
        public Package Package { get; set; }
        public List<Hotel> hotels { get; set; }
    }
}
