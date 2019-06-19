using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class PowerTradesReportExporter : IPowerTradesReportExporter
    {
        public void Export(PowerTrade aggregatedTrade, string directoryPath)
        {
            try
            {
                DateTimeManager.ConvertPowerTradeDateTimeToLocal(aggregatedTrade);

                var data = DateTimeManager.EnrichDataWithDates(aggregatedTrade);

                var fullPath = CombineFullPathToExport(directoryPath, aggregatedTrade);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                int suffix = 1;
                while(File.Exists(fullPath))
                {
                    fullPath = CombineFullPathToExport(directoryPath, aggregatedTrade, suffix.ToString());
                    suffix++;
                }

                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(data);
                }
            }
            catch(Exception e)
            {
                throw new Exception("Report export failed", e.InnerException);
            }
        }

        private string CombineFullPathToExport(string directoryPath, PowerTrade aggregatedTrade, string suffix = "")
        {
            string dateDayPart = aggregatedTrade.CreatedDate.ToString("yyyyMMdd");
            string dateTimePart = aggregatedTrade.CreatedDate.TimeOfDay.ToString("hhmm");
            string extension = "csv";
            if (!String.IsNullOrWhiteSpace(suffix))
            {
                suffix = $"_{suffix}";
            }
            return $"{directoryPath}\\PowerPosition_" +
                    $"{dateDayPart}_" +
                    $"{dateTimePart}{suffix}.{extension}";
        }
    }
}
