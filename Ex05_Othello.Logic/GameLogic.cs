using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public class GameLogic
    {
        public enum eGameMode
        {
            HumanVsHuman = 1,
            HumanVsPC = 2
        }

        private Board m_GameBoard;
        private eGameMode m_GameMode;
        private Player.eColor m_PlayerTurn;
        private List<Cell> m_RedPlayerOptions = new List<Cell>();
        private List<Cell> m_YellowPlayerOptions = new List<Cell>();
        private List<Player> m_Players = new List<Player>();

        public GameLogic(Board i_GameBoard, Player.eColor i_PlayerTurn)
        {
            Board copiedBoard = i_GameBoard;
            m_GameBoard = copiedBoard;
            m_PlayerTurn = i_PlayerTurn;
        }

        public void getPlayersCurrentRoundScores(out int i_YellowPlayerRoundScore, out int i_RedPlayerRoundScore)
        {
            // this method return the players round scores by out parameters 
            i_YellowPlayerRoundScore = m_Players[playerIndex(Player.eColor.Yellow)].RoundScore;
            i_RedPlayerRoundScore = m_Players[playerIndex(Player.eColor.Red)].RoundScore;
        }

        public void getCurrentRoundWinner(out string o_WinnerColor, out bool o_IsGameEndedInTie)
        {
            // this method return the current round winner by out parameters.
            o_IsGameEndedInTie = isGameEndedInTie();
            if (o_IsGameEndedInTie)
            {
                o_WinnerColor = string.Empty;
            }
            else
            {
                o_WinnerColor = m_Players[playerIndex(Player.eColor.Yellow)].RoundScore > m_Players[playerIndex(Player.eColor.Red)].RoundScore ? "Yellow" : "Red";
            }
        }

        private bool isGameEndedInTie()
        {
            // this method return true if the game is ended in tie
            bool isGameEndedInDraw;

            isGameEndedInDraw = m_Players[playerIndex(Player.eColor.Yellow)].RoundScore == m_Players[playerIndex(Player.eColor.Red)].RoundScore;

            return isGameEndedInDraw;
        }

        public void getPlayersOverallScores(out int o_YellowPlayerOverallScore, out int o_RedPlayerOverallScore)
        {
            // this method return the overall players scores by out parameters.
            o_YellowPlayerOverallScore = m_Players[playerIndex(Player.eColor.Yellow)].OverallScore;
            o_RedPlayerOverallScore = m_Players[playerIndex(Player.eColor.Red)].OverallScore;
        }

        public GameLogic()
        {
            // GameLogic c'tor
        }

        public List<Player> Players
        {
            // a propertie for m_Players
            get
            {

                return m_Players;
            }

            set
            {
                m_Players = value;
            }
        }

        public void configureGameSettings(Board.eBoardSize i_BoardSize, eGameMode i_GameMode)
        {
            // this method is configuring the game settings
            m_GameBoard = new Board(i_BoardSize);
            m_GameMode = i_GameMode;
            setGameParticipants();
        }

        public void PlayMove(Cell i_ChosenCell)
        {
            // this method is getting index of a cell and and play according to the index(the cell is surely on option list)
            List<Cell> cellsToUpdate = new List<Cell>();

            // 1. add all cells that should be updated to cellToUpdate list!(how?)
            isPlayerMoveBlockingEnemy(i_ChosenCell, ref cellsToUpdate);

            // 2. update board
            m_GameBoard.UpdateBoard(cellsToUpdate, m_PlayerTurn);

            // 3. update player options
            updatePlayersOptions();

            // 4. update player scores
            updatePlayersScore();
        }

        public bool IsGameOver()
        {
            // this method checks if the game is over(if both of the players has no options to play).
            bool doesBothPlayersHasNoOptions;

            doesBothPlayersHasNoOptions = isPlayerOptionEmpty(Player.eColor.Red) && isPlayerOptionEmpty(Player.eColor.Yellow);

            return doesBothPlayersHasNoOptions;
        }

        private bool isPlayerOptionEmpty(Player.eColor i_PlayerColor)
        {
            // this method recieve a player color and return true if his options list is empty.
            bool isOptionListEmpty;

            if (i_PlayerColor == Player.eColor.Red)
            {
                isOptionListEmpty = m_RedPlayerOptions.Count == 0;
            }
            else
            {
                // if not red player - than its a yellow player.
                isOptionListEmpty = m_YellowPlayerOptions.Count == 0;
            }

            return isOptionListEmpty;
        }

        public void RestartGame()
        {
            // this method restarts a game.
            Initialize();
        }

        public void UpdateWinnerOverallScore()
        {
            // this method checks who is the winner of the game and updating his overall score
            int yellowPlayerScore, redPlayerScore;

            yellowPlayerScore = m_GameBoard.CountSignAppearances((char)Player.eColor.Yellow);
            redPlayerScore = m_GameBoard.CountSignAppearances((char)Player.eColor.Red);
            if (yellowPlayerScore > redPlayerScore)
            {
                m_Players[playerIndex(Player.eColor.Yellow)].OverallScore++;
            }
            else if (yellowPlayerScore < redPlayerScore)
            {
                m_Players[playerIndex(Player.eColor.Red)].OverallScore++;
            }
            else
            {
                m_Players[playerIndex(Player.eColor.Yellow)].OverallScore++;
                m_Players[playerIndex(Player.eColor.Red)].OverallScore++;
            }
        }

        private void updatePlayersScore()
        {
            // this method is updating the players scores.
            int updatedYellowPlayerScore, updatedRedPlayerScore;

            updatedYellowPlayerScore = m_GameBoard.CountSignAppearances((char)Player.eColor.Yellow);
            updatedRedPlayerScore = m_GameBoard.CountSignAppearances((char)Player.eColor.Red);
            m_Players[playerIndex(Player.eColor.Yellow)].RoundScore = updatedYellowPlayerScore;
            m_Players[playerIndex(Player.eColor.Red)].RoundScore = updatedRedPlayerScore;
        }

        public bool ManageTurnChanging()
        {
            // this method is managing the players turn changing and return true if the turn has been changed.
            bool isTurnChanged = true;

            if (m_PlayerTurn == Player.eColor.Red && m_YellowPlayerOptions.Count > 0)
            {
                m_PlayerTurn = Player.eColor.Yellow;
            }
            else if (m_PlayerTurn == Player.eColor.Yellow && m_RedPlayerOptions.Count > 0)
            {
                m_PlayerTurn = Player.eColor.Red;
            }
            else if(IsGameOver())
            {
                m_PlayerTurn = m_PlayerTurn == Player.eColor.Red ? Player.eColor.Yellow : Player.eColor.Red;
            }
            else if ((m_PlayerTurn == Player.eColor.Red && m_YellowPlayerOptions.Count == 0)
                 || (m_PlayerTurn == Player.eColor.Yellow && m_RedPlayerOptions.Count == 0))
            {
                isTurnChanged = false;
            }

            return isTurnChanged;
        }

        public void ExtractCellIndex(string i_CellName, out int o_RowIndex, out int o_ColumnIndex)
        {
            // this method get a pictureBox name and assign the row and column by out parameters.
            string cellName;

            cellName = i_CellName.Replace("pictureBox", string.Empty);
            o_ColumnIndex = cellName[0] - 'A';
            o_RowIndex = cellName[1] - '1';
        }

        public void PcPlay()
        {
            // this method is managing pc player play
            int rowIndex, columnIndex;

            (m_Players[playerIndex(Player.eColor.Red)] as PcPlayer).PickAMove(m_GameBoard, out rowIndex, out columnIndex);
            PlayMove(new Cell(rowIndex, columnIndex));
            ManageTurnChanging();
        }

        private int playerIndex(Player.eColor i_PlayerColor)
        {
            // this method recieves a player color and return the index by int
            int index;

            index = (char)(i_PlayerColor)-'0';

            return index;
        }

        private void setGameParticipants()
        {
            // this method is setting the player list according to the game mode.
            Player yellowPlayer = new HumanPlayer(Player.eColor.Yellow);
            Player redPlayer;

            if (m_GameMode == eGameMode.HumanVsHuman)
            {
                redPlayer = new HumanPlayer(Player.eColor.Red);
            }
            else
            {
                redPlayer = new PcPlayer(Player.eColor.Red);
            }

            m_Players.Add(yellowPlayer);
            m_Players.Add(redPlayer);
        }

        public void Initialize()
        {
            // this method is initializing the player options, scores and board.
            m_GameBoard.Initialize();
            initializePlayersOptions();
            initializePlayersCurrentRoundScores();
            m_PlayerTurn = Player.eColor.Yellow;
        }

        private void initializePlayersCurrentRoundScores()
        {
            // this method is initializing the players scores
            foreach(Player player in m_Players)
            {
                player.RoundScore = 2;
            }
        }

        private void initializePlayersOptions()
        {
            // this method is initializing the player options lists.
            if (m_RedPlayerOptions.Count != 0)
            {
                m_RedPlayerOptions.Clear();
            }

            if (m_YellowPlayerOptions.Count != 0)
            {
                m_YellowPlayerOptions.Clear();
            }

            initializeRedPlayerOptions();
            initializeYellowPlayerOptions();
        }

        private void initializeRedPlayerOptions()
        {
            // this method is initializing the red player options list
            Cell cellToBeAddedToOptions1;
            Cell cellToBeAddedToOptions2;
            Cell cellToBeAddedToOptions3;
            Cell cellToBeAddedToOptions4;
            int difference = ((int)m_GameBoard.Size - (int)Board.eBoardSize.size6x6) / 2;

            cellToBeAddedToOptions1 = new Cell(1 + difference, 2 + difference);
            cellToBeAddedToOptions2 = new Cell(2 + difference, 1 + difference);
            cellToBeAddedToOptions3 = new Cell(4 + difference, 3 + difference);
            cellToBeAddedToOptions4 = new Cell(3 + difference, 4 + difference);
            m_RedPlayerOptions.Add(cellToBeAddedToOptions1);
            m_RedPlayerOptions.Add(cellToBeAddedToOptions2);
            m_RedPlayerOptions.Add(cellToBeAddedToOptions3);
            m_RedPlayerOptions.Add(cellToBeAddedToOptions4);
        }

        private void initializeYellowPlayerOptions()
        {
            // this method is initializing the yellow player options list
            Cell cellToBeAddedToOptions1;
            Cell cellToBeAddedToOptions2;
            Cell cellToBeAddedToOptions3;
            Cell cellToBeAddedToOptions4;
            int difference = ((int)m_GameBoard.Size - (int)Board.eBoardSize.size6x6) / 2;

            cellToBeAddedToOptions1 = new Cell(1 + difference, 3 + difference);
            cellToBeAddedToOptions2 = new Cell(2 + difference, 4 + difference);
            cellToBeAddedToOptions3 = new Cell(3 + difference, 1 + difference);
            cellToBeAddedToOptions4 = new Cell(4 + difference, 2 + difference);
            m_YellowPlayerOptions.Add(cellToBeAddedToOptions1);
            m_YellowPlayerOptions.Add(cellToBeAddedToOptions2);
            m_YellowPlayerOptions.Add(cellToBeAddedToOptions3);
            m_YellowPlayerOptions.Add(cellToBeAddedToOptions4);
        }

        public void updatePlayersOptions()
        {
            // this method is updating the Lists of the players options
            List<Cell> cellList = new List<Cell>();
            Player.eColor lastPlayerTurn;
            bool isCellAnOption, shouldMethodAddCellsToUpdateList;

            m_YellowPlayerOptions.Clear();
            m_RedPlayerOptions.Clear();
            lastPlayerTurn = m_PlayerTurn;
            shouldMethodAddCellsToUpdateList = false;
            foreach (Cell cellIteator in m_GameBoard.Matrix)
            {
                if (cellIteator.Sign == Cell.k_Empty)
                {
                    m_PlayerTurn = Player.eColor.Yellow;
                    isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator, ref cellList, shouldMethodAddCellsToUpdateList);
                    if (isCellAnOption)
                    {
                        m_YellowPlayerOptions.Add(cellIteator);
                    }

                    m_PlayerTurn = Player.eColor.Red;
                    isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator, ref cellList, shouldMethodAddCellsToUpdateList);
                    if (isCellAnOption)
                    {
                        m_RedPlayerOptions.Add(cellIteator);
                    }
                }
            }

            // restore the turn of the last player
            m_PlayerTurn = lastPlayerTurn;
        }

        public bool isPlayerMoveBlockingEnemy(Cell i_CellToCheck, ref List<Cell> io_CellsToUpdate, bool i_AddCellsToList = true)
        {
            // this method recieves a player move and return true if the move is blocking the enemy.
            // its also updates the list of cells to update.
            bool isVerticalBlocking, isHorizontalBlocking, isDiagonalOneBlocking, isDiagonalTwoBlocking, isMoveBlockingEnemy;

            isVerticalBlocking = isVerticallyBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Up, (int)Direction.eDirection.NoDirection), i_AddCellsToList);
            isVerticalBlocking = isVerticallyBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Down, (int)Direction.eDirection.NoDirection), i_AddCellsToList) || isVerticalBlocking;
            isHorizontalBlocking = isHorizontallyBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.NoDirection, (int)Direction.eDirection.Left), i_AddCellsToList);
            isHorizontalBlocking = isHorizontallyBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.NoDirection, (int)Direction.eDirection.Right), i_AddCellsToList) || isHorizontalBlocking;
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Up, (int)Direction.eDirection.Right), i_AddCellsToList);
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Down, (int)Direction.eDirection.Left), i_AddCellsToList) || isDiagonalOneBlocking;
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Up, (int)Direction.eDirection.Left), i_AddCellsToList);
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_CellToCheck, ref io_CellsToUpdate, new Direction((int)Direction.eDirection.Down, (int)Direction.eDirection.Right), i_AddCellsToList) || isDiagonalTwoBlocking;
            isMoveBlockingEnemy = isVerticalBlocking || isHorizontalBlocking || isDiagonalOneBlocking || isDiagonalTwoBlocking;

            return isMoveBlockingEnemy;
        }

        private bool isHorizontallyBlocking(Cell i_CellToCheck, ref List<Cell> io_CellsToUpdate, Direction i_Direction, bool i_AddCellsToList)
        {
            // this method return true if a blocking sequence has been found in horizontal direction.
            Cell cellIterator = null;
            bool isBlockFound;

            isBlockFound = isBlockingLine(i_CellToCheck, i_Direction, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_Direction.Horizontal == (int)Direction.eDirection.Left)
                {
                    for (int column = i_CellToCheck.Column; column > cellIterator.Column; column--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i_CellToCheck.Row, column]);
                    }
                }
                else
                {
                    for (int column = i_CellToCheck.Column; column < cellIterator.Column; column++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i_CellToCheck.Row, column]);
                    }
                }
            }

            return isBlockFound;
        }

        private bool isDiagonallyTwoBlocking(Cell i_CellToCheck, ref List<Cell> io_CellsToUpdate, Direction i_Direction, bool i_AddCellsToList)
        {
            // this method return true if a blocking sequence has been found in diagonal direction.
            // **(Diagonal two is going down by Y).
            Cell cellIterator = null;
            bool isBlockFound;
            int row;

            isBlockFound = isBlockingLine(i_CellToCheck, i_Direction, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_Direction.Horizontal == (int)Direction.eDirection.Left && i_Direction.Vertical == (int)Direction.eDirection.Up)
                {
                    row = i_CellToCheck.Row;
                    for (int column = i_CellToCheck.Column; column > cellIterator.Column; column--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, column]);
                        row += (int)Direction.eDirection.Up;
                    }
                }
                else
                {
                    // elsewise we are going RIGHT and DOWN
                    row = i_CellToCheck.Row;
                    for (int i = i_CellToCheck.Column; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, i]);
                        row += (int)Direction.eDirection.Down;
                    }
                }
            }

            return isBlockFound;
        }

        private bool isDiagonallyOneBlocking(Cell i_CellToCheck, ref List<Cell> io_CellsToUpdate, Direction i_Direction, bool i_AddCellsToList)
        {
            // this method return true if a blocking sequence has been found in diagonal direction.
            // **(Diagonal one is going up by Y).
            Cell cellIterator = null;
            bool isBlockFound;
            int row;

            isBlockFound = isBlockingLine(i_CellToCheck, i_Direction, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_Direction.Horizontal == (int)Direction.eDirection.Left && i_Direction.Vertical == (int)Direction.eDirection.Down)
                {
                    row = i_CellToCheck.Row;
                    for (int column = i_CellToCheck.Column; column > cellIterator.Column; column--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, column]);
                        row += (int)Direction.eDirection.Down;
                    }
                }
                else
                {
                    // elsewise we are going UP and RIGHT
                    row = i_CellToCheck.Row;
                    for (int i = i_CellToCheck.Column; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, i]);
                        row += (int)Direction.eDirection.Up;
                    }
                }
            }

            return isBlockFound;
        }

        private bool isVerticallyBlocking(Cell i_CellToCheck, ref List<Cell> io_CellsToUpdate, Direction i_Direction, bool i_AddCellsToList)
        {
            // this method return true if a blocking sequence has been found in vertical direction.
            Cell cellIterator = null;
            bool isBlockFound;

            isBlockFound = isBlockingLine(i_CellToCheck, i_Direction, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_Direction.Vertical == (int)Direction.eDirection.Up)
                {
                    for (int row = i_CellToCheck.Row; row > cellIterator.Row; row--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, i_CellToCheck.Column]);
                    }
                }
                else
                {
                    for (int row = i_CellToCheck.Row; row < cellIterator.Row; row++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[row, i_CellToCheck.Column]);
                    }
                }
            }

            return isBlockFound;
        }

        private bool isBlockingLine(Cell i_CellToCheck, Direction i_Direction, out Cell o_CellIterator)
        {
            // this method return true if there is a sequence of legal blocking.
            // in addition, it return the last cell in the sequence as an out parameter.
            int currentRow, currentColumn;
            Cell cellIterator;
            bool isBlockFound, isInBoardLimits;

            currentRow = i_CellToCheck.Row + i_Direction.Vertical;
            currentColumn = i_CellToCheck.Column + i_Direction.Horizontal;
            isInBoardLimits = GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockFound = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockFound = isSeriesFound(ref cellIterator, i_Direction);
            }
            else
            {
                cellIterator = null;
            }

            o_CellIterator = cellIterator;

            return isBlockFound;
        }

        private bool isSeriesFound(ref Cell io_CellIterator, Direction i_Direction)
        {
            // this method get directions, cell by ref
            // this method return true if series of blocks has been found
            bool isCellEnemy, isInBoardLimits;
            bool isBlockingLine, isCharSimilarToMeFound;

            isCellEnemy = false;
            isBlockingLine = false;
            isInBoardLimits = m_GameBoard.IsCellInBoard(io_CellIterator);
            if (isInBoardLimits)
            {
                isCellEnemy = isCellAnEnemy(io_CellIterator, Turn);
            }

            if (isInBoardLimits && isCellEnemy)
            {
                // this condition check if the first cell is an enemy and in board, if it is countiue
                do
                {
                    io_CellIterator.Row += i_Direction.Vertical;
                    io_CellIterator.Column += i_Direction.Horizontal;
                    isInBoardLimits = m_GameBoard.IsCellInBoard(io_CellIterator);
                    if (isInBoardLimits)
                    {
                        io_CellIterator.Sign = m_GameBoard.Matrix[io_CellIterator.Row, io_CellIterator.Column].Sign;
                        isCellEnemy = isCellAnEnemy(io_CellIterator, Turn);
                    }
                }
                while (isInBoardLimits && isCellEnemy);

                // check why the while has been stopped
                if (isInBoardLimits)
                {
                    isCharSimilarToMeFound = io_CellIterator.Sign == (char)Turn;
                    if (isCharSimilarToMeFound)
                    {
                        isBlockingLine = true;
                    }
                }
            }

            return isBlockingLine;
        }

        private bool isCellAnEnemy(Cell i_CellIterator, Player.eColor i_CurrentPlayerTurn)
        {
            // this method return true if the sign of the input cell is different from i_CurrentPlayerTurn
            bool isCellEnemy;

            isCellEnemy = i_CellIterator.Sign != (char)i_CurrentPlayerTurn && i_CellIterator.Sign != Cell.k_Empty;

            return isCellEnemy;
        }

        public List<Cell> YellowPlayerOptions
        {
            // a propertie for m_YellowPlayerOptions
            get
            {

                return m_YellowPlayerOptions;
            }
        }

        public Player.eColor Turn
        {
            // a propertie for m_PlayerTurn
            get
            {

                return m_PlayerTurn;
            }

            set
            {
                m_PlayerTurn = value;
            }
        }

        public eGameMode Mode
        {
            // a propertie for m_GameMode
            get
            {

                return m_GameMode;
            }

            set
            {
                m_GameMode = value;
            }
        }

        public Board GameBoard
        {
            // a propertie for m_GameBoard.
            get
            {

                return m_GameBoard;
            }
        }

        public List<Cell> RedPlayerOptions
        {
            // a propertie for m_RedPlayerOptions
            get
            {

                return m_RedPlayerOptions;
            }
        }
    }
}
