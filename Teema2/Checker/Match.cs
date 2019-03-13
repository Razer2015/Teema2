namespace Teema2.Checker
{
    public class Match
    {
        public int Home { get; set; }
        public int Visitor { get; set; }
        public int Round { get; set; }

        public Match(int home, int visitor, int round) {
            Home = home;
            Visitor = visitor;
            Round = round;
        }
    }
}
