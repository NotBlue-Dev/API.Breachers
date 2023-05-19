using API.DTOS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace API.ObjectData
{
    public abstract class BaseObj : MonoBehaviour
    {
        internal static RootDTO dto = Plugin.rootDTO;
        public abstract void GetObjects();
    }
}
