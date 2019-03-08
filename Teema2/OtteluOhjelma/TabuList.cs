using System.Collections.Generic;

namespace Teema2.OtteluOhjelma
{
    public class TabuList
    {
        const int MAX_SIZE = 1;
        int _lastIndex = 0;
        private Dictionary<int, Match> _tabList = new Dictionary<int, Match>();

        /// <summary>
        ///     Add match to tabulist
        /// </summary>
        public void Add(Match match) {
            if (!_tabList.ContainsValue(match)) {
                if (_tabList.Count >= MAX_SIZE)
                    _tabList.Remove(_lastIndex - MAX_SIZE);
                _tabList.Add(_lastIndex, match);
                _lastIndex++;
            }
        }

        /// <summary>
        ///     Check if tabulist contains match
        /// </summary>
        public bool Contains(Match match) {
            return _tabList.ContainsValue(match);
        }
    }
}
