using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public class Logic
    {
        public interface Listener
        {
            void OnPerformMove(Move move);
            void OnCurrentPlayerChanged();
            void OnGameStateChanged();
        }

        State state;

        public State CurrentState => state;
        public int CurrentPlayer => state.currentPlayer;

        public bool HasWinner => state.HasWinner;
        public bool GameIsOver => state.GameIsOver;

        public List<Listener> listeners = new List<Listener>();

        public Logic(State state)
        {
            this.state = state; 
        }

        public void Reset(int playerCount)
        {
            this.state = new State(playerCount);
            listeners.ForEach((obj) => obj.OnGameStateChanged());
        }

        public bool PlayTurn(Position fromPosition, Position toPosition)
        {
            return Move(state.currentPlayer, fromPosition, toPosition);
        }

        private bool Move(int player, Position fromPosition, Position toPosition)
        {
            State oldState = new State(state);

            if (!ValidateMove(player, fromPosition, toPosition)) { 
                return false;
            }

            state.SetPlayerAtPosition(-1, fromPosition);
            state.SetPlayerAtPosition(player, toPosition);

            var move = new Move(player, fromPosition, toPosition);
            listeners.ForEach((obj) => obj.OnPerformMove(move));

            state.UpdateWinningConditions();

            if (!state.GameIsOver)
            {
                state.IncrementCurrentPlayer();
                listeners.ForEach((obj) => obj.OnCurrentPlayerChanged());
            }

            if (oldState.GameIsOver != state.GameIsOver)
            {
                listeners.ForEach((obj) => obj.OnGameStateChanged());
            }

            return true;
        }

        public bool ValidateMove(int player, Position fromPosition, Position toPosition)
        {
            if (GameIsOver) 
            {
                Debug.LogError("The game is over");
                return false;
            }

            var playerAtFromPosition = state.GetPlayerAtPosition(fromPosition);
            var playerAtToPosition = state.GetPlayerAtPosition(toPosition);

            // destination must be empty
            if (playerAtToPosition != -1)
            {
                Debug.LogErrorFormat("Destination ({0}) is not empty. ({1})", toPosition, playerAtToPosition);
                return false;
            }

            // source must be occupied by player
            if (playerAtFromPosition != player)
            {
                Debug.LogErrorFormat(
                    "Source ({0}, {1}) is not occupied by player ({2}).",
                    fromPosition, playerAtFromPosition, player
                    );
                return false;
            }

            return true;
        }

    }
}