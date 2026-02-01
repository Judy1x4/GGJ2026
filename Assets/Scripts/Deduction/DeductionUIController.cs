using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeductionUIController : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown murdererDropdown;
    [SerializeField] private TMP_Dropdown crimeSceneDropdown;
    [SerializeField] private TMP_Dropdown weaponDropdown;

    [Header("UI Elements")]
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button retryButton;

    [Header("Scene Names")]
    [SerializeField] private string firstSceneName = "FirstScene";

    private void Start()
    {
        InitializeDropdowns();
        SetupListeners();
        UpdatePreview();
        
        if (resultPanel != null)
            resultPanel.SetActive(false);
            
        // Show journal button in Deduction Scene
        if (JournalButtonManager.Instance != null)
        {
            JournalButtonManager.Instance.ShowButton();
        }
    }

    private void OnDestroy()
{
    // Hide journal button when leaving Deduction Scene
    if (JournalButtonManager.Instance != null)
    {
        JournalButtonManager.Instance.HideButton();
    }
}

    private void InitializeDropdowns()
    {
        // Murderer options
        murdererDropdown.ClearOptions();
        murdererDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Select Murderer...", "Cat", "Elephant", "Parrot", "Dolphin", "Tiger"
        });

        // Crime scene options
        crimeSceneDropdown.ClearOptions();
        crimeSceneDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Select Location...", "Sabana", "Ocean", "Plaza", "Park", "Jungle"
        });

        // Weapon options
        weaponDropdown.ClearOptions();
        weaponDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Select Weapon...", "Candlestick", "Hammer", "Bow", "Axe", "Rifle"
        });
    }

    private void SetupListeners()
    {
        murdererDropdown.onValueChanged.AddListener(OnMurdererChanged);
        crimeSceneDropdown.onValueChanged.AddListener(OnCrimeSceneChanged);
        weaponDropdown.onValueChanged.AddListener(OnWeaponChanged);
        submitButton.onClick.AddListener(OnSubmitClicked);

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
        }

        // Subscribe to DeductionManager events
        if (DeductionManager.Instance != null)
        {
            DeductionManager.Instance.OnCorrectSolution.AddListener(ShowCorrectResult);
            DeductionManager.Instance.OnIncorrectSolution.AddListener(ShowIncorrectResult);
        }
    }

    private void OnMurdererChanged(int index)
    {
        if (index > 0) // Skip "Select..." option
        {
            DeductionManager.Instance?.SelectMurderer(index - 1);
        }
        UpdatePreview();
    }

    private void OnCrimeSceneChanged(int index)
    {
        if (index > 0)
        {
            DeductionManager.Instance?.SelectCrimeScene(index - 1);
        }
        UpdatePreview();
    }

    private void OnWeaponChanged(int index)
    {
        if (index > 0)
        {
            DeductionManager.Instance?.SelectWeapon(index - 1);
        }
        UpdatePreview();
    }

    private void OnSubmitClicked()
    {
        if (murdererDropdown.value == 0 || crimeSceneDropdown.value == 0 || weaponDropdown.value == 0)
        {
            ShowMessage("Please make all selections before submitting!");
            return;
        }

        DeductionManager.Instance?.SubmitSolution();
    }

    private void UpdatePreview()
    {
        if (previewText != null && DeductionManager.Instance != null)
        {
            previewText.text = DeductionManager.Instance.GetCurrentSelectionText();
        }
    }

    public void ShowCorrectResult()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);
        
        if (resultText != null)
        {
            resultText.text = "CORRECT!\nYou solved the case!";
            resultText.color = Color.green;
        }

        // Hide retry button on correct answer
        if (retryButton != null)
            retryButton.gameObject.SetActive(false);
    }

    public void ShowIncorrectResult()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);
        
        if (resultText != null)
        {
            resultText.text = "INCORRECT!\nThat's not quite right...";
            resultText.color = Color.red;
        }

        // Show retry button on incorrect answer
        if (retryButton != null)
            retryButton.gameObject.SetActive(true);
    }

    public void ShowMessage(string message)
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);
        
        if (resultText != null)
        {
            resultText.text = message;
            resultText.color = Color.yellow;
        }
    }

    public void CloseResultPanel()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    public void OnRetryClicked()
    {
            // Close and reset the journal
    if (JournalUI.Instance != null)
    {
        JournalUI.Instance.ResetJournal();
    }
        // Reset the entire game
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.ResetGame();
        }

        // Reset deduction selections
        if (DeductionManager.Instance != null)
        {
            DeductionManager.Instance.ResetSelections();
        }

        // Load the first scene
        SceneManager.LoadScene(firstSceneName);
    }

    public void ResetUI()
    {
        murdererDropdown.value = 0;
        crimeSceneDropdown.value = 0;
        weaponDropdown.value = 0;
        DeductionManager.Instance?.ResetSelections();
        UpdatePreview();
        CloseResultPanel();
    }
}