using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(Player.eColor i_PlayerColor)
        {
            // Human player c'tor
            m_PlayerColor = i_PlayerColor;
        }
    }
}
