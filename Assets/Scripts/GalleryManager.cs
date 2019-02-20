using Assets.Scripts.DataBase;
using Assets.Scripts.DataBase.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    // Set the prefab in Editor!!! 
    public GameObject galleryItemPrefab;
    // Set the button in Editor!!!
    public TMP_InputField createBoardInput;

    public void createNewBoard()
    {
        BoardHandler boardHandler = new BoardHandler();
        Board board = new Board();
        board.Board_Name = createBoardInput.text;
        Debug.Log(board.Board_Name);
        boardHandler.Post(board);
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

            instantiatedBoard.transform.parent = GameObject.Find("Canvas").transform;
            // TODO: make it into a nice grid -> check GridLayout
            instantiatedBoard.transform.localPosition = new Vector3(-450 + i * 180, 0, 0);
            i++;
        }
    }

    private void Awake()
    {
        createGallery();
    }
}
