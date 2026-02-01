using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ListenUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI listenText;
    [SerializeField] private GameObject listenBox;

    [SerializeField] private OrderUI orderUI;
        
    [SerializeField] private OrderSubmit orderSubmit;
    [SerializeField] private List<IngredientSelect> ingredientSelects = new();

    [SerializeField] private SecretUI secretUI;

    [SerializeField] private float pauseSeconds = 5f;
    private Coroutine pauseRoutine;


    void Start()
    {
        listenBox.SetActive(false);
        listenText.text = string.Empty;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnSpecialCustomer()
    {
        listenBox.SetActive(true);
        listenText.text = "Spy on\nsecret";
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        listenBox.SetActive(false);
        listenText.text = string.Empty;
        GetComponent<BoxCollider2D>().enabled = false;

        // start the 5s pause
        if (pauseRoutine != null) StopCoroutine(pauseRoutine);
        pauseRoutine = StartCoroutine(PauseIngredientSelection());

    }

    private IEnumerator PauseIngredientSelection()
    {
        // disable selecting ingredients and submission
        foreach (var ingredientSelect in ingredientSelects)
        {
            if (ingredientSelect != null)
                ingredientSelect.DisableCollider();
        }
        orderSubmit.enabled = false;
        secretUI.OnListen();
        orderUI.OnListen();

        yield return new WaitForSeconds(pauseSeconds);

        // enable selecting ingredients again
        foreach (var ingredientSelect in ingredientSelects)
        {
            if (ingredientSelect != null)
                ingredientSelect.ActivateCollider();
        }
        orderSubmit.enabled = true;
        secretUI.FinishListen();
        orderUI.FinishListen();
        pauseRoutine = null;
    }
}
