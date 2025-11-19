using UnityEditor.MemoryProfiler;
using UnityEngine;
using VirtueSky.Events;

public class MinesPopUpButton : MonoBehaviour
{
    public GameObject minesCountPopUp;


    //public IntegerEvent clickedIndex;
    private void OnEnable()
    {
        minesCountPopUp.SetActive(false);
    }

    private void OnDisable()
    {
    }

    public void SetPopupStatus()
    {
        minesCountPopUp.SetActive(!minesCountPopUp.activeSelf);
    }
}