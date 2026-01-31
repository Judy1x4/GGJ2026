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

        uiManager.ShowPopup(
            title: "Examinations Complete",
            body: $"You've used all {maxAttempts} examinations for this location tonight.",
            leftButtonText: null, // Hide left button
            rightButtonText: "Leave Location",
            onLeftClick: null,
            onRightClick: () => ShowEndNightSummary()
        );
    }

    public void OnLeaveLocationRequested()
    {
        if (uiManager == null) return;
        Debug.Log("This function is triggred.");

        // Check if player has remaining attempts
        if (remainingAttempts > 0)
        {
            // Show confirmation
            string message = remainingAttempts == 1
                ? "You still have 1 examination remaining. Are you sure you want to leave?"
                : $"You still have {remainingAttempts} examinations remaining. Are you sure you want to leave?";

            uiManager.ShowPopup(
                title: "Leave Location?",
                body: message,
                leftButtonText: "Cancel",
                rightButtonText: "Yes, Leave",
                onLeftClick: () => uiManager.ClosePopup(),
                onRightClick: () => ShowEndNightSummary()
            );
        }
        else
        {
            // No attempts left, just leave
            ShowEndNightSummary();
        }
    }

    private void ShowEndNightSummary()
    {
        if (uiManager == null) return;

        int examined = maxAttempts - remainingAttempts;
        string message = $"You examined {examined}/{maxAttempts} items at the {locationName}.";

        if (GameProgressManager.Instance != null)
        {
            int nextNight = GameProgressManager.Instance.currentNight + 1;
            int totalNights = GameProgressManager.Instance.totalNights;

            if (nextNight <= totalNights)
                message += $"\n\nReady to continue to Night {nextNight}?";
            else
                message += "\n\nThis was the final night!";
        }

        uiManager.ShowPopup(
            title: "Night Investigation Complete",
            body: message,
            leftButtonText: "Review Notes",
            rightButtonText: "End Night",
            onLeftClick: () => OnReviewNotes(),
            onRightClick: () => OnContinueToNextNight()
        );
    }

    private void OnReviewNotes()
    {
        Debug.Log("Opening Notes/Journal...");
        // TODO: Open notes UI
        // For now, just close popup
        uiManager.ClosePopup();
    }

    private void OnContinueToNextNight()
    {
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.EndNight();

            if (GameProgressManager.Instance.IsGameComplete())
            {
                Debug.Log("All nights complete! Time for final deduction.");
                // SceneManager.LoadScene("FinalDeductionScene");
            }
            else
            {
                SceneManager.LoadScene("LocationSelectionScene");
            }
        }
    }

    public int GetRemainingAttempts()
    {
        return remainingAttempts;
    }
}
