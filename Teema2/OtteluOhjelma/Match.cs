namespace Teema2.OtteluOhjelma
{
    public class Match
    {
        public int Home { get; set; }
        public int Visitor { get; set; }

        public Match(int home, int visitor) {
            this.Home = home;
            this.Visitor = visitor;
        }
    }
}
