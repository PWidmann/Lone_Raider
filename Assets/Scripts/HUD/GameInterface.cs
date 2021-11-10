using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [SerializeField] GameObject inventoryPanel;

    public Light2D ambientLight;

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        
    }
}
