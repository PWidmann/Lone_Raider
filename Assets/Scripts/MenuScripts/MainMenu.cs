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

    
    void Start()
    {
        loadWorldPanel.SetActive(false);
    }

    
    void Update()
    {
        
    }


    public void NewWorldButton()
    {
        if (levelName.text != "")
        {
            GameManager.CreateNewWorld = true;
            GameManager.NewWorldName = levelName.text;
            SceneManager.LoadScene("GameWorldScene");
        }
    }


    public void LoadWorldButton()
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
