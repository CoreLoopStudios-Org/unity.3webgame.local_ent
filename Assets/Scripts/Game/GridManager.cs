using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace OSortudo.Game
{
    /// <summary>
    /// Manages the grid of cards with bomb system
    /// Handles card creation, prize distribution, bombs, and player selections
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject cardPrefab;

        [Tooltip("How many columns the grid has (10 for 100 cells)")]
        [SerializeField] private int columns = 10;

        [Tooltip("How many rows the grid has (10 for 100 cells)")]
        [SerializeField] private int rows = 10;

        [Header("Prize Pool")]
        [SerializeField] private List<Data.PrizeData> availablePrizes = new List<Data.PrizeData>();

        [Header("Bomb Settings")]
        [SerializeField] private int bombCount = 5;
        [SerializeField] private Sprite bombSprite;
        [SerializeField] private Color bombColor = Color.red;

        private List<CardController> allCards = new List<CardController>();
        private List<CardController> prizeCards = new List<CardController>();
        private List<CardController> bombCards = new List<CardController>();
        private GridLayoutGroup gridLayout;
        private bool isGameOver = false;

        private void Awake()
        {
            gridLayout = gridParent.GetComponent<GridLayoutGroup>();

            // Find GameManager if not found
            if (Core.GameManager.Instance == null)
            {
                Debug.LogWarning("[GridManager] GameManager not found in Awake!");
            }
        }

        private void OnEnable()
        {
            // Subscribe to game events
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnRoundStart += HandleRoundStart;
                Core.GameManager.Instance.OnRoundEnd += HandleRoundEnd;
                Debug.Log("[GridManager] Subscribed to GameManager events");
            }
            else
            {
                Debug.LogError("[GridManager] Cannot subscribe - GameManager.Instance is null!");
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnRoundStart -= HandleRoundStart;
                Core.GameManager.Instance.OnRoundEnd -= HandleRoundEnd;
            }
        }

        private void Start()
        {
            SetupGrid();
            CenterGridParent();

            // Auto-distribute on start if in playing state
            if (Core.GameManager.Instance != null)
            {
                if (Core.GameManager.Instance.CurrentState == Core.GameManager.GameState.Playing)
                {
                    Debug.Log("[GridManager] Game already playing, distributing prizes/bombs");
                    DistributePrizesAndBombs();
                }
            }
        }

        // For testing in Inspector or through code
        [ContextMenu("Test Distribution")]
        public void TestDistribution()
        {
            Debug.Log("[GridManager] Manual test distribution triggered");
            DistributePrizesAndBombs();
        }

        private void OnDestroy()
        {
            // Cleanup handled in OnDisable
        }

        /// <summary>
        /// Centers the grid using the RectTransform
        /// </summary>
        private void CenterGridParent()
        {
            RectTransform rt = gridParent.GetComponent<RectTransform>();
            if (rt == null) return;

            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Create the grid of cards - EXACTLY columns x rows
        /// </summary>
        private void SetupGrid()
        {
            if (cardPrefab == null || gridParent == null)
            {
                Debug.LogError("[GridManager] Card prefab or grid parent not assigned!");
                return;
            }

            ClearGrid();

            // Setup GridLayoutGroup
            if (gridLayout != null)
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = columns;
                gridLayout.childAlignment = TextAnchor.MiddleCenter;

                // Adjust these values based on your canvas size
                gridLayout.spacing = new Vector2(10f, 10f);
                gridLayout.cellSize = new Vector2(100f, 100f);
            }

            // Create EXACTLY columns * rows cards
            int totalCards = columns * rows;
            Debug.Log($"[GridManager] Creating {totalCards} cards ({columns} x {rows})");

            for (int i = 1; i <= totalCards; i++)
            {
                GameObject cardObj = Instantiate(cardPrefab, gridParent);
                CardController card = cardObj.GetComponent<CardController>();

                if (card != null)
                {
                    card.Initialize(i);
                    card.OnCardClicked += HandleCardClicked;
                    allCards.Add(card);
                }
            }

            Debug.Log($"[GridManager] Successfully created {allCards.Count} cards");
            DistributePrizesAndBombs();
        }

        /// <summary>
        /// Clear all cards from the grid
        /// </summary>
        private void ClearGrid()
        {
            foreach (var card in allCards)
            {
                if (card != null)
                    card.OnCardClicked -= HandleCardClicked;
            }

            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }

            allCards.Clear();
            prizeCards.Clear();
            bombCards.Clear();
        }

        /// <summary>
        /// Regenerate the grid
        /// </summary>
        public void RegenerateGrid()
        {
            Debug.Log("[GridManager] Regenerating grid...");
            isGameOver = false; // Reset game over flag
            SetupGrid();
            CenterGridParent();

            if (Core.GameManager.Instance != null)
            {
                // Always distribute when regenerating
                DistributePrizesAndBombs();
            }
        }

        /// <summary>
        /// Restart the game (call this from Play Again button)
        /// </summary>
        public void RestartGame()
        {
            Debug.Log("[GridManager] Restarting game...");

            // Start a new round
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.StartRound();
            }
        }

        private void HandleRoundStart()
        {
            Debug.Log("[GridManager] HandleRoundStart called");
            isGameOver = false;
            ResetAllCards();
            DistributePrizesAndBombs();
        }

        private void HandleRoundEnd()
        {
            RevealAllCards();

            // If player hit a bomb, show different message
            if (isGameOver)
            {
                Debug.Log("[GridManager] Round ended due to bomb hit!");
            }
            else
            {
                Debug.Log("[GridManager] Round ended normally!");
            }
        }

        /// <summary>
        /// Distribute prizes and bombs randomly across the grid
        /// </summary>
        private void DistributePrizesAndBombs()
        {
            prizeCards.Clear();
            bombCards.Clear();

            Debug.Log($"[GridManager] DistributePrizesAndBombs called. Total cards: {allCards.Count}");
            Debug.Log($"[GridManager] Available prizes: {availablePrizes.Count}");

            if (allCards.Count == 0)
            {
                Debug.LogError("[GridManager] No cards in grid!");
                return;
            }

            if (availablePrizes.Count == 0)
            {
                Debug.LogWarning("[GridManager] No prizes available in the list!");
            }

            // Shuffle all cards
            List<CardController> shuffledCards = allCards.OrderBy(x => Random.value).ToList();

            // Place bombs first
            int bombsToPlace = Mathf.Min(bombCount, allCards.Count);
            Debug.Log($"[GridManager] Placing {bombsToPlace} bombs...");

            for (int i = 0; i < bombsToPlace; i++)
            {
                shuffledCards[i].SetBomb(bombSprite, bombColor);
                bombCards.Add(shuffledCards[i]);
                Debug.Log($"[GridManager] Bomb placed on card #{shuffledCards[i].GetCardNumber()}");
            }

            // Place prizes in remaining cards
            int prizeCount = Core.GameManager.Instance.GetPrizeCount();
            int availableSlots = allCards.Count - bombsToPlace;
            prizeCount = Mathf.Min(prizeCount, availableSlots);

            Debug.Log($"[GridManager] Placing {prizeCount} prizes...");

            for (int i = bombsToPlace; i < bombsToPlace + prizeCount; i++)
            {
                Data.PrizeData selectedPrize = SelectWeightedPrize();

                if (selectedPrize != null)
                {
                    shuffledCards[i].SetPrize(selectedPrize);
                    prizeCards.Add(shuffledCards[i]);
                    Debug.Log($"[GridManager] Prize '{selectedPrize.prizeName}' placed on card #{shuffledCards[i].GetCardNumber()}");
                }
            }

            Debug.Log($"[GridManager] Distribution complete: {bombCards.Count} bombs, {prizeCards.Count} prizes across {allCards.Count} cards");
        }

        private Data.PrizeData SelectWeightedPrize()
        {
            float totalWeight = availablePrizes.Sum(p => p.GetRarityWeight());
            float randomValue = Random.value * totalWeight;
            float cumulativeWeight = 0f;

            foreach (var prize in availablePrizes)
            {
                cumulativeWeight += prize.GetRarityWeight();
                if (randomValue <= cumulativeWeight)
                {
                    return prize;
                }
            }

            return availablePrizes[0]; // fallback
        }

        /// <summary>
        /// Handle card clicked - check for bombs
        /// </summary>
        private void HandleCardClicked(CardController card)
        {
            // If game is over (bomb hit), don't allow more selections
            if (isGameOver)
            {
                Debug.Log("[GridManager] Game over! No more selections allowed.");
                return;
            }

            bool success = Core.GameManager.Instance.PlayerSelectCard(card.GetCardNumber());

            if (success)
            {
                card.SelectCard();

                // Check if player hit a bomb
                if (card.HasBomb())
                {
                    Debug.Log("[GridManager] BOMB HIT! Game Over!");
                    isGameOver = true;
                    HandleBombHit(card);
                }
            }
        }

        /// <summary>
        /// Handle when player hits a bomb
        /// </summary>
        private void HandleBombHit(CardController bombCard)
        {
            // Reveal the bomb immediately
            bombCard.RevealCard();

            // Disable all other cards
            DisableAllCards();

            Debug.Log("💣 BOOM! You hit a bomb!");

            // Wait 1 second before ending round to show bomb sprite
            StartCoroutine(EndRoundAfterDelay(1f));
        }

        /// <summary>
        /// Coroutine to delay round end
        /// </summary>
        private System.Collections.IEnumerator EndRoundAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // End the round after delay
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.EndRound();
            }
        }

        /// <summary>
        /// Disable all cards (prevent further clicks)
        /// </summary>
        private void DisableAllCards()
        {
            foreach (var card in allCards)
            {
                if (!card.IsRevealed())
                {
                    card.DisableCard();
                }
            }
        }

        private void RevealAllCards()
        {
            foreach (var card in allCards)
            {
                card.RevealCard();
            }
        }

        private void ResetAllCards()
        {
            foreach (var card in allCards)
            {
                card.ResetCard();
            }
        }

        public List<int> GetWinningNumbers()
        {
            return prizeCards.Select(c => c.GetCardNumber()).ToList();
        }

        public List<int> GetBombNumbers()
        {
            return bombCards.Select(c => c.GetCardNumber()).ToList();
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }

        public Dictionary<Data.PrizeData.PrizeRarity, int> GetRarityDistribution()
        {
            var distribution = new Dictionary<Data.PrizeData.PrizeRarity, int>();

            foreach (var card in prizeCards)
            {
                var prize = card.GetPrize();
                if (prize != null)
                {
                    if (!distribution.ContainsKey(prize.rarity))
                        distribution[prize.rarity] = 0;

                    distribution[prize.rarity]++;
                }
            }

            return distribution;
        }
    }
}