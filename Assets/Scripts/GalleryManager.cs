using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    // Set the prefab in Editor!!! 
    public GameObject galleryItemPrefab;

    // Dictionary of snippet GameObjects with its SnippetState ID as the key
    public Dictionary<int, GameObject> boardObjectDict;

    // TODO: connect to @Natan's calls
    //Assets.Scripts.DataBase.Board[] boardArr = boardHandler.GetAllBoards();
    private int[] boardArr = { 1, 2, 3 };

    private void createGallery()
    {
        boardObjectDict = new Dictionary<int, GameObject>();

        // TODO: connect to @Natan's calls
        //foreach(Assets.Scripts.DataBase.Board board in boardArr)
        foreach (int board in boardArr)
        {
            GameObject instantiatedBoard = Instantiate(galleryItemPrefab) as GameObject;
            instantiatedBoard.transform.parent = GameObject.Find("Canvas").transform;

            // TODO: make it into a nice grid -> check GridLayout
            instantiatedBoard.transform.localPosition = new Vector3(-540 + board * 180, 0, 0);

            // Debug.Log(instantiatedBoard.transform.localPosition);

            // TODO: create Board's state
            // BoardState instBoardState = instantiatedBoard.GetComponent<BoardState>();
            // instBoardState.loadState(board.Id);

            // names each Tile in gallery by it's title
            // instantiatedBoard.name = instBoardState.title;

            //boardObjectDict.Add(instBoardState.Id, instantiatedBoard);

            instantiatedBoard.name = "Item " + board;
            instantiatedBoard.GetComponentInChildren<Text>().text = "Item " + board;
            boardObjectDict.Add(board, instantiatedBoard);
        }
    }
    private void Awake()
    {
        createGallery();
    }
}
