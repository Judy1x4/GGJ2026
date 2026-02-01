using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HiddenObjectUI : MonoBehaviour
{
    [Header("X Marker")]
    public Image xMarker;
    public float xDisplayDuration = 1f;

    [Header("Multi-Purpose Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI rightButtonText;

    // Track current item being viewed
    private string currentItemName;
    private List<ItemTrait> currentHighlightedTraits;
    
    private void Start()
    {
        if (xMarker != null)
            xMarker.gameObject.SetActive(false);

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void ShowXMarker(Vector2 position)
    {
        if (xMarker == null) return;

        xMarker.transform.position = position;
        xMarker.gameObject.SetActive(true);

        Invoke(nameof(HideXMarker), xDisplayDuration);
    }

    private void HideXMarker()
    {
        if (xMarker != null)
            xMarker.gameObject.SetActive(false);
    }

    // Show item with save to journal button
    public void ShowItemWithTraits(string title, string description, List<ItemTrait> highlightedTraits, Hotspot hotspot)
    {
        // Store current item info
        currentItemName = hotspot.GetItemName();
        currentHighlightedTraits = highlightedTraits;

        // Determine if there are highlighted traits to save
        bool hasHighlightedTraits = highlightedTraits != null && highlightedTraits.Count > 0;

        // Show popup with Save to Journal as right button
        ShowPopup(
            title: title,
            body: description,
            leftButtonText: "Close",
            rightButtonText: hasHighlightedTraits ? "Save" : null,
            onLeftClick: () => ClosePopup(),
            onRightClick: hasHighlightedTraits ? (System.Action)OnSaveToJournal : null
        );
    }

    private void OnSaveToJournal()
    {
        if (ClueManager.Instance != null && currentHighlightedTraits != null && currentHighlightedTraits.Count > 0)
        {
            ClueManager.Instance.AddItemTraits(currentItemName, currentHighlightedTraits);
            Debug.Log($"Saved all traits from {currentItemName} to journal");

            // Give visual feedback
            if (rightButtonText != null)
            {
                string originalText = rightButtonText.text;
                rightButtonText.text = "Saved!";

                // Disable button temporarily
                if (rightButton != null)
                    rightButton.interactable = false;

                // Reset after 1 second
                Invoke(nameof(ResetSaveButtonState), 1f);
            }
        }
        else
        {
            Debug.LogWarning("Cannot save - no highlighted traits or ClueManager missing");
        }
    }

    private void ResetSaveButtonState()
    {
        if (rightButtonText != null)
        {
            rightButtonText.text = "Save to Journal";
        }

        if (rightButton != null)
        {
            rightButton.interactable = true;
        }
    }

    public void ShowPopup(
        string title,
        string body,
        string leftButtonText,
        string rightButtonText,
        System.Action onLeftClick,
        System.Action onRightClick)
    {
        if (popupPanel == null) return;

        if (popupTitle != null)
            popupTitle.text = title;

        if (popupBody != null)
            popupBody.text = body;

        // Configure left button
        if (leftButton != null && this.leftButtonText != null)
        {
            this.leftButtonText.text = leftButtonText;
            leftButton.gameObject.SetActive(!string.IsNullOrEmpty(leftButtonText));

            leftButton.onClick.RemoveAllListeners();
            if (onLeftClick != null)
                leftButton.onClick.AddListener(() => onLeftClick());
        }

        // Configure right button
        if (rightButton != null && this.rightButtonText != null)
        {
            bool showRightButton = !string.IsNullOrEmpty(rightButtonText);
            rightButton.gameObject.SetActive(showRightButton);

            if (showRightButton)
            {
                this.rightButtonText.text = rightButtonText;

                rightButton.onClick.RemoveAllListeners();
                if (onRightClick != null)
                    rightButton.onClick.AddListener(() => onRightClick());
            }
        }

        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        // Reset current item tracking
        currentItemName = null;
        currentHighlightedTraits = null;

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
