using Assets.Scripts.DataBase;
using Assets.Scripts.DataBase.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GalleryManager : MonoBehaviour
{
    // Set the prefab in Editor!!! 
    public GameObject galleryItemPrefab;
    // Set the button in Editor!!!
    public TMP_InputField createBoardInput;

    private List<GameObject> galleryItems;

    public void createNewBoard()
    {
        BoardHandler boardHandler = new BoardHandler();
        Board board = new Board();

        board.Board_Name = createBoardInput.text;
        boardHandler.Post(board);

        createBoardInput.text = "";

        refreshGallery();
    }

    public void deleteBoard()
    {
        BoardHandler boardHandler = new BoardHandler();
        Board board = new Board();

        GameObject item = GameObject.Find(EventSystem.current.currentSelectedGameObject.gameObject.transform.parent.name);
        board.Board_Name = item.GetComponentInChildren<Text>().text;
      
        boardHandler.Delete(board);
        Destroy(item.gameObject);
        refreshGallery();
    }

    private void refreshGallery()
    {
        //Remove all boards and recreate them
        foreach (GameObject item in galleryItems)
        {
            Destroy(item.gameObject);
        }

        createGallery();
    }

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
        int i = 0;
        foreach (Board board in boardArr)
        {
            GameObject instantiatedBoard = Instantiate(galleryItemPrefab) as GameObject;

            instantiatedBoard.name = board.Board_Id.ToString();
            instantiatedBoard.GetComponentInChildren<Text>().text = board.Board_Name;
                
            instantiatedBoard.transform.SetParent(GameObject.Find("Canvas").transform);
            // TODO: make it into a nice grid -> check GridLayout
            instantiatedBoard.transform.localPosition = new Vector3(-450 + i * 180, 0, 0);
            i++;

            galleryItems.Add(instantiatedBoard);
        }
    }

    private void Awake()
    {
        galleryItems = new List<GameObject>();
        createGallery();
    }
}
