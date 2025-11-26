using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.UIButton;
using VirtueSky.Variables;

public class CurrencyCalculatorGame2 : MonoBehaviour
{
    public IntegerEvent integerEvent;
    public TMP_Text availableCoinsText;
    public TMP_Text selectedTilesText;
    private List<int> selected = new List<int>();
    int selectedCount = 0;
    [SerializeField] private IntegerVariable currentCoins;
    
    private void OnEnable()
    {
        selectedTilesText.text = selectedCount.ToString();
        integerEvent.AddListener(CalculateCurrency);
    }

    private void OnDisable()
    {
        integerEvent.RemoveListener(CalculateCurrency);
    }

    private void CalculateCurrency(int index)
    {
        currentCoins.Value = int.Parse(availableCoinsText.text);
        currentCoins.Value = currentCoins.Value - 6;
        availableCoinsText.text = currentCoins.ToString();
        selected.Add(index);
        selectedCount++;
        selectedTilesText.text = selectedCount.ToString();
        Debug.Log("selected numbers = [" + string.Join(", ", selected) + "]");
        if (currentCoins.Value >= 6)
        {
            //EnableButtonsOnSufficientCurrency();
        }
        else
        {
            DisableButtonsOnInsufficientCurrency();
        }
    }

    public void BuyCurrency()
    {
        currentCoins.Value = int.Parse(availableCoinsText.text);
        currentCoins.Value = currentCoins.Value + 6;
        availableCoinsText.text = currentCoins.ToString();
        EnableButtonsOnSufficientCurrency();
    }
    
    void DisableButtonsOnInsufficientCurrency()
    {
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!selected.Contains(i))
            {
                var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
                buttonUI.enabled = false;
            }
        }
    }
    void EnableButtonsOnSufficientCurrency()
    {
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!selected.Contains(i))
            {
                var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
                buttonUI.enabled = true;
            }
        }
    }
}
