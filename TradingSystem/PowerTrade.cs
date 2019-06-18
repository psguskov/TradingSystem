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
            CreatedDate = DateTime.Now;
        }

        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; private set; }
        public double[] Volumes { get; set; }


        internal void AddVolumeByPeriod(int period, double volume)
        {
            if (period < 1 || period > Volumes.Length)
            {
                throw new ArgumentException("Incorrect period", "period");
            }
            Volumes[period-1] += volume;
        }
    }
}
