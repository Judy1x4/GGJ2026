using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> customerBehaviours = new();
    [SerializeField] private Recipe recipe; // drag your Recipe component here

    private int currentIndex = -1;
    public ICustomer CurrentCustomer { get; private set; }

    private void Start()
    {
        foreach (var mb in customerBehaviours)
        {
            if (mb != null) mb.gameObject.SetActive(false);
        }

        NextCustomer();
    }

    public void NextCustomer()
    {
        currentIndex++;

        if (currentIndex >= customerBehaviours.Count)
        {

            CurrentCustomer = null;
            Debug.Log("All customers served!");
            return;
        }

        var mb = customerBehaviours[currentIndex];
        if (mb == null)
        {
            Debug.LogWarning($"Customer at index {currentIndex} is null. Skipping.");
            NextCustomer();
            return;
        }

        CurrentCustomer = mb as ICustomer;
        if (CurrentCustomer == null)
        {
            Debug.LogError($"Object '{mb.name}' does not implement ICustomer. Skipping.");
            mb.gameObject.SetActive(false);
            NextCustomer();
            return;
        }

        mb.gameObject.SetActive(true);
        CurrentCustomer.Order();
    }

    public void SubmitOrder(List<Ingredient> madeIngredients)
    {
        if (CurrentCustomer == null)
        {
            Debug.LogWarning("No active customer.");
            return;
        }

        if (recipe == null)
        {
            Debug.LogError("Recipe reference is not assigned in OrderManager.");
            return;
        }

        bool correct = recipe.Matches(CurrentCustomer.CurrentOrder, madeIngredients);
        Debug.LogError("Incorrect." + CurrentCustomer.CurrentOrder);
        foreach (var ingredients in madeIngredients)
        {
            Debug.Log(ingredients);
        }

        if (correct)
        {
            CurrentCustomer.OnServedCorrect();
            CurrentCustomer.FinishOrder();
            NextCustomer();
        }
        else
        {
            CurrentCustomer.OnServedWrong();
        }
    }
}
