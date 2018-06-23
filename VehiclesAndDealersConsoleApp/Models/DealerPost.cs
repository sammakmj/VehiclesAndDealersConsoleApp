using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sammak.VnD.Models
{
    public class DealerPost
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public List<VehiclePost> Vehicles { get; set; }
    }
}
