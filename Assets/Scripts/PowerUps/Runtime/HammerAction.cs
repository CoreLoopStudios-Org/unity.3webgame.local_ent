using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HammerAction : MonoBehaviour
{
    
    [SerializeField]int usageCount = 3;
    [SerializeField] private TMP_Text usageCountText;
    
    int[] arr = new int[100];
    List<int> toDestroyList = new List<int>();

    void OnEnable()
    {
        int currentUsageCount = int.Parse(usageCountText.text);
        usageCount = currentUsageCount;
        usageCountText.text = usageCount.ToString();
    }
    
    [ContextMenu("Hammer Action")]
    public void OnActionHammer()
    {
        int currentUsageCount = int.Parse(usageCountText.text);
        usageCount = currentUsageCount;
        if (usageCount > 0)
        {
            HammerFunction();
            usageCount--;
            usageCountText.text = usageCount.ToString();
        }
    }

    void HammerFunction()
    {
        toDestroyList.Clear();
        SpawnGame2.RewardData[] data = SpawnGame2.arr2;
        
        List<int> rewardNumbers = data.Select(d => d.rewardNumber).ToList();
    
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (!rewardNumbers.Contains(i))
            {
                var buttonGO = transform.GetChild(i).gameObject;
                Image image = buttonGO.gameObject.GetComponent<Image>();
                if(image.enabled) {arr[i] = i;}
            }
            else
            {
                arr[i] = -1;
            }
        }
        DebugFunction(arr);
        
        Shuffle(arr);
        int count = 0;
        for (int i = 0; i < 100; i++)
        {
            if (count >= 10) break;
            if (arr[i] == -1)
            {
                i++;
                continue;
            }
            toDestroyList.Add(arr[i]);
            count++;
        }
        Debug.Log("[" + string.Join(", ", toDestroyList) + "]");
        
        
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            if (toDestroyList.Contains(i))
            {
                var buttonGO = transform.GetChild(i).gameObject;
                Image image = buttonGO.gameObject.GetComponent<Image>(); 
                image.enabled = false;
            }        
        }
    }
    public static void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k =  Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    
    void DebugFunction(int[] data)
    {
        string result = string.Join(", ", data.Select((value, index) => $"[{index}]={value}"));
        Debug.Log(result);
    }
}
