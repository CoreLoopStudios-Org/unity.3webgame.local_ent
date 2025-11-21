using TMPro;
using UnityEngine;

public class BettingCurrency : MonoBehaviour
{

    public TMP_Text CurrencyAmountText;
    [SerializeField]  public float startingBetAmmount = 3.25f ;
    [SerializeField]  private float increaseBetAmmount = 2f ;
    [SerializeField]  private float decreaseBetAmmount = 2f ;
    
    float currentAmmount;

    void OnEnable()
    {
        CurrencyAmountText.text = startingBetAmmount.ToString();
    }
    public void IncreaseBet()
    {
        currentAmmount = float.Parse(CurrencyAmountText.text);
        float newAmount = currentAmmount + increaseBetAmmount;
        if (newAmount <= 15.00f)
        {
            currentAmmount = newAmount;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
        else
        {
            currentAmmount = 15f;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
        
    }
    
    public void DecreaseBet()
    {
        currentAmmount = float.Parse(CurrencyAmountText.text);
        float newAmount = currentAmmount - decreaseBetAmmount;

        if (newAmount >= 0.50f)
        {
            currentAmmount = newAmount;
            CurrencyAmountText.text = currentAmmount.ToString("F2");
        }
        else
        {
            currentAmmount = 0.50f;
            CurrencyAmountText.text = currentAmmount.ToString("F2");
        }
    }
}
