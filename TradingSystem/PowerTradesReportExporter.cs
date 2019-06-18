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
        public void Export(PowerTrade aggregatedTrade)
        {
            try
            {
                var data = EnrichData(aggregatedTrade);
                var directoryPath = ConfigurationManager.AppSettings["directoryPath"];
                string fileNameSuffix = ConfigurationManager.AppSettings["fileNameSuffix"];
                string dateDayPart = aggregatedTrade.Date.ToString("yyyyMMdd");
                string dareTimePart = aggregatedTrade.Date.TimeOfDay.ToString("hhmm");
                string extension = "csv";
                var fullPath = $"{directoryPath}\\{fileNameSuffix}_" +
                    $"{dateDayPart}_" +
                    $"{dareTimePart}.{extension}";

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
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

        private List<ReportPeriod> EnrichData(PowerTrade aggregatedTrade)
        {
            var enrichedData = new List<ReportPeriod>();

            for (int i = 0; i < aggregatedTrade.Volumes.Count(); i++)
            {
                //TODO - handle -1 hour correctly
                string time = aggregatedTrade.Date.Add(TimeSpan.FromHours(i - 1)).TimeOfDay.ToString(@"hh\:mm");
                enrichedData.Add(new ReportPeriod { Period = time, Value = aggregatedTrade.Volumes[i] });
            }

            return enrichedData;
        }
    }
}
