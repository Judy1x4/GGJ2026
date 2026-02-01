using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PatienceManager : MonoBehaviour
{
    public static PatienceManager Instance { get; private set; }

    [Header("Patience Settings")]
    [SerializeField] private int defaultPatience = 30;
    [SerializeField] private int maxPatience = 30;
    [SerializeField] private int correctOrderBonus = 5;
    [SerializeField] private int wrongOrderPenalty = 5;
    [SerializeField] private float decreaseInterval = 1f; // seconds

    [Header("UI References")]
    [SerializeField] private Slider patienceSlider;
    [SerializeField] private TextMeshProUGUI patienceText;
    [SerializeField] private Image patienceFillImage;

    [Header("Patience Colors")]
    [SerializeField] private Color highPatienceColor = Color.green;
    [SerializeField] private Color mediumPatienceColor = Color.yellow;
    [SerializeField] private Color lowPatienceColor = Color.red;

    [Header("Events")]
    public UnityEvent<int> OnPatienceChanged;
    public UnityEvent OnPatienceDepleted;
    public UnityEvent<int> OnBurgerPhaseComplete;

    private int currentPatience;
    private float timer;
    private bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetPatience();
        StartPatience();
    }

    private void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= decreaseInterval)
        {
            timer = 0f;
            DecreasePatience(1);
        }
    }

    public void StartPatience()
    {
        isActive = true;
        timer = 0f;
    }

    public void StopPatience()
    {
        isActive = false;
    }

    public void ResetPatience()
    {
        currentPatience = defaultPatience;
        UpdateUI();
    }

    public void OnCorrectOrder()
    {
        AddPatience(correctOrderBonus);
        Debug.Log($"Correct order! Patience +{correctOrderBonus}. Current: {currentPatience}");
    }

    public void OnWrongOrder()
    {
        DecreasePatience(wrongOrderPenalty);
        Debug.Log($"Wrong order! Patience -{wrongOrderPenalty}. Current: {currentPatience}");
    }

    public void AddPatience(int amount)
    {
        currentPatience = Mathf.Min(currentPatience + amount, maxPatience);
        OnPatienceChanged?.Invoke(currentPatience);
        UpdateUI();
    }

    public void DecreasePatience(int amount)
    {
        currentPatience = Mathf.Max(currentPatience - amount, 0);
        OnPatienceChanged?.Invoke(currentPatience);
        UpdateUI();

        if (currentPatience <= 0)
        {
            OnPatienceDepleted?.Invoke();
            // Note: No game over even if patience is 0
        }
    }

    public int GetCurrentPatience()
    {
        return currentPatience;
    }

    /// <summary>
    /// Calculate attempts for night investigation based on final patience level.
    /// 30+ = 3 attempts, 20+ = 2 attempts, otherwise = 1 attempt
    /// </summary>
    public int CalculateAttempts()
    {
        if (currentPatience >= 30)
            return 3;
        else if (currentPatience >= 20)
            return 2;
        else
            return 1;
    }

    /// <summary>
    /// Call this when all customers are served to finalize and transfer attempts.
    /// </summary>
    public void CompleteBurgerPhase()
    {
        StopPatience();
        int attempts = CalculateAttempts();

        // Transfer attempts to GameProgressManager
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.attemptsPerNight = attempts;
            Debug.Log($"Burger phase complete! Patience: {currentPatience}, Attempts for investigation: {attempts}");
        }

        OnBurgerPhaseComplete?.Invoke(attempts);
    }

    private void UpdateUI()
    {
        // Update slider
        if (patienceSlider != null)
        {
            patienceSlider.maxValue = maxPatience;
            patienceSlider.value = currentPatience;
        }

        // Update text
        if (patienceText != null)
        {
            patienceText.text = $"Patience: {currentPatience}";
        }

        // Update fill color based on patience level
        if (patienceFillImage != null)
        {
            if (currentPatience >= 20)
                patienceFillImage.color = highPatienceColor;
            else if (currentPatience >= 10)
                patienceFillImage.color = mediumPatienceColor;
            else
                patienceFillImage.color = lowPatienceColor;
        }
    }

    // Helper method to get attempts preview text
    public string GetAttemptsPreviewText()
    {
        int attempts = CalculateAttempts();
        return $"Investigation Attempts: {attempts}";
    }
}
