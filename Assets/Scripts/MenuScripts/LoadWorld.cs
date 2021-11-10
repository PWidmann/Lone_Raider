using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadWorld : MonoBehaviour
{
    public void LoadSaveFile()
    {
        string fileName = transform.GetComponentInChildren<Text>().text;

        GameManager.LoadWorldName = fileName;
        GameManager.LoadWorld = true;

        SceneManager.LoadScene("GameWorldScene");
    }
}
