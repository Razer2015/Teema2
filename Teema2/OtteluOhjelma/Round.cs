using System.Collections.Generic;

namespace Teema2.OtteluOhjelma
{
    public class Round
    {
        public List<Match> Matches { get; }
        public RoundPenalty RoundPenalties { get; }

        public Round(int teamCount) {
            this.Matches = new List<Match>();
            this.RoundPenalties = new RoundPenalty(teamCount);
        }

        /// <summary>
        ///     Reset and count round penalties
        /// </summary>
        public void CountPenalties() {
            RoundPenalties.ResetPenalties();
            RoundPenalties.CountRoundMatchPenalties(Matches);
        }

        /// <summary>
        ///     Get total penalties count for a round
        /// </summary>
        /// <returns></returns>
        public int GetTotalPenalties() {
            return RoundPenalties.GetTotalPenalties();
        }

        /// <summary>
        ///     Get total match penalties
        /// </summary>
        public int GetMatchPenalties(int home, int visitor) {
            return RoundPenalties.GetMatchPenalties(home, visitor);
        }
    }
}
