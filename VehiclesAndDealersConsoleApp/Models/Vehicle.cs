using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sammak.VnD.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int DealerId { get; set; }
    }
}
