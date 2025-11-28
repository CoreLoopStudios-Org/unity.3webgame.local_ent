using System.Collections;
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
    public BooleanEvent roundEndEvent;
    
    private void OnEnable()
    {
        OnStartTimer();
    }

    private void OnStartTimer()
    {
        selected.Clear();
        currentCoins.Value = int.Parse(availableCoinsText.text);
        selectedTilesText.text = selectedCount.ToString();
        if (currentCoins.Value ! > 5)
        {
            integerEvent.AddListener(CalculateCurrency);
        }
        roundEndEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        DisableButtonsOnInsufficientCurrency();
        OnEndTimer();
    }

    private void OnEndTimer()
    {
        integerEvent.RemoveListener(CalculateCurrency);
    }

    #region Currency Calculator

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
    #endregion
    public void Reset(bool isReset)
    {
        if (isReset)
        {
            StartCoroutine(WaitTime(01f));
        }
    }
    IEnumerator WaitTime(float i)
    {
        yield return new WaitForSeconds(i); 
        OnEndTimer();
        yield return new WaitForSeconds(i+5f); 
        OnStartTimer(); ;
    }
}
