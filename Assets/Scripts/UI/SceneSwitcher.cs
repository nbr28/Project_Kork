using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    public void loadScene(string toLoad)
    {
        SceneManager.LoadScene(toLoad);
    }
}
