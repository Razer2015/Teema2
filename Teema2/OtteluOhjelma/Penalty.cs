namespace Teema2.OtteluOhjelma
{
    public class Penalty
    {
        public int TeamId { get; set; }
        public int Penalties { get; set; }

        public Penalty(int teamId, int penalties) {
            this.TeamId = teamId;
            this.Penalties = penalties;
        }
    }
}
