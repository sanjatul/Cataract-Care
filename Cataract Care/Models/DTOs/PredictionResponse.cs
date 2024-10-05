namespace Cataract_Care.Models.DTOs
{
    public class PredictionResponse
    {
        public string PredictedClass { get; set; }
        public double ConfidenceScore { get; set; }
    }
}
