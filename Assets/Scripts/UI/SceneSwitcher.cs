using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Assets.Scripts.DataBase;

public class SceneSwitcher : MonoBehaviour {

    public void loadScene(string toLoad)
    {
        if (SceneManager.GetActiveScene().name == "BoardBrowser")
        {
            URLConfig.TempBoardId = int.Parse(EventSystem.current.currentSelectedGameObject.gameObject.name);
        }

        SceneManager.LoadScene(toLoad);
    }
}
