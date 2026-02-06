using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Location Info")]
    public string locationName = "Ocean";

    [Header("UI References")]
    public TextMeshProUGUI attemptsText;
    public Image[] attemptsIcons;

    private HiddenObjectUI uiManager;
    private int remainingAttempts;
    private int maxAttempts;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (JournalButtonManager.Instance != null)
        {
            JournalButtonManager.Instance.ShowButton();
        }
        uiManager = GetComponent<HiddenObjectUI>();

        // Get max attempts from game progress
        if (GameProgressManager.Instance != null)
        {
            maxAttempts = GameProgressManager.Instance.attemptsPerNight;
            GameProgressManager.Instance.StartNightInvestigation(locationName);
        }
        else
        {
            maxAttempts = 3; // Fallback
        }

        remainingAttempts = maxAttempts;
        UpdateAttemptsUI();
    }

    private void OnDestroy()
    {
        if (JournalButtonManager.Instance != null)
        {
            JournalButtonManager.Instance.HideButton();
        }
    }

    public bool CanExamine()
    {
        return remainingAttempts > 0;
    }

    public void UseAttempt()
    {
        if (remainingAttempts <= 0)
            return;

        remainingAttempts--;

        // Record in global progress
        if (GameProgressManager.Instance != null)
            GameProgressManager.Instance.RecordExamination();

        UpdateAttemptsUI();

        if (remainingAttempts <= 0)
        {
            ShowOutOfAttemptsPopup();
        }
    }

    private void UpdateAttemptsUI()
    {
        // Update text
        if (attemptsText != null)
            attemptsText.text = $"Examinations: {remainingAttempts}/{maxAttempts}";

        // Update icons
        if (attemptsIcons != null)
        {
            for (int i = 0; i < attemptsIcons.Length; i++)
            {
                if (attemptsIcons[i] != null)
                {
                    if (i < remainingAttempts)
                        attemptsIcons[i].color = new Color(1f, 0.8f, 0.2f); // Golden
                    else
                        attemptsIcons[i].color = new Color(0.3f, 0.3f, 0.3f); // Gray
                }
            }
        }
    }

    private void ShowOutOfAttemptsPopup()
    {
        if (uiManager == null) return;

        int examined = maxAttempts - remainingAttempts;

        uiManager.ShowPopup(
            title: "Examinations Complete",
            body: $"You've used all {examined}/{maxAttempts} examinations for this location tonight.\n\nProceed to end the night?",
            leftButtonText: "Cancel",
            rightButtonText: "End Night",
            onLeftClick: () => uiManager.ClosePopup(),
            onRightClick: () => EndNightAndLeave()
        );
    }

    public void OnLeaveLocationRequested()
    {
        if (uiManager == null) return;
        Debug.Log("Leave location requested.");

        int examined = maxAttempts - remainingAttempts;

        string message = $"You have used {examined}/{maxAttempts} examinations at the {locationName}.\n\nProceed to end the night?";

        uiManager.ShowPopup(
            title: "End Night?",
            body: message,
            leftButtonText: "Cancel",
            rightButtonText: "End Night",
            onLeftClick: () => uiManager.ClosePopup(),
            onRightClick: () => EndNightAndLeave()
        );
    }

    private void EndNightAndLeave()
    {
        Debug.Log("EndNightAndLeave called!");

        // End the night in GameProgressManager (it handles scene loading)
        if (GameProgressManager.Instance != null)
        {
            Debug.Log($"Current night BEFORE EndNight: {GameProgressManager.Instance.currentNight}");
            GameProgressManager.Instance.EndNight();
            Debug.Log($"Current night AFTER EndNight: {GameProgressManager.Instance.currentNight}");
            // EndNight() now handles loading NightScene or DeductionScene
        }
        else
        {
            Debug.LogError("GameProgressManager.Instance is NULL!");
            // Fallback: go to NightScene
            SceneManager.LoadScene("NightScene");
        }
    }

    public int GetRemainingAttempts()
    {
        return remainingAttempts;
    }
}
