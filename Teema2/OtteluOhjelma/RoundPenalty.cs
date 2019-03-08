using System.Collections.Generic;
using System.Linq;

namespace Teema2.OtteluOhjelma
{
    public class RoundPenalty
    {
        readonly int TeamCount;
        Penalty[] TeamPenalties;

        public RoundPenalty(int teamCount) {
            TeamCount = teamCount;

            ResetPenalties();
        }

        /// <summary>
        ///     Reset round penalties
        /// </summary>
        public void ResetPenalties() {
            TeamPenalties = new Penalty[TeamCount];
            for (int i = 0; i < TeamPenalties.Length; i++) {
                TeamPenalties[i] = new Penalty(i, 0);
            }
        }

        /// <summary>
        ///     Count penalties for a round
        /// </summary>
        /// <param name="matches"></param>
        public void CountRoundMatchPenalties(List<Match> matches) {
            for (int i = 0; i < TeamPenalties.Length; i++) {
                var penalty = TeamPenalties[i];

                var occurences = matches.Count(x => x.Home == i || x.Visitor == i);
                if (occurences != 1) {
                    if (occurences < 1) {
                        penalty.Penalties++;
                    }
                    else {
                        penalty.Penalties += occurences - 1;
                    }
                    TeamPenalties[i] = penalty;
                }
            }
        }

        /// <summary>
        ///     Get total penalties of a round
        /// </summary>
        public int GetTotalPenalties() {
            return TeamPenalties.Sum(x => x.Penalties);
        }

        /// <summary>
        ///     Get match penalties
        /// </summary>
        public int GetMatchPenalties(int home, int visitor) {
            return TeamPenalties[home].Penalties + TeamPenalties[visitor].Penalties;
        }
    }
}
