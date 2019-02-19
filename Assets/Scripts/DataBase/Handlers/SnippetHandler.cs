using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase
{
    public class SnippetHandler : GenericCRUD<Snippet>
    {
        private const string baseTable = "snippet";

        public new string Delete(string url, ICreateFormData obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all the snippets for a given board
        /// </summary>
        /// <param name="board">if board is set to -1 then it will grab the current board in TempBoard</param>
        /// <returns>the array</returns>
        public Snippet[] GetAllSnippets(int board = -1)
        {
            return JsonHelper.getJsonArray<Snippet>(base.Get(URLConfig.BASEURL + baseTable + "s/" + (board == -1 ? URLConfig.TempBoardId : board)));
        }

        /// <summary>
        /// Overridden post to allow for base conversion
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public Snippet Post(IBaseConverter<Snippet> formData)
        {
            return this.Post(formData.GetBaseInterFace());
        }


        public Snippet Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<Snippet>(base.Post(URLConfig.BASEURL + baseTable, formData)); 
        }

        /// <summary>
        /// Overridden put to allow for base conversion
        /// </summary>
        /// <param name="formData">data to be uploaded</param>
        /// <returns></returns>
        public Snippet Put(IBaseConverter<Snippet> formData)
        {
            return this.Put(formData.GetBaseInterFace());
        }

        public Snippet Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<Snippet>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }





        ///// <summary>
        ///// Getter for all snippets.
        ///// </summary>
        ///// <returns>list of snippets</returns>
        //public Snippet[] GetAllSnippets()
        //{
        //    UnityWebRequest www = UnityWebRequest.Get(URLConfig.BASEURL + baseTable + "s/"+URLConfig.TempBoardId);
        //    www.SendWebRequest();
        //    while (!www.isDone)if(www.downloadProgress<0.75)Thread.Sleep(5);

        //    return JsonHelper.getJsonArray<Snippet>(www.downloadHandler.text);
        //}

        ///// <summary>
        ///// Getter for single snippet with given id.
        ///// </summary>
        ///// <param name="snippetId"></param>
        ///// <returns>single snippet</returns>
        //public Snippet GetSnippet(int snippetId)
        //{
        //    UnityWebRequest www = UnityWebRequest.Get(URLConfig.BASEURL + baseTable + "/" + snippetId);
        //    www.SendWebRequest();
        //    while (!www.isDone) if (www.downloadProgress < 0.75) Thread.Sleep(5);
        //    return JsonUtility.FromJson<Snippet>(www.downloadHandler.text);
        //}

        //public Snippet PostSnippet(IBaseConverter<Snippet> baseConverter)
        //{
        //    UnityWebRequest www = UnityWebRequest.Post(URLConfig.BASEURL + baseTable, baseConverter.GetBaseInterFace().CreateForm());
        //    www.SendWebRequest();
        //    while (!www.isDone) if (www.downloadProgress < 0.75) Thread.Sleep(5);
        //    return JsonUtility.FromJson<Snippet>(www.downloadHandler.text); ;
        //}
    }
}
