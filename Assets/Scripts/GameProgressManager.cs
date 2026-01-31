using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    [Header("Game Progress")]
    public int currentNight = 1;
    public int totalNights = 7;
    public string currentLocation = "";

    [Header("Night Settings")]
    public int attemptsPerNight = 3;
    private int itemsExaminedThisNight = 0;
    public static GameProgressManager Instance {  get; private set; }

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
        currentNight++;
        Debug.Log($"Night {currentNight} complete. Moving to next night.");
    }

    public bool IsGameComplete()
    {
        return currentNight > totalNights; 
    }

    public void ResetGame()
    {
        currentNight = 1;
        itemsExaminedThisNight = 0;
    }
}
