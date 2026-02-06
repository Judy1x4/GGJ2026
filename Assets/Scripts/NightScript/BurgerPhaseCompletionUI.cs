using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BurgerPhaseCompleteUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject completionPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI patienceResultText;
    [SerializeField] private TextMeshProUGUI attemptsText;
    [SerializeField] private TextMeshProUGUI secretsText;
    [SerializeField] private Button continueButton;
    [SerializeField] private PlayerStatus playerStatus;

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "FirstScene";

    private void Awake()
    {
        // Hide panel immediately on awake (before anything else runs)
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // Ensure panel is hidden
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        // Subscribe to burger phase complete event
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.OnBurgerPhaseComplete.AddListener(ShowCompletionUI);
        }
        else
        {
            // Try again on next frame if PatienceManager isn't ready
            Invoke(nameof(SubscribeToPatienceManager), 0.1f);
        }

        // Setup continue button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }

    private void SubscribeToPatienceManager()
    {
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.OnBurgerPhaseComplete.AddListener(ShowCompletionUI);
        }
    }

    public void ShowCompletionUI(int attempts)
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        // Get final patience
        int finalPatience = 0;
        if (PatienceManager.Instance != null)
        {
            finalPatience = PatienceManager.Instance.GetCurrentPatience();
        }

        // Update UI text
        if (titleText != null)
        {
            titleText.text = "Shift Complete!";
            secretsText.text = string.Join(", ", playerStatus.secretsObtained);
        }

        if (patienceResultText != null)
        {
            string rating = GetPerformanceRating(finalPatience);
            patienceResultText.text = $"Final Customer Patience: {finalPatience}\nPerformance: {rating}";
        }

        if (attemptsText != null)
        {
            attemptsText.text = $"You earned {attempts} investigation attempt{(attempts > 1 ? "s" : "")} for tonight!";
        }
    }

    private string GetPerformanceRating(int patience)
    {
        if (patience >= 30)
            return "<color=#00FF00>Excellent!</color>";
        else if (patience >= 20)
            return "<color=#FFFF00>Good</color>";
        else if (patience >= 10)
            return "<color=#FFA500>Fair</color>";
        else
            return "<color=#FF0000>Poor</color>";
    }

    private void OnContinueClicked()
    {
        // Load the next scene (investigation)
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.OnBurgerPhaseComplete.RemoveListener(ShowCompletionUI);
        }
    }
}
