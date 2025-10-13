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
}
