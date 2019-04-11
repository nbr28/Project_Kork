using Assets.Scripts.DataBase;
using Assets.Scripts.DataBase.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class GalleryManager : MonoBehaviour
{
    // Set the prefab in Editor!!! 
    public GameObject galleryItemPrefab;
    // Set the button in Editor!!!
    public TMP_InputField createBoardInput;

    private List<GameObject> galleryItems;

    private void Awake()
    {
        galleryItems = new List<GameObject>();
        createGallery();
    }

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

        refreshGallery();
    }

    private void refreshGallery()
    {
        if (galleryItems != null)
        {
            foreach (GameObject item in galleryItems)
            {
                Destroy(item.gameObject);
            }
        }

        galleryItems.Clear();

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
        foreach (Board board in boardArr)
        {
            GameObject instantiatedBoard = Instantiate(galleryItemPrefab) as GameObject;

            instantiatedBoard.transform.Find("XButton").GetComponent<Button>().onClick.AddListener(deleteBoard);
            instantiatedBoard.name = board.Board_Id.ToString();
            instantiatedBoard.GetComponentInChildren<Text>().text = board.Board_Name;
            instantiatedBoard.transform.SetParent(GameObject.Find("Content").transform);

            galleryItems.Add(instantiatedBoard);
        }
    }
}
