using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject escapeMenuPanel;

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenuPanel.SetActive(!escapeMenuPanel.activeSelf);
        }
    }

    public void ApplyButton()
    {
        // Screen settings
        string dropDownValue = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = dropDownValue.Split('x');
        bool fullScreen = windowDropdown.value == 0 ? false : true;

        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullScreen);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        escapeMenuPanel.SetActive(!escapeMenuPanel.activeSelf);
    }


}
