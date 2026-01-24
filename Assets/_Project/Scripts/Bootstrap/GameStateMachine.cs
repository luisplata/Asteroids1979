using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public class GameStateMachine : MonoBehaviour
    {
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        public event Action<GameState> OnStateChanged;
        public event Action OnGameStarted;
        public event Action OnGameOver;
        public event Action OnReturnedToMenu;

        public void StartGame()
        {
            SetState(GameState.Playing);
        }

        public void GameOver()
        {
            SetState(GameState.GameOver);
        }

        public void ReturnToMenu()
        {
            SetState(GameState.MainMenu);
        }

        private void SetState(GameState newState)
        {
            if (CurrentState == newState)
                return;

            CurrentState = newState;

            switch (newState)
            {
                case GameState.Playing:
                    OnGameStarted?.Invoke();
                    break;

                case GameState.GameOver:
                    OnGameOver?.Invoke();
                    break;

                case GameState.MainMenu:
                    OnReturnedToMenu?.Invoke();
                    break;
            }

            OnStateChanged?.Invoke(newState);
        }
    }
}