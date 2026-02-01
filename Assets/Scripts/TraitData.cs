using UnityEngine;

public enum ClueCategory
{
    Item,
    Location,
    Person
}

[System.Serializable]
public class ItemTrait
{
    public string traitText;
    public bool isHighlighted; // Should this trait be highlighted?
    public ClueCategory category = ClueCategory.Item; // What type of clue is this?
}

[System.Serializable]
public class ClueEntry
{
    public string sourceName; // Name of item/location/person where clue was found
    public string clueText; // The actual clue text
    public ClueCategory category; // Item, Location, or Person

    public ClueEntry(string source, string clue, ClueCategory cat)
    {
        sourceName = source;
        clueText = clue;
        category = cat;
    }
}