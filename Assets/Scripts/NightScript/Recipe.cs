using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Menu
{
    Cheeseburger,
    ClassicBurger,
    VeggieBurger,
    PickleBurger
}

public class Recipe : MonoBehaviour
{
    [SerializeField] private Ingredients ingredientsSource;

    private Dictionary<Menu, List<Ingredient>> Recipes;

    private void Awake()
    {
        Recipes = new Dictionary<Menu, List<Ingredient>>
        {
            { Menu.Cheeseburger, new List<Ingredient>{ ingredientsSource.lettuce, ingredientsSource.tomato, ingredientsSource.pickle, ingredientsSource.onion, ingredientsSource.cheese, ingredientsSource.patty } },
            { Menu.ClassicBurger, new List<Ingredient>{ ingredientsSource.lettuce, ingredientsSource.tomato, ingredientsSource.onion, ingredientsSource.patty } },
            { Menu.VeggieBurger,  new List<Ingredient>{ ingredientsSource.lettuce, ingredientsSource.tomato, ingredientsSource.pickle, ingredientsSource.onion, ingredientsSource.cheese } },
            { Menu.PickleBurger,  new List<Ingredient>{ ingredientsSource.pickle, ingredientsSource.onion, ingredientsSource.cheese, ingredientsSource.patty } },
        };
    }

    public List<Ingredient> GetIngredients(Menu menu)
        => new List<Ingredient>(Recipes[menu]);

    public Menu GetRandomMenu()
    {
        var values = (Menu[])System.Enum.GetValues(typeof(Menu));
        return values[Random.Range(0, values.Length)];
    }

    public bool Matches(Menu menu, List<Ingredient> madeIngredients)
    {
        var requiredIngredients = GetIngredients(menu);

        if (requiredIngredients == null) return false;
        if (madeIngredients == null) return false;

        // Remove nulls (prevents null keys / NRE surprises)
        requiredIngredients = requiredIngredients.Where(i => i != null).ToList();
        var madeClean = madeIngredients.Where(i => i != null).ToList();

        if (madeClean.Count != requiredIngredients.Count) return false;

        // Count-based compare (multiset equality by Ingredient reference)
        var reqCounts = requiredIngredients
            .GroupBy(i => i)
            .ToDictionary(g => g.Key, g => g.Count());

        var madeCounts = madeClean
            .GroupBy(i => i)
            .ToDictionary(g => g.Key, g => g.Count());

        if (reqCounts.Count != madeCounts.Count) return false;

        foreach (var kv in reqCounts)
        {
            if (!madeCounts.TryGetValue(kv.Key, out int count)) return false;
            if (count != kv.Value) return false;
        }

        return true;
    }
}
