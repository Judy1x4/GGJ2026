using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using NUnit.Framework;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.Rendering.DebugUI;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Hotspot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Info")]
    public string itemName = "Axe";

    [Header("Item Traits/Clues")]
    public List<ItemTrait> traits = new List<ItemTrait>();

    // Generate full description from traits
    private string GetFullDescription()
    {
        string description = "";

        for (int i = 0; i < traits.Count; i++)
        {
            if (traits[i].isHighlighted)
            {
                // Color code by category
                string color = GetCategoryColor(traits[i].category);
                description += $"<color={color}>[{traits[i].traitText}]</color>";
            }
            else
            {
                description += traits[i].traitText;
            }

            if (i < traits.Count - 1)
                description += " ";
        }

        return description;
    }

    private string GetCategoryColor(ClueCategory category)
    {
        switch (category)
        {
            case ClueCategory.Item:
                return "#FFD700"; // Gold
            case ClueCategory.Location:
                return "#00CED1"; // Cyan/Turquoise
            case ClueCategory.Person:
                return "#FF69B4"; // Pink
            default:
                return "#FFFFFF"; // White
        }
    }

    private HiddenObjectUI uiManager;
    private bool hasBeenExamined = false;

    private void Start()
    {
        uiManager = FindFirstObjectByType<HiddenObjectUI>();

        if (uiManager == null)
            Debug.LogError("HiddenObjectUI not found!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager == null) return;

        if (GameManager.Instance != null && !GameManager.Instance.CanExamine())
        {
            Debug.Log("No attempts remaining!");
            return;
        }

        if (hasBeenExamined)
        {
            ShowItemPopup();
            Debug.Log($"Re-examining {itemName} (no attempt used)");
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UseAttempt();
        }

        hasBeenExamined = true;
        ShowItemPopup();

        Debug.Log($"Examined {itemName} for the first time");
    }

    private void ShowItemPopup()
    {
        if (uiManager == null) return;

        string description = GetFullDescription();
        List<ItemTrait> highlightedTraits = traits.FindAll(t => t.isHighlighted);

        uiManager.ShowItemWithTraits(itemName, description, highlightedTraits, this);
    }

    public string GetItemName()
    {
        return itemName;
    }
}