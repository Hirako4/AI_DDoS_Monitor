using System;

namespace AI_DDoS_Monitor.Models
{
    public class Run
    {
        public int Id { get; set; }
        public int ExperimentId { get; set; }
        public int DatasetId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public decimal? Accuracy { get; set; }
    }
}