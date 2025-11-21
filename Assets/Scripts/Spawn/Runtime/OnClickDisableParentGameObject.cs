using UnityEngine;
using VirtueSky.Events;

public class OnClickDisableParentGameObject : MonoBehaviour
{
    
    public void DisableParent()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
