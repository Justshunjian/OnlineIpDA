using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA.entity
{
    class QuartzTimer
    {
        public bool state { get; set; }
        public int type { get; set; }
        public int weekday { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
    }
}
