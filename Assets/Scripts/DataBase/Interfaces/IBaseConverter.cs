using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataBase.Interfaces
{
    interface IBaseConverter<T>
    {
        T GetBaseInterFace();
    }
}
