using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class ReservationViewModel
    {

        public Reservation reservation { get; set; }
        public Hotel? hotel { get; set; }
        public Package? package { get; set; }
        public Customer customer { get; set; }
        public List<Customer> customersList { get; set; }
    }
}
