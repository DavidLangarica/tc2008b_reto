/// <summary>
/// The SpeedDropdownHandler class is responsible for handling the speed dropdown.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedDropdownHandler : MonoBehaviour
{
    private GameManager gameManager;
    private TMPro.TMP_Dropdown dropdown;    

    /// <summary>
    /// The Start method is called before the first frame update.
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InitializeDropdown();
    }

    /// <summary>
    /// The InitializeDropdown method is responsible for initializing the speed dropdown.
    /// </summary>
    void InitializeDropdown()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();

        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("1x");
        items.Add("1.5x");
        items.Add("2x");

        dropdown.AddOptions(items);
        dropdown.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    /// <summary>
    /// The OnValueChanged method is responsible for handling the speed dropdown value change.
    /// </summary>
    void OnValueChanged()
    {
        string value = dropdown.options[dropdown.value].text;
        float speedUpdateTime = 1f;
        float foodUpdateTime = 5f;

        switch (value)
        {
            case "1x":
                speedUpdateTime = 1f;
                foodUpdateTime = 5f;
                break;
            case "1.5x":
                speedUpdateTime = 0.5f;
                foodUpdateTime = 2.5f;
                break;
            case "2x":
                speedUpdateTime = 0.1f;
                foodUpdateTime = 0.5f;
                break;
        }

        gameManager.UpdateTimes(speedUpdateTime, foodUpdateTime);
    }
}
