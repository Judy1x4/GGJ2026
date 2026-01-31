using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class Ingradients: MonoBehaviour, IPointerClickHandler
{
    public GameObject ingradient; // Ingradients on the table

    void Awake()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ingradient.activeSelf)
        {
            ingradient.SetActive(true);
        }
        else
        {
            ingradient.SetActive(false);
        }
    }
}
