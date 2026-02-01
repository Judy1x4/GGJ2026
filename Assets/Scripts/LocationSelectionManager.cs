using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class LocationSelectionManager : MonoBehaviour 
{
    [Header("UI References")]
    public TextMeshProUGUI nightDisplay;
    public TextMeshProUGUI nightsRemainingText;

    [Header("Location Buttons")]
    public LocationButton[] locationButtons;

    [Header("Location Data")]
    public List<LocationData> locations = new List<LocationData>();

    [Header("Deduction")]
    [SerializeField] private Button goToDeductionButton;
    [SerializeField] private string deductionSceneName = "DeductionScene";

    private void Start()
    {
        InitializeLocations();
        UpdateLocationButtons();
        UpdateNightDisplay();

        if (JournalButtonManager.Instance != null)
        {
            JournalButtonManager.Instance.ShowButton(); 
        }

        // Setup deduction button
        if (goToDeductionButton != null)
        {
            goToDeductionButton.onClick.AddListener(GoToDeductionScene);
            UpdateDeductionButtonVisibility();
        }
    }

    private void OnDestroy()
    {
        if (JournalButtonManager.Instance != null)
        {
            JournalButtonManager.Instance.HideButton(); 
        }
    }

    private void InitializeLocations()
    {

    }

    private void UpdateNightDisplay()
    {
        if (GameProgressManager.Instance == null) return;

        if (nightDisplay != null)
        {
            nightDisplay.text = $"Night {GameProgressManager.Instance.currentNight}";
        }

        if (nightsRemainingText != null)
        {
            int remaining = GameProgressManager.Instance.totalNights - GameProgressManager.Instance.currentNight + 1;
            nightsRemainingText.text = $"{remaining} night(s) remaining";
        }
    }

    private void UpdateLocationButtons()
    {
        for(int i = 0; i < locationButtons.Length && i < locations.Count; i++)
        {
            locationButtons[i].SetUpLocation(locations[i], OnLocationSelected);
        }
    }

    private void UpdateDeductionButtonVisibility()
    {
        if (goToDeductionButton == null) return;

        // Always show deduction button when all nights are complete
        // Also show on final night so player can choose to go early
        if (GameProgressManager.Instance != null)
        {
            bool showButton = GameProgressManager.Instance.IsFinalNight() || 
                              GameProgressManager.Instance.IsGameComplete();
            goToDeductionButton.gameObject.SetActive(showButton);
        }
    }

    public void OnLocationSelected(LocationData location)
    {
        Debug.Log($"Loading scene: {location.sceneName}");

        // Track that we're starting investigation at this location
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.StartNightInvestigation(location.locationDescription);
        }

        SceneManager.LoadScene(location.sceneName);
    }

    public void GoToDeductionScene()
    {
        Debug.Log("Going to deduction scene...");
        SceneManager.LoadScene(deductionSceneName);
    }

    // Call this when returning from a location investigation
    public void OnReturnFromInvestigation()
    {
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.EndNight();
            UpdateNightDisplay();
            UpdateDeductionButtonVisibility();
        }
    }
};