using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ex05_Othello.Logic;

namespace Ex05_Othello.UI
{
    public partial class FormGameSettings : Form
    {
        private Board.eBoardSize m_BoardSize = Board.eBoardSize.size6x6;
        private GameLogic.eGameMode m_GameMode;
        private bool m_FormClosedByUser = true;

        public FormGameSettings()
        {
            InitializeComponent();
            this.Icon = Ex05_Othello.UI.Resource1.icon;
        }

        private void buttonBoardSize_Click(object i_Sender, EventArgs i_E)
        {
            // this method is representing an event for changing the board size.
            changeBoardSize();
        }

        private void changeBoardSize()
        {
            // this method is changing the game board size in game settings.
            string incOrDec;
            int maxBoardSize, minBoardSize;

            maxBoardSize = (int)Board.eBoardSize.size12x12;
            minBoardSize = (int)Board.eBoardSize.size6x6;

            m_BoardSize += (int)m_BoardSize == maxBoardSize ? -(maxBoardSize - minBoardSize) : 2;
            incOrDec = (int)m_BoardSize == maxBoardSize ? "decrease" : "increase";
            buttonBoardSize.Text = string.Format("Board size: {0}x{0} (click to {1})", (int)m_BoardSize, incOrDec);
        }

        public Board.eBoardSize BoardSize
        {
            get
            {

                return m_BoardSize;
            }

            set
            {
                m_BoardSize = value;
            }
        }

        public GameLogic.eGameMode GameMode
        {
            get
            {

                return m_GameMode;
            }

            set
            {
                m_GameMode = value;
            }
        }

        public bool XButtonWasPressed
        {
            get
            {
                return m_FormClosedByUser;
            }
        }

        private void buttonPlayHumanVsPC_Click(object i_Sender, EventArgs i_E)
        {
            // this method represents an event for HumanVsPc button was clicked.
            m_GameMode = GameLogic.eGameMode.HumanVsPC;
            m_FormClosedByUser = false;
            Close();
        }

        private void buttonPlayHumanVsHuman_Click(object i_Sender, EventArgs i_E)
        {
            // this method represents an event for HumanVsHuman button was clicked.
            m_GameMode = GameLogic.eGameMode.HumanVsHuman;
            m_FormClosedByUser = false;
            Close();
        }
    }
}
