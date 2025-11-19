using UnityEngine;
using VirtueSky.Events;

public class Room : MonoBehaviour
{
    public GameObject HomePanel;
    public GameObject InGamePanel;
    public EventNoParam eventNoParm;
    
    private void OnEnable()
    {
        
        eventNoParm.AddListener(GoldRoom);
    }
    
    private void OnDisable()
    {
        eventNoParm.RemoveListener(GoldRoom);
    }
    
    public void GoldRoom()
    {
        InGamePanel.SetActive(true);
        HomePanel.SetActive(false);
    }   
    
    
}
