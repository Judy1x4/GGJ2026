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

    [Header("Trait Selection")]
    public Transform traitsContainer;
    public GameObject traitButtonPrefab;

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

    public void ShowItemWithTraits(string title, string description, List<ItemTrait> highlightedTraits, Hotspot hotspot)
    {
        // Clear previous trait buttons
        if (traitsContainer != null)
        {
            foreach (Transform child in traitsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        // Create trait selection buttons
        if (highlightedTraits != null && highlightedTraits.Count > 0 && traitsContainer != null && traitButtonPrefab != null)
        {
            foreach (var trait in highlightedTraits)
            {
                GameObject buttonObj = Instantiate(traitButtonPrefab, traitsContainer);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                // Color code button by category
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = GetCategoryButtonColor(trait.category);
                }

                if (buttonText != null)
                {
                    string categoryLabel = GetCategoryLabel(trait.category);
                    buttonText.text = $"Save {categoryLabel}: {trait.traitText}";
                }

                if (button != null)
                {
                    string traitTextCopy = trait.traitText;
                    ClueCategory categoryCopy = trait.category;

                    button.onClick.AddListener(() =>
                        SaveClue(hotspot.GetItemName(), traitTextCopy, categoryCopy));
                }
            }

            traitsContainer.gameObject.SetActive(true);
        }
        else
        {
            if (traitsContainer != null)
                traitsContainer.gameObject.SetActive(false);
        }

        ShowPopup(
            title: title,
            body: description,
            leftButtonText: "Close",
            rightButtonText: null,
            onLeftClick: () => ClosePopup(),
            onRightClick: null
        );
    }

    private void SaveClue(string sourceName, string clueText, ClueCategory category)
    {
        if (ClueManager.Instance != null)
        {
            ClueManager.Instance.AddClue(sourceName, clueText, category);
            Debug.Log($"Saved {category} clue: {sourceName} - {clueText}");
        }
    }

    private Color GetCategoryButtonColor(ClueCategory category)
    {
        switch (category)
        {
            case ClueCategory.Item:
                return new Color(1f, 0.84f, 0f); // Gold
            case ClueCategory.Location:
                return new Color(0f, 0.81f, 0.82f); // Cyan
            case ClueCategory.Person:
                return new Color(1f, 0.41f, 0.71f); // Pink
            default:
                return Color.white;
        }
    }

    private string GetCategoryLabel(ClueCategory category)
    {
        switch (category)
        {
            case ClueCategory.Item:
                return "Item";
            case ClueCategory.Location:
                return "Location";
            case ClueCategory.Person:
                return "Person";
            default:
                return "Clue";
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

        if (leftButton != null && this.leftButtonText != null)
        {
            this.leftButtonText.text = leftButtonText;
            leftButton.gameObject.SetActive(!string.IsNullOrEmpty(leftButtonText));

            leftButton.onClick.RemoveAllListeners();
            if (onLeftClick != null)
                leftButton.onClick.AddListener(() => onLeftClick());
        }

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
        if (traitsContainer != null)
        {
            foreach (Transform child in traitsContainer)
            {
                Destroy(child.gameObject);
            }
            traitsContainer.gameObject.SetActive(false);
        }

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}