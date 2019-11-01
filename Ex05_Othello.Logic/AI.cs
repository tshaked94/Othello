using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public class AI
    {
        public static int Minimax(Board i_GameBoardState, int i_Depth, Player.eColor i_MaximizingPlayer, ref Cell io_Cell, ref List<KeyValuePair<int, List<Cell>>> io_ListOfKeyValue) 
        {
            // this method return a List of pairs < heuristic score , list of cells that lead to this score >
            // using Minimax algorithm, it return that list of pair I described by ref 
            GameLogic gameMangaerAI = new GameLogic(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> playerOptionList = new List<Cell>();
            List<Cell> playerMovesList = new List<Cell>();
            KeyValuePair<int, List<Cell>> scoreAndCellsListPair;
            Board copiedBoard;
            int eval, minEval, maxEval;

            if (i_Depth == 0 || isGameOver(i_GameBoardState, i_MaximizingPlayer))
            {
                return heuristic(i_GameBoardState, i_MaximizingPlayer);
            }
            else
            {
                gameMangaerAI.updatePlayersOptions();
                if (i_MaximizingPlayer == Player.eColor.Red) 
                {
                    // this is PC's turn - Choose max value
                    maxEval = int.MinValue;
                    foreach (Cell cellIteator in gameMangaerAI.RedPlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        eval = Minimax(copiedBoard, i_Depth - 1, Player.eColor.Yellow, ref io_Cell, ref io_ListOfKeyValue);
                        if (eval > maxEval)
                        {
                            playerMovesList.Add(io_Cell);
                            scoreAndCellsListPair = new KeyValuePair<int, List<Cell>>(eval, playerMovesList);
                            io_ListOfKeyValue.Add(scoreAndCellsListPair);
                            io_Cell.Row = cellIteator.Row;
                            io_Cell.Column = cellIteator.Column;                 
                            maxEval = eval;
                        }
                    }

                    return maxEval;
                }
                else 
                {
                // this is Human player - Choose min value
                    minEval = int.MaxValue;
                    foreach (Cell cellIteator in gameMangaerAI.YellowPlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        eval = Minimax(copiedBoard, i_Depth - 1, Player.eColor.Red, ref io_Cell, ref io_ListOfKeyValue);
                        if (eval < minEval)
                        {
                            playerMovesList.Add(io_Cell);
                            scoreAndCellsListPair = new KeyValuePair<int, List<Cell>>(eval, playerMovesList);
                            io_ListOfKeyValue.Add(scoreAndCellsListPair);
                            minEval = eval;
                        }
                    }

                    return minEval;
                }
            }
        }

        private static int differencePCScoreHumanScore(Board i_GameBoardState)
        {
            // this method calculate the difference between the PC player score and the human score and return it
            int yellowCharsInBoard, redCharsInBoard, difference;

            yellowCharsInBoard = i_GameBoardState.CountSignAppearances((char)Player.eColor.Yellow);
            redCharsInBoard = i_GameBoardState.CountSignAppearances((char)Player.eColor.Red);
            difference = redCharsInBoard - yellowCharsInBoard;

            return difference;
        }

        private static bool isGameOver(Board i_GameBoardState, Player.eColor i_MaximizingPlayer)
        {
            // this method passing all cell in the list and check if their is an option for maximizingPlayer
            GameLogic tempGameManager = new GameLogic(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> cellLists = new List<Cell>();
            bool addToCellsList, isCellAnOption, currentGameNotOver;

            addToCellsList = false;
            currentGameNotOver = false;
            foreach (Cell cellIteator in i_GameBoardState.Matrix)
            {
                isCellAnOption = tempGameManager.isPlayerMoveBlockingEnemy(cellIteator, ref cellLists, addToCellsList);
                if (isCellAnOption)
                {
                    return currentGameNotOver;
                }
            }

            currentGameNotOver = true;

            return currentGameNotOver;
        }

        public static void PCPlay(Board i_GameBoard, out int o_CurrentMoveRowIndex, out int o_CurrentMoveColumnIndex)
        {
            // this method choosing appropriate move using Minimax algorithm. 
            int minmaxOutput;
            Cell chosenCell = new Cell();
            List<KeyValuePair<int, List<Cell>>> listOfScoreAndMoveList = new List<KeyValuePair<int, List<Cell>>>();

            minmaxOutput = Minimax(i_GameBoard, 2, Player.eColor.Red, ref chosenCell, ref listOfScoreAndMoveList);

            // sorting the list of heuristic scores and moves that lead them,
            // sorting this list by their score.
            listOfScoreAndMoveList.Sort((x, y) => x.Key.CompareTo(y.Key));

            // choose the best score from the listOfScoreAndMoveList (the best located in the last index) and pick up the first Cell => will be PC chose
            // that lead to this best score.
            o_CurrentMoveRowIndex = listOfScoreAndMoveList[listOfScoreAndMoveList.Count - 1].Value[0].Row;
            o_CurrentMoveColumnIndex = listOfScoreAndMoveList[listOfScoreAndMoveList.Count - 1].Value[0].Column;
        }

        private static int getCornersHeuristic(Board i_Board, char i_Sign)
        {
            // this method return a score according to AI computing.
            int result, edgesOfBoard;

            edgesOfBoard = (int)i_Board.Size - 1;
            result = 0;
            Cell[] boardCorners =
                {
                i_Board.Matrix[0, 0],
                i_Board.Matrix[0, edgesOfBoard],
                i_Board.Matrix[edgesOfBoard, 0],
                i_Board.Matrix[edgesOfBoard, edgesOfBoard],
            };

            foreach (Cell corner in boardCorners)
            {
                if (corner.Sign == i_Sign)
                {
                    result += 20;
                }
            }

            return result;
        }

        private static int heuristic(Board i_Board, Player.eColor i_playerTurn)
        {
            // heuristic method for Minimax algorithm
            int heuristicResult;
            int differencePCHuman;
            char playerTurnSign;

            heuristicResult = 0;
            differencePCHuman = differencePCScoreHumanScore(i_Board);
            playerTurnSign = i_playerTurn == Player.eColor.Red ? (char)Player.eColor.Red : (char)Player.eColor.Yellow;
            heuristicResult += getCornersHeuristic(i_Board, playerTurnSign);
            return heuristicResult + differencePCHuman;
        }
    }
}
