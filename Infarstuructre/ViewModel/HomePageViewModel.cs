using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class HomePageViewModel
    {
        public Hotel Hotel { get; set; }
        public string ImageFileName { get; set; }
        public List<Package> Packages { get; set; }
    }
}
