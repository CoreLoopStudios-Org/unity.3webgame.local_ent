using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HammerAction : MonoBehaviour
{
    /*void DebugFunction()
    {
        for (int i = 0; i < data.Length; i++)
        {
            Debug.Log($"Index {i}: rewardNumber = {data[i].rewardNumber}, rarity = {data[i].rarity}");
        }
    }*/

    [ContextMenu("Hammer Action")]
    public void OnActionHammer()
    {
        SpawnGame2.RewardData[] data = SpawnGame2.arr2;
        
        List<int> rewardNumbers = data.Select(d => d.rewardNumber).ToList();
    
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!rewardNumbers.Contains(i))
            {
                var buttonGO = transform.GetChild(i).gameObject;
                Image image = buttonGO.gameObject.GetComponent<Image>(); 
                image.enabled = false;
            }
        }
    }
}
