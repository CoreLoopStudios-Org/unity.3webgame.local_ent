using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace OSortudo.Game
{
    /// <summary>
    /// Controls individual card behavior in the grid
    /// Handles clicking, revealing, bombs, and visual states
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CardController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image cardBackground;
        [SerializeField] private Image prizeImage;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private GameObject revealedPanel;
        [SerializeField] private GameObject lockedPanel;

        [Header("Visual Settings")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color revealedColor = Color.green;

        // Card state
        private int cardNumber;
        private bool hasPrize;
        private bool hasBomb;
        private Data.PrizeData prizeData;
        private Sprite bombSprite;
        private Color bombColor;
        private bool isRevealed;
        private bool isSelected;
        [SerializeField] private Button button;

        // Events
        public event Action<CardController> OnCardClicked;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleCardClick);
        }

        /// <summary>
        /// Initialize the card with its number
        /// </summary>
        public void Initialize(int number)
        {
            cardNumber = number;
            numberText.text = number.ToString();

            if (prizeImage != null)
                prizeImage.gameObject.SetActive(false);

            if (revealedPanel != null)
                revealedPanel.SetActive(false);

            if (lockedPanel != null)
                lockedPanel.SetActive(false);

            // Reset button interactability
            button.interactable = true;
        }

        /// <summary>
        /// Assign a prize to this card
        /// </summary>
        public void SetPrize(Data.PrizeData prize)
        {
            hasPrize = true;
            hasBomb = false;
            prizeData = prize;
        }

        /// <summary>
        /// Assign a bomb to this card
        /// </summary>
        public void SetBomb(Sprite sprite, Color color)
        {
            hasBomb = true;
            hasPrize = false;
            bombSprite = sprite;
            bombColor = color;
        }

        /// <summary>
        /// Handle card click
        /// </summary>
        private void HandleCardClick()
        {
            if (isRevealed || Core.GameManager.Instance.CurrentState != Core.GameManager.GameState.Playing)
            {
                return;
            }

            // Notify listeners (GridManager will handle the logic)
            OnCardClicked?.Invoke(this);
        }

        /// <summary>
        /// Select this card (player chose it)
        /// </summary>
        public void SelectCard()
        {
            isSelected = true;
            cardBackground.color = selectedColor;

            if (lockedPanel != null)
                lockedPanel.SetActive(true);

            // Keep button disabled after selection
            button.interactable = false;
        }

        /// <summary>
        /// Reveal the card (show if it has a prize or bomb)
        /// </summary>
        public void RevealCard()
        {
            isRevealed = true;

            if (revealedPanel != null)
                revealedPanel.SetActive(true);

            // Show bomb
            if (hasBomb)
            {
                cardBackground.color = bombColor;

                if (prizeImage != null)
                {
                    // Use bomb sprite if available, otherwise use emoji in text
                    if (bombSprite != null)
                    {
                        prizeImage.sprite = bombSprite;
                        prizeImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        // Show emoji in text if no sprite
                        if (numberText != null)
                        {
                            numberText.text = "💣";
                            numberText.fontSize = 48;
                        }
                    }
                }

                Debug.Log($"[Card {cardNumber}] Bomb revealed!");
            }
            // Show prize
            else if (hasPrize && prizeData != null)
            {
                cardBackground.color = prizeData.GetRarityColor();

                if (prizeImage != null && prizeData.prizeImage != null)
                {
                    prizeImage.sprite = prizeData.prizeImage;
                    prizeImage.gameObject.SetActive(true);
                }

                Debug.Log($"[Card {cardNumber}] Prize '{prizeData.prizeName}' revealed!");
            }
            // No prize, no bomb
            else
            {
                cardBackground.color = revealedColor;
            }

            // Disable button
            button.interactable = false;
        }

        /// <summary>
        /// Disable the card (prevent clicking)
        /// </summary>
        public void DisableCard()
        {
            button.interactable = false;

            // Optional: make it look disabled
            Color disabledColor = cardBackground.color;
            disabledColor.a = 0.5f;
            cardBackground.color = disabledColor;
        }

        /// <summary>
        /// Reset card for new round
        /// </summary>
        public void ResetCard()
        {
            hasPrize = false;
            hasBomb = false;
            prizeData = null;
            bombSprite = null;
            isRevealed = false;
            isSelected = false;

            if (prizeImage != null)
                prizeImage.gameObject.SetActive(false);

            if (revealedPanel != null)
                revealedPanel.SetActive(false);

            if (lockedPanel != null)
                lockedPanel.SetActive(false);

            cardBackground.color = defaultColor;

            // Reset number text
            if (numberText != null)
            {
                numberText.text = cardNumber.ToString();
                numberText.fontSize = 24; // Reset to default size
            }

            // Re-enable button
            button.interactable = true;
        }

        // Getters
        public int GetCardNumber() => cardNumber;
        public bool HasPrize() => hasPrize;
        public bool HasBomb() => hasBomb;
        public Data.PrizeData GetPrize() => prizeData;
        public bool IsRevealed() => isRevealed;
        public bool IsSelected() => isSelected;
    }
}