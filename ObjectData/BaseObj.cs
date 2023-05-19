using API.DTOS;
using UnityEngine;

namespace API.ObjectData {
    public abstract class BaseObj : MonoBehaviour {
        internal static RootDTO dto = Plugin.RootDTO;
        public abstract void GetObjects();
    }
}
