using System;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;

public class Spawn : MonoBehaviour
{
    public Button prefab;
    public EventNoParam eventNoParm;
    //public IntegerEvent clickedIndex;
    
    
    private void OnEnable()
    {
        eventNoParm.AddListener(Spawn100);
    }
    
    private void OnDisable()
    {
        eventNoParm.RemoveListener(Spawn100);
    }

    [ContextMenu("Spawn")]
    public void Spawn100()
    {
        for (int i = 0; i < 100; i++)
        {
            var button = Instantiate(prefab, transform.position, transform.rotation, transform);
            int index = i; 
            //button.onClick.AddListener(() => OnButtonClick(index));
            var gameObjectName = button.gameObject.name;
            button.gameObject.name = String.Format(gameObjectName, i.ToString());
        }
    }

    public void OnButtonClick(int i)
    {
        //clickedIndex.Raise(i);
    }
}