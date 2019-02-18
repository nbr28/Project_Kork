using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DataBase.Interfaces
{
    public interface ICreateFormData
    {
        List<IMultipartFormSection> CreateForm();
    }
}
