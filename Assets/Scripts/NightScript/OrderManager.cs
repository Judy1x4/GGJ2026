using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> customerBehaviours = new();
    [SerializeField] private Recipe recipe; // drag your Recipe component here

    [Header("Shift Settings")]
    [SerializeField] private int successfulOrdersRequired = 10;

    [Header("Events")]
    public UnityEvent OnAllCustomersServed;

    private int currentIndex = -1;
    private int successfulOrders = 0;
    public ICustomer CurrentCustomer { get; private set; }
    public int SuccessfulOrders => successfulOrders;
    public int OrdersRequired => successfulOrdersRequired;

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
            
            // Complete the burger phase and calculate attempts
            if (PatienceManager.Instance != null)
            {
                PatienceManager.Instance.CompleteBurgerPhase();
            }
            
            OnAllCustomersServed?.Invoke();
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
            // Add patience for correct order
            if (PatienceManager.Instance != null)
            {
                PatienceManager.Instance.OnCorrectOrder();
            }
            
            successfulOrders++;
            Debug.Log($"Successful orders: {successfulOrders}/{successfulOrdersRequired}");
            
            CurrentCustomer.OnServedCorrect();
            CurrentCustomer.FinishOrder();
            
            // Check if we've reached the required successful orders
            if (successfulOrders >= successfulOrdersRequired)
            {
                CompleteShift();
            }
            else
            {
                NextCustomer();
            }
        }
        else
        {
            // Decrease patience for wrong order
            if (PatienceManager.Instance != null)
            {
                PatienceManager.Instance.OnWrongOrder();
            }
            
            CurrentCustomer.OnServedWrong();
        }
    }

    private void CompleteShift()
    {
        CurrentCustomer = null;
        Debug.Log($"Shift complete! Served {successfulOrders} successful orders.");
        
        // Complete the burger phase and calculate attempts
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.CompleteBurgerPhase();
        }
        
        OnAllCustomersServed?.Invoke();
    }
}
