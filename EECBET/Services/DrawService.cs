using System;
using System.Collections.Generic;
using System.Linq;
using EECBET.Models;

namespace EECBET.Services
{
    public class DrawService
    {
        private static readonly Random _rand = new Random();
        private static int _issueCounter = 1;

        private readonly DrawHistoryService _historyService;

        public DrawService(DrawHistoryService historyService)
        {
            _historyService = historyService;
            InitializeIssueCounter();
        }

        public DrawResult Evaluate(BetRequest bet)
        {
            if (bet == null)
                bet = new BetRequest { Numbers = new List<int>() };

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
                        //PayoutPerGroup = payoutPerGroup,
                        TotalPayout = winGroups * payoutPerGroup
                    };
                    wins.Add(detail);
                    totalPayout += detail.TotalPayout;
                }
            }

            var result = new DrawResult
            {
                IssueNo = _issueCounter++,
                DrawNumbers = drawNumbers,
                Numbers = bet.Numbers,
                MatchingCount = matchingCount,
                Wins = wins,
                TotalPayout = totalPayout,
                DrawTime = DateTime.Now
            };

            // 儲存開獎紀錄到資料庫
            _historyService.SaveDrawResult(result);

            return result;
        }

        public List<DrawResult> GetRecentDraws(int count)
        {
            return _historyService.GetRecentDraws(count);
        }

        private void InitializeIssueCounter()
        {
            try
            {
                var latestIssueNumber = _historyService.GetLatestIssueNumber();
                _issueCounter = latestIssueNumber + 1;
            }
            catch (Exception ex)
            {
                // 如果獲取失敗，使用預設值1
                Console.WriteLine($"Error initializing issue counter: {ex.Message}");
                _issueCounter = 1;
            }
        }

        private List<int> GenerateDrawNumbers(List<int> playerNumbers)
        {
            var result = new List<int>();

            int maxWin = Math.Min(playerNumbers.Count, 5);
            int minWin = (int)Math.Ceiling(maxWin * 0.5);
            int maxWinCount = (int)Math.Floor(maxWin * 0.7);

            int winCount = 0;
            if (maxWin >= 1 && maxWinCount >= minWin)
            {
                winCount = _rand.Next(minWin, maxWinCount + 1);
            }

            if (winCount > 0)
            {
                var winNumbers = playerNumbers.OrderBy(x => _rand.Next()).Take(winCount).ToList();
                result.AddRange(winNumbers);
            }

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
            if (betNumbers.Count < n)
                return 0;

            var combos = GetCombinations(betNumbers, n);
            int cnt = 0;
            foreach (var combo in combos)
            {
                if (combo.All(x => drawNumbers.Contains(x)))
                    cnt++;
            }
            return cnt;
        }

        private int GetPayout(int n) => n switch
        {
            2 => 100,
            3 => 500,
            4 => 10000,
            5 => 50000,
            _ => 0
        };

        private IEnumerable<List<int>> GetCombinations(List<int> list, int length)
        {
            if (length == 0)
                return new List<List<int>> { new List<int>() };

            return list.SelectMany((e, i) =>
                GetCombinations(list.Skip(i + 1).ToList(), length - 1)
                    .Select(c =>
                    {
                        var newList = new List<int> { e };
                        newList.AddRange(c);
                        return newList;
                    })
            );
        }
        public class DatabaseConnection
        {
            public required string _connStr { get; set; }  // 使用 'required' 修飾符

            public DatabaseConnection(string connectionString)
            {
                _connStr = connectionString;
            }
        }



    }
}
