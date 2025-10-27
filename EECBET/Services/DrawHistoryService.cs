using EECBET.Models;

namespace EECBET.Services
{
    public class DrawHistoryService
    {
        private static readonly List<DrawResult> _drawHistory = new List<DrawResult>();
        private static readonly object _lock = new object();

        public DrawHistoryService()
        {
        }

        public void SaveDrawResult(DrawResult result)
        {
            lock (_lock)
            {
                _drawHistory.Add(result);
            }
        }

        public List<DrawResult> GetRecentDraws(int count)
        {
            lock (_lock)
            {
                return _drawHistory
                    .OrderByDescending(d => d.IssueNo)
                    .Take(count)
                    .ToList();
            }
        }

        public int GetLatestIssueNumber()
        {
            lock (_lock)
            {
                return _drawHistory.Count > 0 
                    ? _drawHistory.Max(d => d.IssueNo) 
                    : 0;
            }
        }
    }
}
