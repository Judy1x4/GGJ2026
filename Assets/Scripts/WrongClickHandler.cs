using UnityEngine;
using UnityEngine.EventSystems; 

public class WrongClickHandler : MonoBehaviour, IPointerClickHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private HiddenObjectUI uiManager;
    void Start()
    {
        uiManager = FindFirstObjectByType<HiddenObjectUI>();

        if (uiManager == null)
        {
            Debug.LogError("HiddenObjectUI not found!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager == null)
        {
            return;
        }
        uiManager.ShowXMarker(eventData.position);
    }
}
