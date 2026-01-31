using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class LocationSelectionManager : MonoBehaviour 
{
    [Header("UI References")]
    public TextMeshProUGUI nightDisplay;
    public Button notesButton;

    [Header("Location Buttons")]
    public LocationButton[] locationButtons;

    [Header("Location Data")]
    public List<LocationData> locations = new List<LocationData>();

    private void Start()
    {
        InitializeLocations();
        UpdateLocationButtons(); 

        if (notesButton != null)
        {
            notesButton.onClick.AddListener(OpenNotes);
        }
    }

    private void InitializeLocations()
    {

    }

    private void UpdateNightDisplay()
    {
        if (nightDisplay != null && GameProgressManager.Instance != null)
        {
            nightDisplay.text = $"Night {GameProgressManager.Instance.currentNight}";
        }
    }

    private void UpdateLocationButtons()
    {
        for(int i = 0; i < locationButtons.Length && i < locations.Count; i++)
        {
            locationButtons[i].SetUpLocation(locations[i], OnLocationSelected);
        }
    }

    public void OnLocationSelected(LocationData location)
    {
        Debug.Log($"Loading scene: {location.sceneName}");

        SceneManager.LoadScene(location.sceneName);
    }

    private void OpenNotes()
    {
        Debug.Log("Opening notes");
        // TODO: Implement Notes 
    }
}