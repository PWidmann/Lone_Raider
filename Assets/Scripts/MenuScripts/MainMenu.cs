using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] InputField levelName;
    [SerializeField] GameObject loadWorldPanel;
    [SerializeField] GameObject buttonContentPanel;
    [SerializeField] GameObject worldButtonPrefab;


    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject settingsPanel;

    [Header("Settings Panel")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;

    void Start()
    {
        loadWorldPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    
    void Update()
    {
        
    }


    public void NewWorldButton()
    {
        settingsPanel.SetActive(false);

        if (levelName.text != "")
        {
            GameManager.CreateNewWorld = true;
            GameManager.NewWorldName = levelName.text;
            SceneManager.LoadScene("GameWorldScene");
        }
    }

    public void SettingsButton()
    {
        if (settingsPanel.activeSelf == true)
        {
            settingsPanel.SetActive(false);
            return;
        }
        else
        {
            settingsPanel.SetActive(true);
            loadWorldPanel.SetActive(false);
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ApplyButton()
    {
        // Screen settings
        string dropDownValue = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = dropDownValue.Split('x');
        bool fullScreen = windowDropdown.value == 0 ? false : true;

        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullScreen);
    }


    public void LoadWorldButton()
    {
        settingsPanel.SetActive(false);

        if (loadWorldPanel.activeSelf == true)
        {
            loadWorldPanel.SetActive(false);
            DeleteWorldButtons();
        }
        else
        {
            // Read the save file folder and display all save files with a button
            loadWorldPanel.SetActive(true);

            if (!Directory.Exists(Application.dataPath + "/SaveData"))
            {
                Directory.CreateDirectory(Application.dataPath + "/SaveData");
            }

            string path = Application.dataPath + "/SaveData/";
            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();

            // Instantiate a load button in the menu for every safe file
            foreach (FileInfo file in fileInfo)
            {
                if (file.Extension == ".save")
                {
                    string fileName = file.Name.Substring(0, file.Name.IndexOf('.')); // give name without extension

                    GameObject go = Instantiate(worldButtonPrefab);
                    go.transform.SetParent(buttonContentPanel.transform);
                    go.GetComponentInChildren<Text>().text = fileName;
                    LoadWorld lw = go.AddComponent<LoadWorld>();
                    go.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() => lw.LoadSaveFile()));
                }
            }
        }
    }

    private void DeleteWorldButtons()
    {
        foreach (Transform child in buttonContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
