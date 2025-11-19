using UnityEngine;
using VirtueSky.Events;

public class Mines : MonoBehaviour
{
    public int maxMineCount = 20;
    public IntegerEvent clickedIndex;
    private void OnEnable()
    {
        //clickedIndex.AddListener();
    }
    
    private void OnDisable()
    {
        //clickedIndex.RemoveListener();
    }

    void SetMinesCount(int count)
    {
        
    }
}
