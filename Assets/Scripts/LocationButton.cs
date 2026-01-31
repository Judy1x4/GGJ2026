using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI; 

public class LocationButton : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI locationDescription;
    public Button button;

    private LocationData locationData;
    private Action<LocationData> onClickCallback; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetUpLocation(LocationData data, Action<LocationData> callback)
    {
        locationData = data;
        onClickCallback = callback;

        if (locationDescription != null)
        {
            locationDescription.text = data.description;
        }

    }

    private void OnButtonClick()
    {
        onClickCallback?.Invoke(locationData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
