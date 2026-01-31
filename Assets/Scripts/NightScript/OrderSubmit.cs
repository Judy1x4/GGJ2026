using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private Ingredients ingredientsSource;

    [SerializeField] public List<Ingredient> currentPlate = new();

    public void OnPointerClick(PointerEventData eventData)
    {
        orderManager.SubmitOrder(currentPlate);
    }
}
