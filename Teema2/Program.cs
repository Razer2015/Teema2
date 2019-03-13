using System.IO;
using Teema2.Checker;
using Teema2.OtteluOhjelma;

namespace Teema2
{
    class Program
    {
        static void Main(string[] args) {
            // Check the validity of a game schedule
            //var checkerProgram = new CheckerProgram(File.ReadAllLines("schedule.txt"), 12);
            //var gamesOnce = checkerProgram.IsOnceInARound();
            //var homeOnce = checkerProgram.PlaysOnceHomeAgainstEveryone();
            //var visitOnce = checkerProgram.PlaysOnceVisitorAgainstEveryone();
            //if (gamesOnce && homeOnce && visitOnce)
            //    System.Console.WriteLine("All good!");
            //else
            //    System.Console.WriteLine("Something went wrong!");

            // Do GameSchedule
            new GameProgram(12);
        }
    }
}
