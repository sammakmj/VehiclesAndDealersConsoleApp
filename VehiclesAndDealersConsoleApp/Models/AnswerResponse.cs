using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sammak.VnD.Models
{
    public class AnswerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalMilliseconds { get; set; }
    }
}
