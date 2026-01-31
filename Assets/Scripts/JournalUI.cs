using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class JournalUI : MonoBehaviour
{
    [Header("UI References")]
    public Canvas journalCanvas;
    public GameObject journalPanel;
    public Transform cluesContent;
    public GameObject clueEntryPrefab;
    public TextMeshProUGUI noCluesText;
    public Button closeJournalButton;

    [Header("Tabs")]
    public Button itemsTab;
    public Button locationsTab;
    public Button personsTab;

    [Header("Category Icons")]
    public Sprite itemIcon;
    public Sprite locationIcon;
    public Sprite personIcon;

    // REMOVED: Summary fields

    private ClueCategory currentCategory = ClueCategory.Item;

    public static JournalUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (journalCanvas != null)
            {
                DontDestroyOnLoad(journalCanvas.gameObject);
            }

            Debug.Log("JournalUI singleton created and persisted");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);

        if (closeJournalButton != null)
            closeJournalButton.onClick.AddListener(CloseJournal);

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

        currentCategory = ClueCategory.Item;
        RefreshJournal();
        journalPanel.SetActive(true);

        Debug.Log("Journal opened");
    }

    public void CloseJournal()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);

        Debug.Log("Journal closed");
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
        // REMOVED: RefreshSummary();
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

    // REMOVED: RefreshSummary() method

    private void UpdateTabHighlight()
    {
        if (itemsTab == null || locationsTab == null || personsTab == null) return;

        // Reset all to normal
        itemsTab.interactable = true;
        locationsTab.interactable = true;
        personsTab.interactable = true;

        // Highlight current tab
        switch (currentCategory)
        {
            case ClueCategory.Item:
                itemsTab.interactable = false;
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
