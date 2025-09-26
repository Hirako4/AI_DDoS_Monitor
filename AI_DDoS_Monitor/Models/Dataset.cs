using System;
using System.Collections.Generic;

namespace AI_DDoS_Monitor.Models
{
    public class Dataset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal SizeMb { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}