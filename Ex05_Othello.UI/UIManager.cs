using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ex05_Othello.Logic;

namespace Ex05_Othello.UI
{
    public class UIManager
    {
        private GameLogic m_GameLogic = new GameLogic();

        public void Run()
        {
            // this method maintains the main loop for the game
            FormGameSettings formGameSettings;
            DialogResult dialogResult;
            
            visualStyles();
            formGameSettings = startFormGameSettings();
            if (!formGameSettings.XButtonWasPressed)
            {
                do
                {
                    startFormOthello(formGameSettings);
                    updateWinnerOverallScore();
                    dialogResult = endOfRoundMessageBox(m_GameLogic);
                    restartGame();
                }
                while (dialogResult == DialogResult.Yes);
            }
        }

        private static FormGameSettings startFormGameSettings()
        {
            // this method excute form game settings and return the form.
            FormGameSettings formGameSettings = new FormGameSettings();
            Application.Run(formGameSettings);

            return formGameSettings;
        }

        private void updateWinnerOverallScore()
        {
            // this method updates the winner overall score
            m_GameLogic.UpdateWinnerOverallScore();
        }

        private void startFormOthello(FormGameSettings i_FormGameSettings)
        {
            // this method execute form othello
            FormOthello formOthello;

            formOthello = new FormOthello(m_GameLogic, i_FormGameSettings.BoardSize, i_FormGameSettings.GameMode);
            formOthello.Initialize();
            Application.Run(formOthello);
        }

        private void restartGame()
        {
            // this method is doing a logic restart.
            m_GameLogic.RestartGame();
        }

        private DialogResult endOfRoundMessageBox(GameLogic i_GameLogic)
        {
            // this method shows an end of round message box.
            bool isGameEndedInTie;
            int yellowPlayerRoundScore, redPlayerRoundScore, yellowPlayerOverallScore, redPlayerOverallScore;
            string winnerColor, endOfRoundMessage;
            DialogResult dialogResult;

            i_GameLogic.getPlayersCurrentRoundScores(out yellowPlayerRoundScore, out redPlayerRoundScore);
            i_GameLogic.getPlayersOverallScores(out yellowPlayerOverallScore, out redPlayerOverallScore);
            i_GameLogic.getCurrentRoundWinner(out winnerColor, out isGameEndedInTie);
            if (isGameEndedInTie)
            {
                endOfRoundMessage = string.Format("Its a DRAW !!({0}/{1})({2}/{3}){4}Would you like another round?", redPlayerRoundScore, yellowPlayerRoundScore,
                   redPlayerOverallScore, yellowPlayerOverallScore, Environment.NewLine);
            }
            else
            {
                endOfRoundMessage = string.Format("{0} Won!!({1}/{2})({3}/{4}){5}Would you like another round?", winnerColor, redPlayerRoundScore, yellowPlayerRoundScore,
                   redPlayerOverallScore, yellowPlayerOverallScore, Environment.NewLine);
            }

            dialogResult = MessageBox.Show(endOfRoundMessage, "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            return dialogResult;
        }

        private void visualStyles()
        {
            // default styling forms methods.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
