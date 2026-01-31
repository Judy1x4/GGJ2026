using System.Collections; 
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HiddenObjectUI : MonoBehaviour
{
    public static HiddenObjectUI Instance;

    // WrongClickX
    public Image xMarker;
    public GameObject popupPanel;
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;
    public Button closeButton;

    public float xDisplayDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (xMarker != null)
        {
            xMarker.gameObject.SetActive(false);
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePopup);
        }
        
    }

    public void ShowXMarker(Vector2 position)
    {
        if (xMarker == null) return;
        xMarker.transform.position = position; 
        xMarker.gameObject.SetActive(true);

        // Hide after xDisplayDuration
        Invoke(nameof(HideXMarker), xDisplayDuration);
    }

    private void HideXMarker()
    {
        if (xMarker != null)
        {
            xMarker.gameObject.SetActive(false);
        }
    }

    public void ShowPopup(string title, string description)
    {
        if (popupPanel == null)
        {
            return; 
        }

        if (popupTitle != null)
        {
            popupTitle.text = title;
        }

        if (popupBody != null)
        {
            popupBody.text = description; 
        }

        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel
                .SetActive(false);
        }
    }
}
