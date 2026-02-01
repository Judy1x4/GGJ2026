using UnityEngine;
using TMPro;

public class SecretUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI secretText;
    [SerializeField] private GameObject secretBubble;
    [SerializeField] private Secrets secrets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secretText.text = string.Empty;
        secretBubble.SetActive(false);
    }

    // Update is called once per frame
    public void OnListen(string secret)
    {
        secretText.text = secret;
        secretBubble.SetActive(true);
    }

    public void FinishListen()
    {
        secretText.text = string.Empty;
        secretBubble.SetActive(false);
    }
}
