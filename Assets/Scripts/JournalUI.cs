using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class JournalUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject journalPanel;
    public Transform cluesContent;
    public GameObject clueEntryPrefab;
    public TextMeshProUGUI noCluesText;
    public Button closeJournalButton;

    [Header("Tabs")]
    public Button itemsTab;
    public Button locationsTab;
    public Button personsTab;

    [Header("Summary")]
    public TextMeshProUGUI itemSummary;
    public TextMeshProUGUI locationSummary;
    public TextMeshProUGUI personSummary;

    [Header("Category Icons")]
    public Sprite itemIcon;
    public Sprite locationIcon;
    public Sprite personIcon;

    private ClueCategory currentCategory = ClueCategory.Item;

    private void Start()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);

        if (closeJournalButton != null)
            closeJournalButton.onClick.AddListener(CloseJournal);

        // Setup tabs
        if (itemsTab != null)
            itemsTab.onClick.AddListener(() => ShowCategory(ClueCategory.Item));

        if (locationsTab != null)
            locationsTab.onClick.AddListener(() => ShowCategory(ClueCategory.Location));

        if (personsTab != null)
            personsTab.onClick.AddListener(() => ShowCategory(ClueCategory.Person));
    }

    public void OpenJournal()
    {
        if (journalPanel == null) return;

        currentCategory = ClueCategory.Item; // Start with Items tab
        RefreshJournal();
        journalPanel.SetActive(true);
    }

    public void CloseJournal()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);
    }

    private void ShowCategory(ClueCategory category)
    {
        currentCategory = category;
        RefreshCluesList();
        UpdateTabHighlight();
    }

    private void RefreshJournal()
    {
        RefreshCluesList();
        RefreshSummary();
        UpdateTabHighlight();
    }

    private void RefreshCluesList()
    {
        // Clear existing entries
        foreach (Transform child in cluesContent)
        {
            Destroy(child.gameObject);
        }

        if (ClueManager.Instance == null)
        {
            Debug.LogWarning("ClueManager not found!");
            return;
        }

        // Get clues for current category
        List<ClueEntry> clues = ClueManager.Instance.GetCluesByCategory(currentCategory);

        // Show "no clues" message if empty
        if (noCluesText != null)
        {
            noCluesText.gameObject.SetActive(clues.Count == 0);
            noCluesText.text = $"No {currentCategory} clues collected yet...";
        }

        // Create UI entry for each clue
        foreach (var clue in clues)
        {
            GameObject entryObj = Instantiate(clueEntryPrefab, cluesContent);

            // Set category icon
            Image iconImage = entryObj.transform.Find("CategoryIcon")?.GetComponent<Image>();
            if (iconImage != null)
                iconImage.sprite = GetCategoryIcon(clue.category);

            // Set texts
            TextMeshProUGUI sourceText = entryObj.transform.Find("SourceNameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI clueText = entryObj.transform.Find("ClueText")?.GetComponent<TextMeshProUGUI>();

            if (sourceText != null)
                sourceText.text = $"{clue.sourceName}:";

            if (clueText != null)
                clueText.text = clue.clueText;
        }
    }

    private void RefreshSummary()
    {
        if (ClueManager.Instance == null) return;

        // Count clues in each category
        int itemCount = ClueManager.Instance.GetClueCount(ClueCategory.Item);
        int locationCount = ClueManager.Instance.GetClueCount(ClueCategory.Location);
        int personCount = ClueManager.Instance.GetClueCount(ClueCategory.Person);

        // Update summary text
        if (itemSummary != null)
            itemSummary.text = itemCount > 0 ? $"Item: {itemCount} clue(s)" : "Item: ???";

        if (locationSummary != null)
            locationSummary.text = locationCount > 0 ? $"Location: {locationCount} clue(s)" : "Location: ???";

        if (personSummary != null)
            personSummary.text = personCount > 0 ? $"Person: {personCount} clue(s)" : "Person: ???";
    }

    private void UpdateTabHighlight()
    {
        // Update tab colors to show active tab
        ColorBlock itemColors = itemsTab.colors;
        ColorBlock locationColors = locationsTab.colors;
        ColorBlock personColors = personsTab.colors;

        // Reset all to normal
        itemsTab.interactable = true;
        locationsTab.interactable = true;
        personsTab.interactable = true;

        // Highlight current tab
        switch (currentCategory)
        {
            case ClueCategory.Item:
                itemsTab.interactable = false; // Makes it appear "selected"
                break;
            case ClueCategory.Location:
                locationsTab.interactable = false;
                break;
            case ClueCategory.Person:
                personsTab.interactable = false;
                break;
        }
    }

    private Sprite GetCategoryIcon(ClueCategory category)
    {
        switch (category)
        {
            case ClueCategory.Item:
                return itemIcon;
            case ClueCategory.Location:
                return locationIcon;
            case ClueCategory.Person:
                return personIcon;
            default:
                return null;
        }
    }

    public void ClearAllClues()
    {
        if (ClueManager.Instance != null)
        {
            ClueManager.Instance.ClearAllClues();
            RefreshJournal();
        }
    }
}