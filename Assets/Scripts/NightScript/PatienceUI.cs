using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatienceUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider patienceSlider;
    [SerializeField] private TextMeshProUGUI patienceText;
    [SerializeField] private TextMeshProUGUI attemptsPreviewText;
    [SerializeField] private Image sliderFill;

    [Header("Colors")]
    [SerializeField] private Color highColor = new Color(0.2f, 0.8f, 0.2f); // Green
    [SerializeField] private Color mediumColor = new Color(0.9f, 0.8f, 0.2f); // Yellow
    [SerializeField] private Color lowColor = new Color(0.9f, 0.2f, 0.2f); // Red

    [Header("Animation")]
    [SerializeField] private bool animateChanges = true;
    [SerializeField] private float animationSpeed = 5f;

    private float targetValue;
    private float displayValue;

    private void Start()
    {
        // Subscribe to patience changes
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.OnPatienceChanged.AddListener(OnPatienceChanged);

            // Initialize with current patience
            int currentPatience = PatienceManager.Instance.GetCurrentPatience();
            displayValue = currentPatience;
            targetValue = currentPatience;
            UpdateDisplay(currentPatience);
        }

        // Initialize slider
        if (patienceSlider != null)
        {
            patienceSlider.maxValue = 30;
            patienceSlider.value = 30;
        }
    }

    private void Update()
    {
        // Smooth animation for slider
        if (animateChanges && patienceSlider != null)
        {
            if (Mathf.Abs(displayValue - targetValue) > 0.01f)
            {
                displayValue = Mathf.Lerp(displayValue, targetValue, Time.deltaTime * animationSpeed);
                patienceSlider.value = displayValue;
            }
        }
    }

    private void OnPatienceChanged(int newPatience)
    {
        targetValue = newPatience;

        if (!animateChanges && patienceSlider != null)
        {
            patienceSlider.value = newPatience;
        }

        UpdateDisplay(newPatience);
    }

    private void UpdateDisplay(int patience)
    {
        // Update text
        if (patienceText != null)
        {
            patienceText.text = $"{patience}";
        }

        // Update attempts preview
        if (attemptsPreviewText != null && PatienceManager.Instance != null)
        {
            int attempts = PatienceManager.Instance.CalculateAttempts();
            attemptsPreviewText.text = $"Night Attempts: {attempts}";
        }

        // Update color
        UpdateSliderColor(patience);
    }

    private void UpdateSliderColor(int patience)
    {
        if (sliderFill == null) return;

        if (patience >= 20)
        {
            sliderFill.color = highColor;
        }
        else if (patience >= 10)
        {
            sliderFill.color = mediumColor;
        }
        else
        {
            sliderFill.color = lowColor;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (PatienceManager.Instance != null)
        {
            PatienceManager.Instance.OnPatienceChanged.RemoveListener(OnPatienceChanged);
        }
    }
}
