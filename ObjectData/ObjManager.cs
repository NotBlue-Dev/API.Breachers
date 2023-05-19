using API.DTOS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace API.ObjectData
{
    internal class ObjManager
    {
        private int UpdateCount = 0;
        private int everyXUpdate = 1000;
        public void GetAllObjects()
        {
            if(UpdateCount > everyXUpdate)
            {
                List<BaseObj> classList = new List<BaseObj>
                {
                    new NetGameDataObj()
                };

                foreach (BaseObj item in classList)
                {
                    item.GetObjects();
                }
                UpdateCount = 0;
            }
            UpdateCount++;
        }
    }
}
