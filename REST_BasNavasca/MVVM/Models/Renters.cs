using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_BasNavasca.MVVM.Models
{

    public class Rootobject
    {
        public Renters[] Renter { get; set; }
    }

    public class Renters
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public string VehicleModel { get; set; }
        public string Profile { get; set; }
        public bool Status { get; set; }
        public string id { get; set; }
    }



}
