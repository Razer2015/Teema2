using System.Collections.Generic;

namespace Teema2.OtteluOhjelma
{
    public class Round
    {
        public List<Match> Matches { get; }
        public RoundPenalty Penalty { get; }

        public Round(int teamCount) {
            this.Matches = new List<Match>();
            this.Penalty = new RoundPenalty(teamCount);
        }

        /// <summary>
        ///     Reset and count round penalties
        /// </summary>
        public void CountPenalties() {
            Penalty.ResetPenalties();
            Penalty.CountRoundMatchPenalties(Matches);
        }

        /// <summary>
        ///     Get total penalties count for a round
        /// </summary>
        /// <returns></returns>
        public int GetTotalPenalties() {
            return Penalty.GetTotalPenalties();
        }

        /// <summary>
        ///     Get total match penalties
        /// </summary>
        public int GetMatchPenalties(int home, int visitor) {
            return Penalty.GetMatchPenalties(home, visitor);
        }
    }
}
