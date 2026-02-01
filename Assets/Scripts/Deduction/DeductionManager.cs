using UnityEngine;
using UnityEngine.Events;

public class DeductionManager : MonoBehaviour
{
    public static DeductionManager Instance { get; private set; }

    [Header("Correct Solution")]
    [SerializeField] private Suspect correctMurderer = Suspect.Elephant;
    [SerializeField] private CrimeScene correctScene = CrimeScene.Sabana;
    [SerializeField] private Weapon correctWeapon = Weapon.Candlestick;

    [Header("Player Selections")]
    private Suspect? selectedMurderer;
    private CrimeScene? selectedScene;
    private Weapon? selectedWeapon;

    [Header("Events")]
    public UnityEvent OnCorrectSolution;
    public UnityEvent OnIncorrectSolution;
    public UnityEvent<string> OnSolutionSubmitted;

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

    public void SelectMurderer(int index)
    {
        selectedMurderer = (Suspect)index;
        Debug.Log($"Selected murderer: {selectedMurderer}");
    }

    public void SelectCrimeScene(int index)
    {
        selectedScene = (CrimeScene)index;
        Debug.Log($"Selected crime scene: {selectedScene}");
    }

    public void SelectWeapon(int index)
    {
        selectedWeapon = (Weapon)index;
        Debug.Log($"Selected weapon: {selectedWeapon}");
    }

    public bool CanSubmit()
    {
        return selectedMurderer.HasValue && selectedScene.HasValue && selectedWeapon.HasValue;
    }

    public void SubmitSolution()
    {
        if (!CanSubmit())
        {
            Debug.LogWarning("Cannot submit: Not all selections have been made.");
            return;
        }

        string solutionText = $"The victim was killed with {selectedWeapon} in {selectedScene} by {selectedMurderer}.";
        OnSolutionSubmitted?.Invoke(solutionText);

        bool isCorrect = CheckSolution();

        if (isCorrect)
        {
            Debug.Log("Correct! You solved the case!");
            OnCorrectSolution?.Invoke();
        }
        else
        {
            Debug.Log("Incorrect. Try again!");
            OnIncorrectSolution?.Invoke();
        }
    }

    private bool CheckSolution()
    {
        return selectedMurderer == correctMurderer &&
               selectedScene == correctScene &&
               selectedWeapon == correctWeapon;
    }

    public void ResetSelections()
    {
        selectedMurderer = null;
        selectedScene = null;
        selectedWeapon = null;
    }

    public string GetCurrentSelectionText()
    {
        string murderer = selectedMurderer.HasValue ? selectedMurderer.ToString() : "???";
        string scene = selectedScene.HasValue ? selectedScene.ToString() : "???";
        string weapon = selectedWeapon.HasValue ? selectedWeapon.ToString() : "???";

        return $"The victim was killed with {weapon} in {scene} by {murderer}.";
    }
}
