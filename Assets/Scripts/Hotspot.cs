using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hotspot : MonoBehaviour, IPointerClickHandler
{
    public string objectId;

    void Awake()
    {
        Debug.Log("hotspot awake: " + objectId);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Found: " + objectId);
    }
}
