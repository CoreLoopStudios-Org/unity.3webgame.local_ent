using UnityEngine;

namespace OSortudo.Data
{
    /// <summary>
    /// Scriptable Object to define prizes
    /// Allows designers to create prizes without coding
    /// Right-click in Project > Create > OSortudo > Prize Data
    /// </summary>
    [CreateAssetMenu(fileName = "NewPrize", menuName = "OSortudo/Prize Data")]
    public class PrizeData : ScriptableObject
    {
        [Header("Prize Information")]
        public string prizeName;
        [TextArea(3, 5)]
        public string description;
        public Sprite prizeImage;
        public int quantity = 1;

        [Header("Rarity")]
        public PrizeRarity rarity = PrizeRarity.Common;
        
        [Header("Value")]
        [Tooltip("Prize value in USD")]
        public float prizeValue = 1f;

        // Prize rarity enum
        public enum PrizeRarity
        {
            Common,    // Gray/White
            Rare,      // Blue
            Epic,      // Purple
            Legendary  // Gold
        }

        /// <summary>
        /// Get the color associated with this prize rarity
        /// </summary>
        public Color GetRarityColor()
        {
            switch (rarity)
            {
                case PrizeRarity.Common:
                    return new Color(0.8f, 0.8f, 0.8f); // Light Gray
                case PrizeRarity.Rare:
                    return new Color(0.2f, 0.5f, 1f); // Blue
                case PrizeRarity.Epic:
                    return new Color(0.6f, 0.2f, 1f); // Purple
                case PrizeRarity.Legendary:
                    return new Color(1f, 0.84f, 0f); // Gold
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// Get rarity weight for random distribution
        /// Higher = more rare
        /// </summary>
        public float GetRarityWeight()
        {
            switch (rarity)
            {
                case PrizeRarity.Common:
                    return 0.6f; // 60% chance
                case PrizeRarity.Rare:
                    return 0.25f; // 25% chance
                case PrizeRarity.Epic:
                    return 0.12f; // 12% chance
                case PrizeRarity.Legendary:
                    return 0.03f; // 3% chance
                default:
                    return 1f;
            }
        }
    }
}