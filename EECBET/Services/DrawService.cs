using System.Diagnostics;
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication1.Services
{
    public class DrawService
    {
        private static readonly Random _rand = new Random();
        private static int _issueCounter = 1;

        /// <summary>
        /// 評估玩家投注結果
        /// </summary>
        public DrawResult Evaluate(BetRequest bet)
        {
            var drawNumbers = GenerateDrawNumbers(bet.Numbers);

            int matchingCount = bet.Numbers.Count(n => drawNumbers.Contains(n));

            var wins = new List<WinDetail>();
            int totalPayout = 0;

            for (int n = 2; n <= 4; n++)
            {
                int winGroups = CountMatches(bet.Numbers, drawNumbers, n);
                int payoutPerGroup = GetPayout(n);
                if (winGroups > 0)
                {
                    var detail = new WinDetail
                    {
                        Type = n,
                        WinningGroups = winGroups,
                        PayoutPerGroup = payoutPerGroup,
                        TotalPayout = winGroups * payoutPerGroup
                    };
                    wins.Add(detail);
                    totalPayout += detail.TotalPayout;
                }
            }

            return new DrawResult
            {
                IssueNo = _issueCounter++,
                DrawNumbers = drawNumbers,
                Numbers = bet.Numbers,
                MatchingCount = matchingCount,
                Wins = wins,
                TotalPayout = totalPayout
            };
        }

        /// <summary>
        /// 生成開獎號碼，中獎率 10%-50%
        /// </summary>
        private List<int> GenerateDrawNumbers(List<int> playerNumbers)
        {
            var numbers = Enumerable.Range(1, 39).ToList();
            var result = new List<int>();

            int maxWin = Math.Min(playerNumbers.Count, 5);
            int minWin = (int)Math.Ceiling(maxWin * 0.1);
            int maxWinCount = (int)Math.Floor(maxWin * 0.5);

            int winCount = _rand.Next(minWin, maxWinCount + 1);

            var winNumbers = playerNumbers.OrderBy(x => _rand.Next()).Take(winCount).ToList();
            result.AddRange(winNumbers);

            while (result.Count < 5)
            {
                int n = _rand.Next(1, 40);
                if (!result.Contains(n))
                    result.Add(n);
            }

            result.Sort();
            return result;
        }

        private int CountMatches(List<int> betNumbers, List<int> drawNumbers, int n)
        {
            if (betNumbers.Count < n) return 0;
            var combinations = GetCombinations(betNumbers, n);
            int count = 0;
            foreach (var combo in combinations)
            {
                if (combo.All(x => drawNumbers.Contains(x)))
                    count++;
            }
            return count;
        }

        private int GetPayout(int n) => n switch
        {
            2 => 50,
            3 => 150,
            4 => 1000,
            _ => 0
        };

        private IEnumerable<List<int>> GetCombinations(List<int> list, int length)
        {
            if (length == 0) return new List<List<int>> { new List<int>() };

            return list.SelectMany((e, i) =>
                GetCombinations(list.Skip(i + 1).ToList(), length - 1)
                    .Select(c => (new List<int> { e }).Concat(c).ToList())
            );
        }
    }
}
