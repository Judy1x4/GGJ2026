using UnityEngine;

public abstract class ICustomer : MonoBehaviour
{
    public bool IsOrdering { get; private set; }
    public bool IsOrderComplete { get; private set; }

    public Menu CurrentOrder { get; private set; }

    [SerializeField] protected float patienceSeconds = 10f;
    [SerializeField] protected Recipe recipe; // assign in Inspector

    public virtual void Order()
    {
        IsOrdering = true;
        CurrentOrder = recipe.GetRandomMenu();
    }

    public virtual void OnServedCorrect()
    {
    }

    public virtual void OnServedWrong()
    {
    }

    public void FinishOrder()
    {
        IsOrdering = false;
        IsOrderComplete = true;
        gameObject.SetActive(false);
    }
}
