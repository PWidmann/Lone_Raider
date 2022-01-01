using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevPanel : MonoBehaviour
{
    public static DevPanel Instance;

    [SerializeField] Text fpsText;
    [SerializeField] Slider visibilityRangeSlider;
    [SerializeField] Text rangeSliderValue;

    private float uiUpdateTimer = 0;
    float displayFPSValue;
    float avg = 0F;
    public int currentVisibilityRadius = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Update()
    {
        avg += ((Time.deltaTime / Time.timeScale) - avg) * 0.03f;
        displayFPSValue = (int)(1F / avg); //display this value

        currentVisibilityRadius = (int)visibilityRangeSlider.value * 10;

        if (transform.gameObject.activeSelf)
        {
            if (uiUpdateTimer < 0.1f)
            {
                uiUpdateTimer += Time.deltaTime;

                rangeSliderValue.text = currentVisibilityRadius.ToString();
            }
            else
            {
                fpsText.text = displayFPSValue.ToString();
                
                
                
                
                uiUpdateTimer = 0;
            }
        }
    }
}
