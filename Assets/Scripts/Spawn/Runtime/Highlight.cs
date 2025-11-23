using System;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.UIButton;

public class Highlight : MonoBehaviour
{
    int columns = 10;
    int rows = 10;
    
    private void OnEnable()
    {
        GridLayoutGroup gridLayoutGroup = transform.GetComponent<GridLayoutGroup>();
    }

    [ContextMenu("Highlight")]
    public void HighlightColumn()
    {
        
        var childcount = transform.childCount;
        /*
        for (int i = childcount-1; i >= 0; i--)
        {
            var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
            buttonUI.enabled = false;
        }
        */
        
        for (int col = 0; col < columns; col++)
        {
            if(col == 1)
            {
                for (int row = 0; row < rows; row++)
                {
                    int index = row * columns + col;
                    var buttonUI = transform.GetChild(index);
                    buttonUI.GetComponent<Image>().color = new Color32(200, 0, 10, 100);
                   
                }
            }
        }
    }
}
