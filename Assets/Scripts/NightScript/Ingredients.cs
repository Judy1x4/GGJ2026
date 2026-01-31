using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{
    public Ingredient lettuce;
    public Ingredient tomato;
    public Ingredient pickle;
    public Ingredient onion;
    public Ingredient cheese;
    public Ingredient patty;

    public List<Ingredient> ingredients;

    private void Awake()
    {
        ingredients = new List<Ingredient>
        {
            lettuce,
            tomato,
            pickle,
            onion,
            cheese,
            patty
        };
    }
}
