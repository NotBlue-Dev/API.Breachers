using BreachersAPI.DTOS;
using UnityEngine;

namespace BreachersAPI.ObjectData {
    public abstract class BaseObj : MonoBehaviour {
        internal static RootDTO dto = Plugin.RootDTO;
        public abstract void GetObjects();
    }
}
