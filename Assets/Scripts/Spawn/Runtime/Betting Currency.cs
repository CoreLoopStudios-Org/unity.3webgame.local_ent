using TMPro;
using UnityEngine;

public class BettingCurrency : MonoBehaviour
{

    public TMP_Text CurrencyAmountText;
    [SerializeField]  private float startingBetAmmount = 3.25f ;
    [SerializeField]  private float increaseBetAmmount = 0.5f ;
    [SerializeField]  private float decreaseBetAmmount = 0.5f ;
    
    float currentAmmount;

    void OnEnable()
    {
        CurrencyAmountText.text = startingBetAmmount.ToString();
    }
    public void IncreaseBet()
    {
        currentAmmount = float.Parse(CurrencyAmountText.text);
        if (currentAmmount < 200.00f)
        {
            currentAmmount += increaseBetAmmount;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
        else
        {
            currentAmmount = 200;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
        
    }
    
    public void DecreaseBet()
    {
        currentAmmount = float.Parse(CurrencyAmountText.text);
        if (currentAmmount > 0.00f)
        {
            currentAmmount -= decreaseBetAmmount;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
        else
        {
            currentAmmount = 0;
            CurrencyAmountText.text = currentAmmount.ToString();
        }
    }
}
