using System.Collections.Generic;

namespace EECBET.Models
{
    public class BetRequest
    {
        public List<int> Numbers { get; set; } = new List<int>();
        public int Bet2 { get; set; }
        public int Bet3 { get; set; }
        public int Bet4 { get; set; }
        public int TotalAmount { get; set; }
    }

    public class LottoBetRequest
    {
        public List<int> Numbers { get; set; } = new List<int>();
        public int BetAmount { get; set; }
        public int BetCount { get; set; }
        public int TotalAmount { get; set; }
    }

    public class PowerBetRequest
    {
        public List<int> FirstAreaNumbers { get; set; } = new List<int>();
        public List<int> SecondAreaNumbers { get; set; } = new List<int>();
        public int BetAmount { get; set; }
        public int BetCount { get; set; }
        public int TotalAmount { get; set; }
    }

    public class DoubleColorBetRequest
    {
        public List<int> RedNumbers { get; set; } = new List<int>();
        public List<int> BlueNumbers { get; set; } = new List<int>();
        public int BetAmount { get; set; }
        public int BetCount { get; set; }
        public int TotalAmount { get; set; }
    }

    public class Today539BetRequest
    {
        public List<int> Numbers { get; set; } = new List<int>();
        public int BetAmount { get; set; }
        public int BetCount { get; set; }
        public int TotalAmount { get; set; }
    }
}
