using TMPro;
using UnityEngine;
using VirtueSky.Events;

public class CurrencyCalculator : MonoBehaviour
{
    public IntegerEvent currencyMultiplierIntegerEvent;
    
    
    public TMP_Text mineCountText;
    public TMP_Text currencyMultiplierText;
    public TMP_Text betText;
    
    private float multiplier;
    private float betValue;
    private float mineCountValue;
    
    private void OnEnable()
    {
        currencyMultiplierIntegerEvent.AddListener(UpdateCurrencyViaMines);
    }


    private void OnDisable()
    {
        currencyMultiplierIntegerEvent.RemoveListener(UpdateCurrencyViaMines);
    }

    void UpdateCurrencyViaMines(int index)
    {
        if (index >= 2 && index <= 24)
        {
            multiplier = CalculateMultiplier(index);
            betValue = float.Parse(betText.text);
            mineCountValue = float.Parse(mineCountText.text);
            currencyMultiplierText.text = ((betValue + mineCountValue)*multiplier).ToString();
        }
    }
    
    float CalculateMultiplier(int index)
    {
        // Define your formula here based on the pattern
        return 0.07f + (index * 0.5f);
    }
    
    public void UpdateCurrencyViaBet()
    {
        betValue = float.Parse(betText.text);
        mineCountValue = float.Parse(mineCountText.text); 
        currencyMultiplierText.text = ((betValue + mineCountValue)*multiplier).ToString();
    }
    
}
