using Assets.Scripts.DataBase.Handlers;
using Assets.Scripts.DataBase.Interfaces;
using Proyecto26;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DataBase
{
    public class YarnLineHandler : GenericCRUD<YarnLine>
    {
        [Obsolete]
        private RequestHelper currentRequest;
        [Obsolete]
        private YarnLine[] yarnLines;
        [Obsolete]
        private YarnLine yarnLine;
        private const string baseTable = "YarnLine";



        /// <summary>
        /// Gets all the yarn lines in the table
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestAllYarnLines()
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + "s"));
        }

        /// <summary>
        /// Get all yarn lines of a single yarn off its id
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestSingleYarnById(int yarnId)
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + yarnId));
        }

        /// <summary>
        /// Gets a all yarn lines for a single snippet 
        /// </summary>
        /// <returns></returns>
        public YarnLine[] GetRequestYarnsBySnippetId(int snippetId)
        {
            return JsonHelper.getJsonArray<YarnLine>(base.Get(URLConfig.BASEURL + baseTable + "/snippetId/" + snippetId));
        }


        public YarnLine Post(IBaseConverter<YarnLine> formData)
        {
            return this.Post(formData.GetBaseInterFace());
        }


        public YarnLine Post(ICreateFormData formData)
        {
            return JsonUtility.FromJson<YarnLine>(base.Post(URLConfig.BASEURL + baseTable, formData));
        }


        public YarnLineUpdate Put(IBaseConverter<YarnLineUpdate> formData)
        {
            return this.Put(formData.GetBaseInterFace());
        }

        public YarnLineUpdate Put(ICreateFormData obj)
        {
            return JsonUtility.FromJson<YarnLineUpdate>(base.Put(URLConfig.BASEURL + baseTable, obj));
        }

        public int Delete(ICreateFormData obj)
        {
            return base.Delete(URLConfig.BASEURL + baseTable, obj);
        }
    }
}
