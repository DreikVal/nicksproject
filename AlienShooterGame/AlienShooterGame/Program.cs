using System;

namespace AlienShooterGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Application app = new Application())
            {
                app.Run();
            }
        }
    }
}

