namespace DataVisionAPI.Models.DTOs
{
    public class ReportDto
    {
        public string Type { get; set; } = string.Empty; // "temperature", "humidity", "pressure"
        public List<ChartDataPoint> Data { get; set; } = new List<ChartDataPoint>();
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }

    public class ChartDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public string? Category { get; set; }
    }
}