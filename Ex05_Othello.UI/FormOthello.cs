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
    public partial class FormOthello : Form
    {
        private readonly Image r_RedImage = Properties.Resources.CoinRed;
        private readonly Image r_YellowImage = Properties.Resources.CoinYellow;
        private GameLogic m_GameLogic;

        public GameLogic GameLogic
        {
            get
            {
                return m_GameLogic;
            }
        }

        public FormOthello(GameLogic i_GameLogic, Board.eBoardSize i_BoardSize, GameLogic.eGameMode i_GameMode)
        {
            // formOthello c'tor
            m_GameLogic = i_GameLogic;
            InitializeComponent();
            configureGameSettings(i_BoardSize, i_GameMode);
            createGameBoard();
            Initialize();
            adjustWindowSize(i_BoardSize);
            this.Icon = Ex05_Othello.UI.Resource1.icon;
        }

        private void adjustWindowSize(Board.eBoardSize i_BoardSize)
        {
            // this method is adjusting the window size according to the chosen game board size
            int windowLength, picBoxSize, windowMargin, picBoxMargin;

            flowLayoutPanelBoard.Top = 10;
            flowLayoutPanelBoard.Left = 10;
            picBoxMargin = 5;
            picBoxSize = flowLayoutPanelBoard.Controls[0].Width;
            windowMargin = flowLayoutPanelBoard.Top;
            windowLength = (windowMargin * 2) + (picBoxSize * (int)i_BoardSize) + (((int)i_BoardSize - 1) * picBoxMargin) + ((int)i_BoardSize / 2);
            flowLayoutPanelBoard.Size = new Size(windowLength, windowLength);
        }

        public void Initialize()
        {
            updateGameBoard();
        }

        private void showPlayersByGameBoard()
        {
            // this method is assigning images to the appropriate picture boxes.
            PictureBox cellAsPictureBox;

            foreach(Cell cell in m_GameLogic.GameBoard.Matrix)
            {
                cellAsPictureBox = convertCellToPictureBox(cell);
                if(cell.Sign == (char)Player.eColor.Red)
                {
                    // assign red player image
                    redPlayerPictureBoxStyle(cellAsPictureBox);
                }
                else if(cell.Sign == (char)Player.eColor.Yellow)
                {
                    // assign yellow player image
                    yellowPlayerPictureBoxStyle(cellAsPictureBox);
                }
            }
        }

        private void enableAllLegalPlayerPictureBoxs(Player.eColor i_Turn)
        {
            // this method is enabling all the picture boxes which represents the possible cells for the current turn
            List<Cell> currentPlayerOptionList;

            currentPlayerOptionList = i_Turn == Player.eColor.Yellow ? m_GameLogic.YellowPlayerOptions : m_GameLogic.RedPlayerOptions;
            foreach (Cell cell in currentPlayerOptionList)
            {
                enableRepresentingPictureBox(cell);
            }
        }

        private void enableRepresentingPictureBox(Cell i_Cell)
        {
            // this method recieve a cell and enabling the representing pictureBox.
            PictureBox pictureBox;

            pictureBox = convertCellToPictureBox(i_Cell);

            // 1. style the representing pictureBox.
            availablePictureBoxStyle(pictureBox);
        }

        private PictureBox convertCellToPictureBox(Cell i_Cell)
        {
            // this method recieves a cell and returns it representing picture box
            Control control;
            PictureBox pictureBox;
            string pictureBoxName;

            pictureBoxName = string.Format("pictureBox{0}", i_Cell.ToString());
            control = flowLayoutPanelBoard.Controls[pictureBoxName];
            pictureBox = control as PictureBox;

            return pictureBox;
        }

        private void disabledPictureBoxStyle(PictureBox i_PictureBoxToStyle)
        {
            // this method is styling a disabled pictureBox
            i_PictureBoxToStyle.Image = null;
            i_PictureBoxToStyle.Enabled = false;
            i_PictureBoxToStyle.BackColor = Color.Gray;
        }

        private void availablePictureBoxStyle(PictureBox i_PictureBoxToStyle)
        {
            // this method is styling an available pictureBox
            i_PictureBoxToStyle.Image = null;
            i_PictureBoxToStyle.Enabled = true;
            i_PictureBoxToStyle.BackColor = Color.Green;
        }

        private void yellowPlayerPictureBoxStyle(PictureBox i_PictureBoxToStyle)
        {
            // this method is styling a pictureBox occupied by a yellow player
            i_PictureBoxToStyle.Image = r_YellowImage;
            i_PictureBoxToStyle.Enabled = false;
            i_PictureBoxToStyle.BackColor = Color.Gray;
        }

        private void redPlayerPictureBoxStyle(PictureBox i_PictureBoxToStyle)
        {
            // this method is styling a pictureBox occupied by a red player
            i_PictureBoxToStyle.Image = r_RedImage;
            i_PictureBoxToStyle.Enabled = false;
            i_PictureBoxToStyle.BackColor = Color.Gray;
        }

        private void disableAllBoardPictureBoxes()
        {
            // this method is disabling all the picture boxes in the panel
            foreach (Control control in flowLayoutPanelBoard.Controls)
            {
               disabledPictureBoxStyle(control as PictureBox);
            }
        }

        private void configureGameSettings(Board.eBoardSize i_BoardSize, GameLogic.eGameMode i_GameMode)
        {
            // this method is configuring the game settings
            m_GameLogic.configureGameSettings(i_BoardSize, i_GameMode);
            m_GameLogic.Initialize();
        }

        private void createGameBoard()
        {
             // this method creates a game board(adding controls to the panel)   
            for (int i = 0; i < (int)m_GameLogic.GameBoard.Size * (int)m_GameLogic.GameBoard.Size; i++)
            {
                flowLayoutPanelBoard.Controls.Add(createGameBoardPictureBox(i, (int)m_GameLogic.GameBoard.Size));
            }
        }

        private PictureBox createGameBoardPictureBox(int i_PictureBoxNumber, int i_BoardSize)
        {
            // this method create a picture box for othello game.
            PictureBox pictureBox = new PictureBox();
            string pictureBoxIndex;

            pictureBoxIndex = extractPictureBoxIndex(i_PictureBoxNumber, i_BoardSize);
            pictureBox.Name = string.Format("pictureBox{0}", pictureBoxIndex);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = Color.Gray;
            pictureBox.Width = 40;
            pictureBox.Height = 40;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.MouseDown += pictureBoxCell_MouseDown;

            return pictureBox;
        }

        private string extractPictureBoxIndex(int i_PictureBoxNumber, int i_BoardSize)
        {
            // this method recieve a picture box number and return its index
            string pictureBoxIndex;
            char pictureBoxRow, pictureBoxColumn;

            pictureBoxRow = (char)((i_PictureBoxNumber / i_BoardSize) + '1');
            pictureBoxColumn = (char)((i_PictureBoxNumber % i_BoardSize) + 'A');
            pictureBoxIndex = string.Format("{0}{1}", pictureBoxColumn, pictureBoxRow);

            return pictureBoxIndex;
        }

        private void updateGameBoard()
        {
            // this method is updating the game board.

            // 1. disable all cells in game board.
            disableAllBoardPictureBoxes();

            // 2. show all player in game board. 
            showPlayersByGameBoard();

            // 3. show all legal option for this current turn. 
            enableAllLegalPlayerPictureBoxs(m_GameLogic.Turn);

            // 4. set the form title according to the player's turn.
            setFormTitle();
        }

        private void setFormTitle()
        {
            // this method sets the form title according to the current turn
            Player.eColor playerTurn;
            string formTitle;

            playerTurn = m_GameLogic.Turn;
            formTitle = string.Format("Othello - {0}'s turn", playerTurn.ToString());
            Text = formTitle;
        }

        private void pictureBoxCell_MouseDown(object i_Sender, MouseEventArgs i_E)
        {
            // this method represent a mouse down event when the user chose a legal cell to play
            int rowIndex, columnIndex;
            bool isGameOver;
            
            if (i_E.Button == MouseButtons.Left)
            {
                m_GameLogic.ExtractCellIndex((i_Sender as PictureBox).Name, out rowIndex, out columnIndex);
                m_GameLogic.PlayMove(new Cell(rowIndex, columnIndex));
                manageTurnChanging();
                updateGameBoard();
                isGameOver = manageGameOver();
                if (!isGameOver)
                {
                    managePcPlaying();
                }
            }
        }

        private void managePcPlaying()
        {
            // this method is managing the pc playing routine.
            bool isPcPlaying;

            isPcPlaying = m_GameLogic.Mode == GameLogic.eGameMode.HumanVsPC && m_GameLogic.Turn == Player.eColor.Red;
            if (isPcPlaying)
            {
                disableAllBoardPictureBoxes();
                PcPlay();
                updateGameBoard();
                manageGameOver();
            }
        }

        private void PcPlay()
        {
            // this method is wrapping the logic pc play
            m_GameLogic.PcPlay();

            while (m_GameLogic.Turn == Player.eColor.Red)
            {
                showTurnHasntBeenChangedMessageBox();
                m_GameLogic.PcPlay();
            }
        }

        private void manageTurnChanging()
        {
            // this method is managing the turn changing routine
            bool isTurnChanged;

            isTurnChanged = m_GameLogic.ManageTurnChanging();
            if (!isTurnChanged)
            {
                showTurnHasntBeenChangedMessageBox();
            }
        }

        private void showTurnHasntBeenChangedMessageBox()
        {
            // this method is showing a message box which informing the user that a player have no options and the turn hasn't been changed. 
            Player.eColor currentPlayerTurn, outOfOptionPlayer;
            string messageBoxText, messageBoxTitle;

            currentPlayerTurn = m_GameLogic.Turn;
            outOfOptionPlayer = currentPlayerTurn == Player.eColor.Red ? Player.eColor.Yellow : Player.eColor.Red;
            messageBoxText = string.Format("{0} player you have no options!{1}Its {2}'s turn again!", outOfOptionPlayer.ToString(), Environment.NewLine, currentPlayerTurn.ToString());
            messageBoxTitle = "Turn notice!";
            updateGameBoard();
            MessageBox.Show(messageBoxText, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool manageGameOver()
        {
            // this method is managing the game over routine.
            bool isGameOver;

            isGameOver = m_GameLogic.IsGameOver();
            if (isGameOver)
            {
                Close();
            }

            return isGameOver;
        }
    }
}
