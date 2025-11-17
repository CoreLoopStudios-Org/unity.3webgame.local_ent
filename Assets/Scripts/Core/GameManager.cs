using UnityEngine;
using System;

namespace OSortudo.Core
{
    /// <summary>
    /// Central game controller - manages game state and transitions
    /// Singleton pattern for easy access across all scripts
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField] private int gridSize = 100; // 10x10 grid
        [SerializeField] private float roundDuration = 120f; // 2 minutes
        [SerializeField] private int prizeCount = 10; // Number of prizes per round

        [Header("Economy Settings")]
        [SerializeField] private int costPerPlay = 1; // Cost in Local Coins or $1
        [SerializeField] private int coinsPerDollar = 1; // Users get 1 coin per $1 spent

        // Game State
        public enum GameState { MainMenu, WaitingForPlayers, Playing, RoundEnd, Results }
        public GameState CurrentState { get; private set; }

        // Events for other systems to subscribe to
        public event Action<GameState> OnGameStateChanged;
        public event Action<float> OnRoundTimeChanged;
        public event Action OnRoundStart;
        public event Action OnRoundEnd;

        private float currentRoundTime;
        private bool isRoundActive;

        private void Awake()
        {
            // Singleton setup
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ChangeState(GameState.MainMenu);
        }

        private void Update()
        {
            if (isRoundActive)
            {
                UpdateRoundTimer();
            }
        }

        /// <summary>
        /// Change game state and notify all subscribers
        /// </summary>
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            Debug.Log($"[GameManager] State changed to: {newState}");
        }

        /// <summary>
        /// Start a new game round
        /// </summary>
        public void StartRound()
        {
            currentRoundTime = roundDuration;
            isRoundActive = true;
            ChangeState(GameState.Playing);
            OnRoundStart?.Invoke();
            Debug.Log("[GameManager] Round started!");
        }

        /// <summary>
        /// Update the round timer
        /// </summary>
        private void UpdateRoundTimer()
        {
            currentRoundTime -= Time.deltaTime;
            OnRoundTimeChanged?.Invoke(currentRoundTime);

            if (currentRoundTime <= 0)
            {
                EndRound();
            }
        }

        /// <summary>
        /// End the current round
        /// </summary>
        public void EndRound()
        {
            isRoundActive = false;
            ChangeState(GameState.RoundEnd);
            OnRoundEnd?.Invoke();
            Debug.Log("[GameManager] Round ended!");
        }

        /// <summary>
        /// Player selects a card/number
        /// </summary>
        public bool PlayerSelectCard(int cardNumber)
        {
            if (CurrentState != GameState.Playing)
            {
                Debug.Log("[GameManager] Cannot select card - round not active!");
                return false;
            }

            // TODO: Check if player has enough coins/money
            // TODO: Process payment
            // TODO: Award automatic coin reward (1 coin per $1 spent)

            Debug.Log($"[GameManager] Player selected card: {cardNumber}");
            return true;
        }

        // Getters for game settings
        public int GetGridSize() => gridSize;
        public float GetRoundDuration() => roundDuration;
        public int GetPrizeCount() => prizeCount;
        public int GetCostPerPlay() => costPerPlay;
        public float GetCurrentRoundTime() => currentRoundTime;
    }
}