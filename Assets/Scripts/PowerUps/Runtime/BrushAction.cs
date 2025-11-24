using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Animation;

public class BrushAction : MonoBehaviour
{
    
    [ContextMenu("Brush Action")]
    public void OnActionHammer()
    {
        SpawnGame2.RewardData[] data = SpawnGame2.arr2;

        List<int> rewardNumbers = data.Select(d => d.rewardNumber).ToList();

        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            child.GetComponent<LitMotionAnimation>().Play();
            if (rewardNumbers.Contains(i))
            {
                var glowObject = child.transform.GetChild(3).gameObject;
                glowObject.SetActive(true);
                glowObject.GetComponent<LitMotionAnimation>().Play();
                
            }
        }
    }

}
