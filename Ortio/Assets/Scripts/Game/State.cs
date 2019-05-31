using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class State
    {
        internal int currentPlayer;
        internal readonly int numberOfPlayers;

        /* A list of winning conditions that have been met */
        internal List<WinningCondition> winningConditions;

        int[] board;
        const int BoardSize = Position.LastIndex - Position.FirstIndex;

        public int[] RawBoardData { get { return board; } }

        public State(int playerCount = 4)
        {
            currentPlayer = 0;
            numberOfPlayers = playerCount;
            winningConditions = new List<WinningCondition>();
            board = new int[BoardSize];

            foreach(Position position in Position.all)
            {
                board[position.index] = position.nestOwner;
            }

            UpdateWinningConditions();
        }

        public State(State other)
        {
            this.currentPlayer = other.currentPlayer;
            this.numberOfPlayers = other.numberOfPlayers;
            this.winningConditions = other.winningConditions;
            this.board = (int[])other.board.Clone();
        }

        public void SetPlayerAtPosition(int player, Position position)
        {
            board[position.index] = player;
        }

        public int GetPlayerAtPosition(Position position)
        {
            return board[position.index];
        }

        public int CurrentPlayer()
        {
            return currentPlayer;
        }

        static int NextPlayerAfter(int currentPlayer, int numberOfPlayers)
        {
            // if there's 2 players we want to use the opposing sides
            if (numberOfPlayers == 2)
            {
                return (int)Mathf.Repeat(currentPlayer + 2, 4);
            }
            return (int)Mathf.Repeat(currentPlayer + 1, numberOfPlayers);
        }

        public void IncrementCurrentPlayer()
        {
            currentPlayer = NextPlayerAfter(currentPlayer, numberOfPlayers);
        }

        public void UpdateWinningConditions()
        {
            winningConditions = WinningCondition.FromGameState(board);
        }

        public bool HasWinner => winningConditions.Count > 0;
        public bool GameIsOver => HasWinner;
        public List<WinningCondition> WinningConditions => winningConditions;
    }
}