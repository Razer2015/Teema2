using System;
using System.Collections.Generic;
using System.Linq;

namespace Teema2.Checker
{
    public class CheckerProgram
    {
        private readonly int _teamCount;
        private readonly int _rounds;
        private List<Match> _matches;

        public CheckerProgram(string content, int teamCount) {
            _teamCount = teamCount;
            _rounds = _teamCount % 2 == 0 ? (_teamCount - 1) * 2 : _teamCount * 2;
            ParseContent(content.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            ));
        }

        public CheckerProgram(string[] content, int teamCount) {
            _teamCount = teamCount;
            _rounds = _teamCount % 2 == 0 ? (_teamCount - 1) * 2 : _teamCount * 2;
            ParseContent(content);
        }

        private void ParseContent(string[] lines) {
            _matches = new List<Match>();
            lines.ToList().ForEach(x => {
                var parts = x.Split(' ');
                if (parts.Length != 3) throw new Exception("Error: Invalid data.");

                _matches.Add(new Match(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[0])));
            });
        }

        public bool IsOnceInARound() {
            for (int i = 0; i < _rounds; i++) {
                var matches = _matches.Where(x => x.Round == i + 1).ToList();
                for (int y = 0; y < _teamCount; y++) {
                    if (matches.Count(x => x.Home == y + 1 || x.Visitor == y + 1) != 1)
                        return false;
                }
            }

            return true;
        }

        public bool PlaysOnceHomeAgainstEveryone() {
            for (int i = 0; i <= _teamCount; i++) {
                for (int y = i + 1; y <= _teamCount - 1; y++) {
                    if (_matches.Count(x => x.Home == i + 1 && x.Visitor == y + 1) != 1)
                        return false;

                }
            }

            return true;
        }

        public bool PlaysOnceVisitorAgainstEveryone() {
            for (int i = 0; i <= _teamCount; i++) {
                for (int y = i + 1; y <= _teamCount - 1; y++) {
                    if (_matches.Count(x => x.Home == y + 1 && x.Visitor == i + 1) != 1)
                        return false;

                }
            }

            return true;
        }
    }
}
