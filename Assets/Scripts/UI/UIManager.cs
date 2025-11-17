using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace OSortudo.UI
{
    /// <summary>
    /// Manages all UI elements and updates
    /// Displays timer, coins, stats, etc.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Main UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject resultsPanel;

        [Header("Gameplay UI")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private Button startRoundButton;
        [SerializeField] private TextMeshProUGUI roundStatusText;

        [Header("Stats Panel")]
        [SerializeField] private Transform lastWinningNumbersContainer;
        [SerializeField] private GameObject numberDisplayPrefab;
        [SerializeField] private TextMeshProUGUI mostFrequentColorText;

        [Header("Results UI")]
        [SerializeField] private TextMeshProUGUI resultsText;
        [SerializeField] private Button playAgainButton;

        private int playerCoins = 100; // Starting coins

        private void Awake()
        {
            // Subscribe to game events
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameStateChanged += HandleStateChange;
                Core.GameManager.Instance.OnRoundTimeChanged += UpdateTimer;
                Core.GameManager.Instance.OnRoundStart += HandleRoundStart;
                Core.GameManager.Instance.OnRoundEnd += HandleRoundEnd;
            }

            // Setup button listeners
            if (startRoundButton != null)
                startRoundButton.onClick.AddListener(OnStartRoundClicked);

            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        }

        private void Start()
        {
            UpdateCoinsDisplay();

            // Debug check
            Debug.Log($"[UIManager] Start - Checking panels:");
            Debug.Log($"  MainMenu: {(mainMenuPanel != null ? "OK" : "NULL")}");
            Debug.Log($"  Gameplay: {(gameplayPanel != null ? "OK" : "NULL")}");
            Debug.Log($"  Results: {(resultsPanel != null ? "OK" : "NULL")}");
            Debug.Log($"  StartButton: {(startRoundButton != null ? "OK" : "NULL")}");

            ShowPanel(mainMenuPanel);
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameStateChanged -= HandleStateChange;
                Core.GameManager.Instance.OnRoundTimeChanged -= UpdateTimer;
                Core.GameManager.Instance.OnRoundStart -= HandleRoundStart;
                Core.GameManager.Instance.OnRoundEnd -= HandleRoundEnd;
            }
        }

        /// <summary>
        /// Handle game state changes
        /// </summary>
        private void HandleStateChange(Core.GameManager.GameState newState)
        {
            Debug.Log($"[UIManager] HandleStateChange called: {newState}");

            switch (newState)
            {
                case Core.GameManager.GameState.MainMenu:
                    ShowPanel(mainMenuPanel);
                    break;

                case Core.GameManager.GameState.Playing:
                    ShowPanel(gameplayPanel);
                    if (roundStatusText != null)
                        roundStatusText.text = "Round in Progress...";
                    break;

                case Core.GameManager.GameState.RoundEnd:
                case Core.GameManager.GameState.Results:
                    ShowPanel(resultsPanel);
                    break;
            }
        }

        /// <summary>
        /// Show specific panel and hide others
        /// </summary>
        private void ShowPanel(GameObject panelToShow)
        {
            Debug.Log($"[UIManager] ShowPanel called with: {(panelToShow != null ? panelToShow.name : "NULL")}");

            // Force disable all panels first
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
                Debug.Log("  MainMenuPanel force disabled");
            }

            if (gameplayPanel != null)
            {
                gameplayPanel.SetActive(false);
                Debug.Log("  GameplayPanel force disabled");
            }

            if (resultsPanel != null)
            {
                resultsPanel.SetActive(false);
                Debug.Log("  ResultsPanel force disabled");
            }

            // Then enable the target panel
            if (panelToShow != null)
            {
                panelToShow.SetActive(true);
                Debug.Log($"  {panelToShow.name} enabled");
            }
        }

        /// <summary>
        /// Update the timer display
        /// </summary>
        private void UpdateTimer(float timeRemaining)
        {
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";

                // Change color when time is running out
                if (timeRemaining <= 30f)
                {
                    timerText.color = Color.red;
                }
                else
                {
                    timerText.color = Color.white;
                }
            }
        }

        /// <summary>
        /// Update coins display
        /// </summary>
        private void UpdateCoinsDisplay()
        {
            if (coinsText != null)
            {
                coinsText.text = $"Coins: {playerCoins}";
            }
        }

        /// <summary>
        /// Handle round start
        /// </summary>
        private void HandleRoundStart()
        {
            if (startRoundButton != null)
                startRoundButton.gameObject.SetActive(false);

            if (roundStatusText != null)
                roundStatusText.text = "Select your cards!";
        }

        /// <summary>
        /// Handle round end
        /// </summary>
        private void HandleRoundEnd()
        {
            if (roundStatusText != null)
                roundStatusText.text = "Round Ended!";

            DisplayResults();
        }

        /// <summary>
        /// Display round results
        /// </summary>
        private void DisplayResults()
        {
            if (resultsText != null)
            {
                // TODO: Get actual results from GridManager
                resultsText.text = "Round Complete!\n\nCheck the revealed cards to see if you won!";
            }
        }

        /// <summary>
        /// Add coins to player balance
        /// </summary>
        public void AddCoins(int amount)
        {
            playerCoins += amount;
            UpdateCoinsDisplay();
            Debug.Log($"[UIManager] Added {amount} coins. Total: {playerCoins}");
        }

        /// <summary>
        /// Subtract coins from player balance
        /// </summary>
        public bool SpendCoins(int amount)
        {
            if (playerCoins >= amount)
            {
                playerCoins -= amount;
                UpdateCoinsDisplay();
                Debug.Log($"[UIManager] Spent {amount} coins. Remaining: {playerCoins}");
                return true;
            }

            Debug.LogWarning($"[UIManager] Not enough coins! Need {amount}, have {playerCoins}");
            return false;
        }

        /// <summary>
        /// Display last winning numbers
        /// </summary>
        public void DisplayLastWinningNumbers(int[] numbers)
        {
            if (lastWinningNumbersContainer == null || numberDisplayPrefab == null)
                return;

            // Clear existing displays
            foreach (Transform child in lastWinningNumbersContainer)
            {
                Destroy(child.gameObject);
            }

            // Create new number displays
            foreach (int num in numbers)
            {
                GameObject numObj = Instantiate(numberDisplayPrefab, lastWinningNumbersContainer);
                TextMeshProUGUI numText = numObj.GetComponentInChildren<TextMeshProUGUI>();
                if (numText != null)
                {
                    numText.text = num.ToString();
                }
            }
        }

        // Button callbacks
        private void OnStartRoundClicked()
        {
            Core.GameManager.Instance?.StartRound();
        }

        private void OnPlayAgainClicked()
        {
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.MainMenu);
        }

        // Public getters
        public int GetPlayerCoins() => playerCoins;
    }
}