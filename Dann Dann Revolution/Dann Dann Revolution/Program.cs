/*
 * Alex Kitt, Mike Hodges, Vaughn Gregory, and Noah Sweetnam
 * AKA. Dann Dann Revolution Team
 *
 * December 18, 2017 - closing day for The Music Man :(
 * CompSci 40S
 * 
 * dane dna resolution: sorry, I meant:
 * Dann Dann Revolution: This game has the player use the arrow keys to make Mr. Dann do slick moves in time with several hit songs!
 */

using System;

namespace Dann_Dann_Revolution
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new DannDannRevolution())
                game.Run();
        }
    }
#endif
}
