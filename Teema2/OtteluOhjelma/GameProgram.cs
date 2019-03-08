using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Teema2.OtteluOhjelma
{
    public class GameProgram
    {
        static Random rand = new Random();
        int TeamCount { get; }
        Round[] Rounds { get; set; }
        TabuList TabuList { get; set; }
        int TotalMoves = 0;

        public GameProgram(int teamCount) {
            this.TeamCount = teamCount;

            int giveUp = 10000;
            while (giveUp-- > 0) {
                InitGames();

                var totalPenalty = GetTotalPenalty();
                if (totalPenalty == 0) {
                    Console.WriteLine("Wut! Total penalty is zero before any moves.");
                    return;
                }

                Console.WriteLine($"Total Penalty: {totalPenalty}");

                var firstMove = GetFirstMove();
                var (Round, Match) = MoveBestRound(firstMove);
                var moves = 1;
                while (moves <= 1000) {
                    var worstMatch = GetWorstMatch(Round, Match);
                    if (worstMatch == null) break;
                    (Round, Match) = MoveBestRound(worstMatch);
                    moves++;
                    if (GetTotalPenalty() <= 0) break;
                }

                if (GetTotalPenalty() <= 0) {
                    Console.WriteLine();
                    Console.WriteLine($"Success with {TotalMoves} moves");
                    //PrintByRound();
                    //Console.WriteLine();
                    PrintGame();
                    SaveGame();
                    Console.ReadKey();
                }
                Console.WriteLine(GetTotalPenalty());
                Console.WriteLine("Attempt failed");
            }
        }

        /// <summary>
        ///     Initialize the games list by setting the games in random rounds
        ///     Also counts the penalties after that
        /// </summary>
        private void InitGames() {
            TotalMoves = 0;
            TabuList = new TabuList();
            Rounds = new Round[TeamCount % 2 == 0 ? (TeamCount - 1) * 2 : TeamCount * 2];
            for (int i = 0; i < Rounds.Length; i++) {
                Rounds[i] = new Round(TeamCount);
            }

            for (int i = 0; i <= TeamCount; i++) {
                for (int y = i + 1; y <= TeamCount - 1; y++) {
                    // Home game
                    var unfilledRounds = Rounds.Where(x => x.Matches.Count < 6).ToList();
                    var random = rand.Next(0, unfilledRounds.Count - 1);
                    var round = unfilledRounds[random];
                    round.Matches.Add(new Match(i, y));

                    // Visitor game
                    unfilledRounds = Rounds.Where(x => x.Matches.Count < 6).ToList();
                    random = rand.Next(0, unfilledRounds.Count - 1);
                    round = unfilledRounds[random];
                    round.Matches.Add(new Match(y, i));

                    //var random = rand.Next(0, Rounds.Length - 1);
                    //var round = Rounds[random];
                    //round.Matches.Add(new Match(i, y));
                    //random = rand.Next(0, Rounds.Length - 1);
                    //round = Rounds[random];
                    //round.Matches.Add(new Match(y, i));
                }
            }

            // Count penalties
            foreach (var round in Rounds) {
                round.CountPenalties();
            }
        }

        /// <summary>
        ///     Move round from round to another
        /// </summary>
        private Round MoveRound(int prevRound, int newRound, Match match) {
            // Remove from old
            Rounds[prevRound].Matches.Remove(match);
            Rounds[prevRound].CountPenalties();

            // Add to new
            Rounds[newRound].Matches.Add(match);
            Rounds[newRound].CountPenalties();

            // Add to tabulist
            TabuList.Add(match);

            TotalMoves++;
            Console.WriteLine($"Moved match {match.Home + 1}:{match.Visitor + 1} from {prevRound + 1} to {newRound + 1}");
            Console.WriteLine($"Total penalties {GetTotalPenalty()}");

            return Rounds[newRound];
        }

        /// <summary>
        ///     Get total penalties of the game (all rounds combined)
        /// </summary>
        /// <returns></returns>
        private int GetTotalPenalty() {
            return Rounds.Sum(x => x.RoundPenalties.GetTotalPenalties());
        }

        /// <summary>
        ///     Get the first game move by random
        /// </summary>
        /// <returns></returns>
        private Match GetFirstMove() {
            while (true) {
                var roundCandidate = rand.Next(0, Rounds.Length - 1);
                var round = Rounds[roundCandidate];
                var matches = round.Matches.Where(x => !TabuList.Contains(x)).ToList();
                if (matches.Count >= 1) {
                    return matches[rand.Next(0, matches.Count - 1)];
                }
            }
        }

        /// <summary>
        ///     Select the worst match of the chosen round (exclude previous move)
        /// </summary>
        /// <param name="roundId"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        private Match GetWorstMatch(int roundId, Match ignore) {
            var round = Rounds[roundId];
            var matches = round.Matches.Where(x => !TabuList.Contains(x) && (x.Home != ignore.Home && x.Visitor != ignore.Visitor)).ToList();
            List<(Match match, int penalties)> matchList = new List<(Match, int)>();
            foreach (var m in matches) {
                matchList.Add((m, round.GetMatchPenalties(m.Home, m.Visitor)));
            }

            if (matchList.Count <= 0) {
                return null;
                //return GetFirstMove();
            }

            var maxs = matchList.Where(x => x.penalties == matchList.Max(y => y.penalties)).ToList();
            return maxs[rand.Next(0, maxs.Count - 1)].match;
        }

        /// <summary>
        ///     Move the chosen match to the round that gives the lowest total penalty count
        /// </summary>
        private (int Round, Match match) MoveBestRound(Match match) {
            int totalPenaltyBefore = GetTotalPenalty();

            int roundId = -1;
            List<(int round, int penalty)> penaltys = new List<(int round, int penalty)>();
            for (int i = 0; i < Rounds.Length; i++) {
                if (Rounds[i].Matches.Contains(match)) {
                    roundId = i;
                    continue;
                }

                var testRound = Rounds[i];
                testRound.Matches.Add(match);
                testRound.CountPenalties();

                penaltys.Add((i, GetTotalPenalty()));

                testRound.Matches.Remove(match);
                testRound.CountPenalties();
            }

            var mins = penaltys.Where(x => x.penalty == penaltys.Min(y => y.penalty)).ToList();
            var bestRound = mins[rand.Next(0, mins.Count - 1)].round;

            MoveRound(roundId, bestRound, match);

            return (bestRound, match);
        }

        /// <summary>
        ///     Just a little debug print
        /// </summary>
        private void PrintByRound() {
            for (int i = 0; i < Rounds.Length; i++) {
                Console.WriteLine($"Round {i + 1,2} penalties: {Rounds[i].GetTotalPenalties()}");
                foreach (var match in Rounds[i].Matches) {
                    var gamePenalties = Rounds[i].GetMatchPenalties(match.Home, match.Visitor);
                    Console.WriteLine($"Round {i + 1,2}: {match.Home + 1,2} - {match.Visitor + 1,2} | Penalties: {gamePenalties,2}");
                }
            }
        }

        /// <summary>
        ///     Print the game result
        /// </summary>
        private void PrintGame() {
            //Console.WriteLine("0 0 0");
            //Console.WriteLine("0 0 0");

            for (int i = 0; i < Rounds.Length; i++) {
                foreach (var match in Rounds[i].Matches) {
                    Console.WriteLine($"{i + 1} {match.Home + 1} {match.Visitor + 1}");
                }
            }
        }

        /// <summary>
        ///     Save the game result to a file
        /// </summary>
        private void SaveGame() {
            //Console.WriteLine("0 0 0");
            //Console.WriteLine("0 0 0");

            var gameRows = new List<string>();
            for (int i = 0; i < Rounds.Length; i++) {
                foreach (var match in Rounds[i].Matches) {
                    gameRows.Add($"{i + 1} {match.Home + 1} {match.Visitor + 1}");
                }
            }

            File.WriteAllLines("gameSchedule.txt", gameRows);
        }
    }
}
