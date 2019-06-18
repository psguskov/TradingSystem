namespace TradingSystem
{
    public interface IPowerTradesReportExporter
    {
        void Export(PowerTrade aggregatedTrade, string path);
    }
}