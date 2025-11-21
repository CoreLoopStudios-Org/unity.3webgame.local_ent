using UnityEngine;
using VirtueSky.Events;

public class PopUpButton1 : MonoBehaviour
{
    public GameObject historyPopUp;
    private bool ishistoryPopUp;


    //public IntegerEvent clickedIndex;
    private void OnEnable()
    {
        historyPopUp.SetActive(false);
    }

    private void OnDisable()
    {
    }

    public void HistoryPopUpFuntion()
    {
        ishistoryPopUp = true;
        SetPopupStatus();
    }
    

    private void SetPopupStatus()
    {
        if(ishistoryPopUp) {historyPopUp.SetActive(!historyPopUp.activeSelf);}
    }
}