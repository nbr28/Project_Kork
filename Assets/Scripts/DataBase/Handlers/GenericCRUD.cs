using Assets.Scripts.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
        public string Get(string url)
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
        public string Post(string url, ICreateFormData formData)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, formData.CreateForm());
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
        public string Put(string url, ICreateFormData obj)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, obj.CreateForm());
            www.method = "PUT";
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
        public string Delete(string url, ICreateFormData obj)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, obj.CreateForm());
            www.method = "DELETE";
            www.SendWebRequest();
            while (!www.isDone) if (www.downloadProgress < threshold) Thread.Sleep(sleepTime);
            return www.downloadHandler.text;
        }
    }
}
