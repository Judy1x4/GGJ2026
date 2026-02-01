using UnityEngine;
using TMPro;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject orderBox;

    public void Awake()
    {
        orderBox.SetActive(false);
        orderText.text = string.Empty;
        statusText.text = string.Empty;
    }

    public void ServeOrder(Menu menu)
    {
        orderBox.SetActive(true);
        orderText.text = string.Format(menu.ToString());
    }

    public void OnSuccess()
    {
        orderText.text = string.Format(" ");
        statusText.text = string.Format(" ");
        orderBox.SetActive(false);
    }

    public void OnFailure()
    {
        statusText.text = string.Format("your order is wrong");
    }
}
