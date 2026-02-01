using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSelect : MonoBehaviour, IPointerClickHandler
{
    public Ingredient ingredient;
    public OrderSubmit orderSubmit;
    private bool inputEnabled = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!inputEnabled) return;
        if (ingredient.gameObject.activeSelf)
        {
            ingredient.isSelected = false;
            ingredient.gameObject.SetActive(false);
            orderSubmit.currentPlate.Remove(ingredient);
        }
        else
        {
            ingredient.isSelected = true;
            ingredient.gameObject.SetActive(true);
            orderSubmit.currentPlate.Add(ingredient);
        }
    }

    public void DisableCollider()
    {
        inputEnabled = false;
    }

    public void ActivateCollider()
    {
        inputEnabled = true;
    }
}
