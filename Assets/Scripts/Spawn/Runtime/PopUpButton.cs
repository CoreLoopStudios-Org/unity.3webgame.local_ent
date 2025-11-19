using UnityEditor.MemoryProfiler;
using UnityEngine;
using VirtueSky.Events;

public class PopUpButton : MonoBehaviour
{
    public GameObject minesCountPopUp;
    public GameObject startingMoneyPopUp;
    private bool isMinesCountPopUp;
    private bool isStartingMoneyPopUp;

    //public IntegerEvent clickedIndex;
    private void OnEnable()
    {
        minesCountPopUp.SetActive(false);
        startingMoneyPopUp.SetActive(false);
    }

    private void OnDisable()
    {
    }

    public void MinesCountPopUp()
    {
        isMinesCountPopUp = true;
        isStartingMoneyPopUp = false;
        SetPopupStatus();
    }
    
    public void StartingMoneyPopUp()
    {
        isStartingMoneyPopUp = true;
        isMinesCountPopUp = false;
        SetPopupStatus();
    }
    

    private void SetPopupStatus()
    {
        if(isMinesCountPopUp) {minesCountPopUp.SetActive(!minesCountPopUp.activeSelf);}
        if(isStartingMoneyPopUp) {startingMoneyPopUp.SetActive(!startingMoneyPopUp.activeSelf);}
    }
}