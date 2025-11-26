using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.UIButton;


public class FlipUI : MonoBehaviour
{
    
    public IntegerEvent integerEvent;

    private void OnEnable()
    {
        integerEvent.AddListener(Flipface);
    }


    private void OnDisable()
    {
        integerEvent.RemoveListener(Flipface);
    }
    

    public void Flipface(int index)
    {
        {
            var selected = transform.GetChild(index);
            selected.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        
            var buttonUI = selected.GetComponent<ButtonUI>();
            buttonUI.enabled = false;
        }
    }
    
}
