using Assets.Scripts.DataBase;
using Assets.Scripts.DataBase.Handlers;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    // Set the prefab in Editor!!! 
    public GameObject galleryItemPrefab;

    private Board[] boardArr
    {
        get
        {
            BoardHandler boardHandler = new BoardHandler();
            return boardHandler.GetAllBoards();
        }
    }

    private void createGallery()
    {
        foreach (Board board in boardArr)
        {
            GameObject instantiatedBoard = Instantiate(galleryItemPrefab) as GameObject;
            
            instantiatedBoard.name = board.Board_Id.ToString();
            instantiatedBoard.GetComponentInChildren<Text>().text = board.Board_Name;

            instantiatedBoard.transform.parent = GameObject.Find("Canvas").transform;
            // TODO: make it into a nice grid -> check GridLayout
            instantiatedBoard.transform.localPosition = new Vector3(-360 + board.Board_Id % 3 * 180, 0, 0);

        }
        
        // TODO: plus sign for @Natan
    }
    private void Awake()
    {
        createGallery();
    }
}
