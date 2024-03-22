using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class CustomerViewModel
    {
        public Customer customer { get; set; }
        public List<Customer>? customerlist { get; set; }
    }
}
