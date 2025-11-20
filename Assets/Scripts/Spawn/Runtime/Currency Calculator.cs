using TMPro;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

public class CurrencyCalculator : MonoBehaviour
{
    public IntegerEvent currencyMultiplierIntegerEvent;
    public IntegerEvent integerEvent;
    
    
    public TMP_Text mineCountText;
    public TMP_Text currencyMultiplierText;
    public TMP_Text earnedCurrencyText;
    public TMP_Text betText;
    
    [SerializeField] FloatVariable multiplier;
    [SerializeField] FloatVariable betValue;
    [SerializeField] IntegerVariable mineCountValue;
    private float currencyToEarnNext;
    private int safeTilesRevealed = 0;
    
    private void OnEnable()
    {
        
        earnedCurrencyText.text = "0.00";
        safeTilesRevealed = 0;
        UpdateInitialMultiplier();
        
        currencyMultiplierIntegerEvent.AddListener(UpdateCurrencyViaMines);
        integerEvent.AddListener(CalculateCurrency);
    }


    private void OnDisable()
    {
        currencyMultiplierIntegerEvent.RemoveListener(UpdateCurrencyViaMines);
        integerEvent.RemoveListener(CalculateCurrency);
    }

    void UpdateCurrencyViaMines(int mineCount)
    {
        if (mineCount >= 2 && mineCount <= 24)
        {
            mineCountValue.Value = mineCount;
            safeTilesRevealed = 0;
            //
            betValue.Value = float.Parse(betText.text);
            //mineCountValue = float.Parse(mineCountText.text);
            
            multiplier.Value = CalculateMultiplier(safeTilesRevealed, mineCountValue.Value);
            currencyMultiplierText.text = (betValue.Value*multiplier.Value).ToString("F2");
        }
    }

    void CalculateCurrency(int index)
    {
        safeTilesRevealed++;
        
        //update earned currency
        earnedCurrencyText.text = currencyMultiplierText.text;
        
        //update next flip earnable currency
        float nextMultiplier = CalculateMultiplier(safeTilesRevealed, mineCountValue.Value);
        currencyMultiplierText.text = ((float.Parse(earnedCurrencyText.text))*nextMultiplier).ToString("F2");
    }
    
    float CalculateMultiplier(int safeTilesRevealed, int totalMines)
    {
        int totalTiles = 25;
        int remainingTiles = totalTiles - safeTilesRevealed;
        int remainingMines = totalMines;
        int remainingSafeTiles = remainingTiles - remainingMines;
        
        // Edge case: no safe tiles left (shouldn't happen in normal gameplay)
        if (remainingSafeTiles <= 0) return 1f;
        
        float probability = (float)remainingSafeTiles / remainingTiles;
        float baseMultiplier = 1f / probability;
        float riskRewardRate = 0.2f;
        
        return 1f + ((baseMultiplier - 1f) * riskRewardRate);
    }
    
    void UpdateInitialMultiplier()
    {
        if (int.TryParse(mineCountText.text, out int mines))
        {
            mineCountValue.Value = mines;
            betValue.Value = float.Parse(betText.text);
            float tempMultiplier = CalculateMultiplier(0, mineCountValue.Value);
            currencyMultiplierText.text = (betValue.Value * tempMultiplier).ToString("F2");
        }
    }
    
    public void UpdateCurrencyViaBet()
    {
        betValue.Value = float.Parse(betText.text);
        float nextMultiplier = CalculateMultiplier(safeTilesRevealed, mineCountValue.Value);
        
        if (safeTilesRevealed == 0)
        {
            currencyMultiplierText.text = (betValue.Value * nextMultiplier).ToString("F2");
        }
        else
        {
            float currentEarned = float.Parse(earnedCurrencyText.text);
            currencyMultiplierText.text = (currentEarned * nextMultiplier).ToString("F2");
        }
    }
    
}
