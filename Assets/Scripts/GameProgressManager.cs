using UnityEngine;
using UnityEngine.Events;

public class GameProgressManager : MonoBehaviour
{
    [Header("Game Progress")]
    public int currentNight = 1;
    public int totalNights = 5;
    public string currentLocation = "";
    private bool hasInvestigatedThisNight = false;

    [Header("Night Settings")]
    public int attemptsPerNight = 3;
    private int itemsExaminedThisNight = 0;
    public static GameProgressManager Instance { get; private set; }

    [Header("Events")]
    public UnityEvent OnNightEnded;
    public UnityEvent OnAllNightsComplete;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void StartNightInvestigation(string locationName)
    {
        currentLocation = locationName;
        itemsExaminedThisNight = 0;
        Debug.Log($"Started Night {currentNight} at {locationName}");
    }

    public void RecordExamination()
    {
        itemsExaminedThisNight++;
    }

    public int GetItemsExaminedThisNight()
    {
        return itemsExaminedThisNight;
    }

    public void EndNight()
    {
        hasInvestigatedThisNight = true;
        currentNight++;
        Debug.Log($"Night {currentNight - 1} complete. Moving to night {currentNight}.");
        
        OnNightEnded?.Invoke();
        
        if (IsGameComplete())
        {
            Debug.Log("All nights complete! Time for deduction.");
            OnAllNightsComplete?.Invoke();
        }
    }

    public bool IsGameComplete()
    {
        return currentNight > totalNights;
    }

    public bool IsFinalNight()
    {
        return currentNight == totalNights;
    }

    public bool HasInvestigatedThisNight()
    {
        return hasInvestigatedThisNight;
    }

    public void ResetNightInvestigation()
    {
        hasInvestigatedThisNight = false;
    }

    public void ResetGame()
    {
        currentNight = 1;
        itemsExaminedThisNight = 0;
        hasInvestigatedThisNight = false;
    }
}
