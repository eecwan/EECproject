using System;
using System.Collections.Generic;

namespace EECBET.Models
{
    public class DrawResult
    {
        public int IssueNo { get; set; }
        public List<int> DrawNumbers { get; set; } = new();
        public List<WinDetail> Wins { get; set; } = new();
        public int TotalPayout { get; set; }
        public List<int> Numbers { get; set; } = new();
        public int MatchingCount { get; set; }
        public DateTime DrawTime { get; set; }
    }
}

