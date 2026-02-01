using UnityEngine;
using UnityEngine.UI;

public class JournalButtonManager : MonoBehaviour
{
    [Header("Button Reference")]
    public Button journalButton;
    public Canvas journalButtonCanvas;

    public static JournalButtonManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (journalButtonCanvas != null)
            {
                DontDestroyOnLoad(journalButtonCanvas.gameObject);
            }

            Debug.Log("JournalButtonManager created and persisted");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (journalButton != null)
        {
            journalButton.onClick.AddListener(OpenJournal);

            // Start hidden
            journalButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Journal Button is not assigned!");
        }
    }

    private void OpenJournal()
    {
        if (JournalUI.Instance != null)
        {
            JournalUI.Instance.OpenJournal();
            Debug.Log("Journal opened via JournalButton");
        }
        else
        {
            Debug.LogError("JournalUI.Instance is null!");
        }
    }

    public void ShowButton()
    {
        if (journalButton != null)
        {
            journalButton.gameObject.SetActive(true);
            Debug.Log("Journal button shown");
        }
    }

    public void HideButton()
    {
        if (journalButton != null)
        {
            journalButton.gameObject.SetActive(false);
            Debug.Log("Journal button hidden");
        }
    }
}