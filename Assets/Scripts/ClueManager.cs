using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    [Header("Collected Clues")]
    public List<ClueEntry> allClues = new List<ClueEntry>();

    public static ClueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddClue(string sourceName, string clueText, ClueCategory category)
    {
        // Check if this exact clue already exists
        bool alreadyExists = allClues.Exists(clue =>
            clue.sourceName == sourceName &&
            clue.clueText == clueText &&
            clue.category == category);

        if (!alreadyExists)
        {
            allClues.Add(new ClueEntry(sourceName, clueText, category));
            Debug.Log($"Added {category} clue: {sourceName} - {clueText}");
        }
        else
        {
            Debug.Log($"Clue already exists: {sourceName} - {clueText}");
        }
    }

    public void AddItemTraits(string itemName, List<ItemTrait> highlightedTraits)
    {
        if (highlightedTraits == null || highlightedTraits.Count == 0)
        {
            Debug.LogWarning($"No highlighted traits to save for {itemName}");
            return;
        }

        // Group traits by category
        var itemTraits = highlightedTraits.Where(t => t.category == ClueCategory.Item).ToList();
        var locationTraits = highlightedTraits.Where(t => t.category == ClueCategory.Location).ToList();
        var personTraits = highlightedTraits.Where(t => t.category == ClueCategory.Person).ToList();

        // Create entries for each category
        if (itemTraits.Count > 0)
        {
            string combinedText = string.Join(", ", itemTraits.Select(t => t.traitText.TrimEnd(',', '.', ' ')));
            AddClue(itemName, combinedText, ClueCategory.Item);
        }

        if (locationTraits.Count > 0)
        {
            string combinedText = string.Join(", ", locationTraits.Select(t => t.traitText.TrimEnd(',', '.', ' ')));
            AddClue(itemName, combinedText, ClueCategory.Location);
        }

        if (personTraits.Count > 0)
        {
            string combinedText = string.Join(", ", personTraits.Select(t => t.traitText.TrimEnd(',', '.', ' ')));
            AddClue(itemName, combinedText, ClueCategory.Person);
        }

        Debug.Log($"Saved all highlighted traits from {itemName} to journal");
    }

    // Get all clues
    public List<ClueEntry> GetAllClues()
    {
        return allClues;
    }

    // Get clues by category
    public List<ClueEntry> GetCluesByCategory(ClueCategory category)
    {
        return allClues.Where(c => c.category == category).ToList();
    }

    // Get clues for a specific source (e.g., all clues from "Axe")
    public List<ClueEntry> GetCluesBySource(string sourceName)
    {
        return allClues.Where(c => c.sourceName == sourceName).ToList();
    }

    public void ClearAllClues()
    {
        allClues.Clear();
        Debug.Log("All clues cleared");
    }

    // Get count by category
    public int GetClueCount(ClueCategory category)
    {
        return allClues.Count(c => c.category == category);
    }
}