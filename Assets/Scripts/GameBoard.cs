using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //assigns a random color
        gameObject.GetComponent<Image>().color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        //randomImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void randomImage()
    //{
    //    StartCoroutine(setImage("http://lorempixel.com/400/200")); //balanced parens CAS
    //}

    //IEnumerator setImage(string url)
    //{
      

    //    WWW www = new WWW(url);
    //    while (!www.isDone) ;

    //    // calling this function with StartCoroutine solves the problem
    //    Debug.Log("Why on earh is this never called?");


    //    gameObject.GetComponent<Image>().material.mainTexture= www.texture;
    //    www.Dispose();
    //    www = null;
    //    yield return www;
    //}
}
