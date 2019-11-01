using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public class PcPlayer : Player
    {
        public void PickAMove(Board i_GameBoard, out int o_CurrentMoveRowIndex, out int o_CurrentMoveColumnIndex)
        {
            // this method is activating PCPlay method from AI class and calls a message from UI
            AI.PCPlay(i_GameBoard, out o_CurrentMoveRowIndex, out o_CurrentMoveColumnIndex);
        }

        public PcPlayer(Player.eColor i_PlayerColor)
        {
            // PcPlayer c'tor
            m_PlayerColor = i_PlayerColor;
        }
    }
}
