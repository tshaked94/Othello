using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ex05_Othello.UI
{
    public static class Program
    {
        // This is Othello game by Tomer Shaked and Oran Sherf, 2nd year students in MTA.
        // Please notice,
        // When the game mode is set to Human Vs PC - The PC player will be the RED player!
        // Enjoy!
        [STAThread]
        public static void Main()
        {
            UIManager othello = new UIManager();
            othello.Run();
        }
    }
}
