using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hotspot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Info")]
    public string itemName = "Axe";
    [TextArea(3, 10)]
    // 3: min number of lines shown
    // 10: max number of lines shown before scrollbar appears 
    public string itemDescription = "A rusty old axe";
    private HiddenObjectUI uiManager;

    private void Start()
    {
        Debug.Log("Hot spot is set up.");
        uiManager = FindFirstObjectByType
            <HiddenObjectUI>();
        if (uiManager == null)
            Debug.LogError("HiddenObjectUI not found!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager == null) return;
        uiManager.ShowPopup(itemName, itemDescription);
    }
}
