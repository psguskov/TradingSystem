using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class PowerTrade
    {
        public PowerTrade(int periodsCount)
        {
            Volumes = new double[periodsCount];
            CreatedDate = DateTime.UtcNow;
        }

        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public double[] Volumes { get; set; }
    }
}
