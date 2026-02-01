using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSelect : MonoBehaviour, IPointerClickHandler
{
    public Ingredient ingredient;
    public OrderSubmit orderSubmit;

    public void OnPointerClick(PointerEventData eventData)
    {
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
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void ActivateCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
