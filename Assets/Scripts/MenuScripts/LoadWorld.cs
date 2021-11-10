using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadWorld : MonoBehaviour
{
    public void LoadSaveFile()
    {
        string fileName = transform.GetComponentInChildren<Text>().text;
        Debug.Log("clicked on load world button, world: " + fileName);

        GameManager.LoadWorldName = fileName;
        GameManager.LoadWorld = true;

        SceneManager.LoadScene("GameWorldScene");
    }
}
