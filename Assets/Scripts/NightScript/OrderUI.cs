using UnityEngine;
using TMPro;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject orderBox;

    private string currentOrderText = string.Empty;
    private string currentStatusText = string.Empty;

    public void Awake()
    {
        orderBox.SetActive(false);
        orderText.text = string.Empty;
        statusText.text = string.Empty;
    }

    public void ServeOrder(Menu menu)
    {
        orderBox.SetActive(true);
        currentOrderText = string.Format(menu.ToString());
        orderText.text = currentOrderText;

    }

    public void OnSuccess()
    {
        orderText.text = string.Format(" ");
        statusText.text = string.Format(" ");
        orderBox.SetActive(false);
    }

    public void OnFailure()
    {
        currentStatusText = string.Format("your order is wrong");
        statusText.text = currentStatusText;
    }

    public void OnListen()
    {
        orderText.text = string.Empty;
        statusText.text = string.Empty;
        orderBox.SetActive(false);
    }
    
    public void FinishListen()
    {
        orderText.text = currentOrderText;
        statusText.text = currentStatusText;
        orderBox.SetActive(true);
    }
}
