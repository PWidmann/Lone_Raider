using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject QuicksavePanel;

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;

    public Light2D ambientLight;

    private float quicksaveTimer;
    private bool startQuicksave = false;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        QuicksavePanel.SetActive(false);
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

        if (startQuicksave)
        {
            quicksaveTimer -= Time.deltaTime;

            if (quicksaveTimer < 0)
            {
                startQuicksave = false;
                QuicksavePanel.SetActive(false);
            } 
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

    public void ShowQuicksavePanel()
    {
        startQuicksave = true;
        quicksaveTimer = 2f;
        QuicksavePanel.SetActive(true);
    }
}
