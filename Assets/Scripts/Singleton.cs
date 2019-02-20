using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    public static Singleton Instance
    {
        get
        {
            return instance;
        }
    }

    public int boardId;

    void Start()
    {
        // testing a singleton use commented lines on the other scene
        // Debug.Log("Board id is " + GameObject.Find("Singleton").GetComponent<Singleton>().boardId);
        // GameObject.Find("Singleton").GetComponent<Singleton>().boardId = 20;
        // Debug.Log("Board id is " + GameObject.Find("Singleton").GetComponent<Singleton>().boardId);
        // end of the test
        boardId = 0;
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
