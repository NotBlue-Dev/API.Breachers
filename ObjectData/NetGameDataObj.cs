using Breachers;
using Breachers.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity;

namespace API.ObjectData
{
    public class NetGameDataObj : BaseObj
    {
        //exemple
        public override void GetObjects()
        {
            NetGameData[] netGameDatas = FindObjectsOfType<NetGameData>();
            
            foreach (NetGameData netGameData in netGameDatas)
            {
                //dto.ServerName = netGameData.NET_ServerName.ToString();
                //dto.ServerOwner = netGameData.NET_ServerOwner.ToString();
                //dto.SpectatorCount = netGameData.NET_SpectatorCount.ToString();
            }
        }
    }
}
