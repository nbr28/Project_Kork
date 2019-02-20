﻿using Assets.Scripts.DataBase.Interfaces;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase.Handlers
{

 
    public class GenericCRUD<T>
         where T : ICreateFormData //ISerializable might cause issues
    {
        const int sleepTime = 5;
        const double threshold = 0.75;

        /// <summary>
        /// Get Request.
        /// </summary>
        /// <returns>The returned String</returns>
        public virtual string Get(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (!www.isDone) if (www.downloadProgress < threshold) Thread.Sleep(sleepTime);
            return www.downloadHandler.text;
        }

        /// <summary>
        /// Posts a type T to the given url with the form data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <returns>The database return</returns>
        public virtual string Post(string url, ICreateFormData formData)
        {
            UnityWebRequest www = UnityWebRequest.Put(url, JsonUtility.ToJson(formData));
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            www.SendWebRequest();
            while (!www.isDone) if (www.downloadProgress < threshold) Thread.Sleep(sleepTime);
            return www.downloadHandler.text;
        }

        /// <summary>
        /// This might not work
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual string Put(string url, ICreateFormData obj)
        {
            UnityWebRequest www = UnityWebRequest.Put(url, JsonUtility.ToJson(obj));
            www.method = "PUT";
            www.SetRequestHeader("Content-Type", "application/json");
            www.SendWebRequest();
            while (!www.isDone) if (www.downloadProgress < threshold) Thread.Sleep(sleepTime);
            return www.downloadHandler.text;
        }


        /// <summary>
        /// This might not work
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Delete(string url, ICreateFormData obj)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, obj.CreateForm());
            www.method = "DELETE";
            www.SendWebRequest();
            while (!www.isDone) if (www.downloadProgress < threshold) Thread.Sleep(sleepTime);
            return Int32.Parse(www.downloadHandler.text);
        }
    }
}
