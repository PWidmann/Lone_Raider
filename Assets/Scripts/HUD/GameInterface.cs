using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quicksavePanel;
    [SerializeField] private GameObject darkenBackgroundPanel;
    [SerializeField] private GameObject devPanel;

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;
    [SerializeField] private Toggle allObjectsToggle;

    private float quicksaveTimer;
    private bool startQuicksave = false;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        quicksavePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapPanel.activeSelf)
            {
                mapPanel.SetActive(false);
            }
            else
            {
                escapeMenuPanel.SetActive(!escapeMenuPanel.activeSelf);


                if (escapeMenuPanel.activeSelf == true)
                {
                    darkenBackgroundPanel.SetActive(true);
                }
                else
                {
                    darkenBackgroundPanel.SetActive(false);
                }

                if (settingsPanel.activeSelf == true)
                {
                    BackButton();
                }
            }
        }

        if (startQuicksave)
        {
            quicksaveTimer -= Time.deltaTime;

            if (quicksaveTimer < 0)
            {
                startQuicksave = false;
                quicksavePanel.SetActive(false);
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
        ObjectVisibility.Instance.PlayerActive = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        escapeMenuPanel.SetActive(!escapeMenuPanel.activeSelf);
        darkenBackgroundPanel.SetActive(false);
    }

    public void SettingsButton()
    {
        escapeMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        escapeMenuPanel.SetActive(true); 
    }

    public void ShowQuicksavePanel()
    {
        startQuicksave = true;
        quicksaveTimer = 2f;
        quicksavePanel.SetActive(true);
    }

    public void ShowDevWindowPanel()
    {
        settingsPanel.SetActive(false);
        devPanel.SetActive(true);
        darkenBackgroundPanel.SetActive(false);
    }

    public void CloseDevPanel()
    {
        devPanel.SetActive(false);
    }

    public void CloseWorldMapPanel()
    {
        mapPanel.SetActive(false);
    }

    public void VisibilityRangeUpateClearMap()
    {
        ObjectVisibility.Instance.DisableAllObjectsOnMap();
    }

    public void ShowAllObjectsAndPauseGameToggle()
    {
        if (allObjectsToggle.isOn)
        {
            Time.timeScale = 0;
            ObjectVisibility.Instance.EnableAllObjectsOnMap();
        }
        else
        {
            Time.timeScale = 1;
            ObjectVisibility.Instance.DisableAllObjectsOnMap();
            DevPanel.Instance.avg = 0;
        }
    }
}
