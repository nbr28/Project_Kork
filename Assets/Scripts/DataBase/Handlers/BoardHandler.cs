using Assets.Scripts.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataBase.Handlers
{
    class BoardHandler:GenericCRUD<Board>
    {
        const string baseTable = "board";

        /// <summary>
        /// Deletes a given Board
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int Delete(string url, ICreateFormData obj)
        {
            return base.Delete(URLConfig.BASEURL + baseTable, obj);
        }

        /// <summary>
        /// Gets all the boards for a given board
        /// </summary>
        /// <param name="board">if board is set to -1 then it will grab the current board in TempBoard</param>
        /// <returns>the array</returns>
        public Board[] GetAllBoards()
        {
            return JsonHelper.getJsonArray<Board>(base.Get(URLConfig.BASEURL + baseTable +"s"));
        }

        /// <summary>
        /// Gets all the boards for a given board
        /// </summary>
        /// <param name="board">if board is set to -1 then it will grab the current board in TempBoard</param>
        /// <returns>the array</returns>
        public Board GetBoards(int boardId)
        {
            return JsonUtility.FromJson<Board>(base.Get(URLConfig.BASEURL + baseTable + "/" + boardId));
        }

        /// <summary>
        /// Overridden post to allow for base conversion
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public Board Post(IBaseConverter<Board> formData)
        {
            return this.Post(formData.GetBaseInterFace());
        }


        public Board Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<Board>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }

        /// <summary>
        /// Overridden put to allow for base conversion
        /// </summary>
        /// <param name="formData">data to be uploaded</param>
        /// <returns></returns>
        public Board Put(IBaseConverter<Board> formData)
        {
            return this.Put(formData.GetBaseInterFace());
        }

        public Board Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<Board>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }
    }
}
