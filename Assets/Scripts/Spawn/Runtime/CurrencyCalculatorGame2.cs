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
    
    private Coroutine resetCoroutine;
    
    private void OnEnable()
    {
        OnStartTimer();
    }

    private void OnStartTimer()
    {
        selected.Clear();
        selectedCount = 0;
        currentCoins.Value = int.Parse(availableCoinsText.text);
        selectedTilesText.text = selectedCount.ToString();
        
        // Fixed: changed "! >" to ">="
        if (currentCoins.Value >= 5)
        {
            integerEvent.AddListener(CalculateCurrency);
        }
        roundEndEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        // Critical: Stop coroutine before object is destroyed
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
        
        DisableButtonsOnInsufficientCurrency();
        OnEndTimer();
    }

    private void OnEndTimer()
    {
        integerEvent.RemoveListener(CalculateCurrency);
        roundEndEvent.RemoveListener(Reset);
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
        if (transform == null) return;
        
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!selected.Contains(i))
            {
                var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
                if (buttonUI != null)
                {
                    buttonUI.enabled = false;
                }
            }
        }
    }
    
    void EnableButtonsOnSufficientCurrency()
    {
        if (transform == null) return;
        
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!selected.Contains(i))
            {
                var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
                if (buttonUI != null)
                {
                    buttonUI.enabled = true;
                }
            }
        }
    }
    #endregion
    
    public void Reset(bool isReset)
    {
        if (isReset)
        {
            // Stop any existing coroutine
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            
            // Start new coroutine and store reference
            resetCoroutine = StartCoroutine(WaitTime(1f));
        }
    }
    
    IEnumerator WaitTime(float i)
    {
        yield return new WaitForSeconds(i);
        
        // Safety check before continuing
        if (this == null || !this.isActiveAndEnabled)
        {
            resetCoroutine = null;
            yield break;
        }
        
        OnEndTimer();
        
        yield return new WaitForSeconds(i + 5f);
        
        // Safety check again
        if (this == null || !this.isActiveAndEnabled)
        {
            resetCoroutine = null;
            yield break;
        }
        
        OnStartTimer();
        resetCoroutine = null;
    }
}