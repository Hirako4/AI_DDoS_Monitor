using System;

namespace AI_DDoS_Monitor.Models
{
    public class Experiment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AttackType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}