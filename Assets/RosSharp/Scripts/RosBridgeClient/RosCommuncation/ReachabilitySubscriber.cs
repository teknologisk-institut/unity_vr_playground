using System;
using System.IO;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    [Serializable]
    public struct Map
    {
        public Vector3[] points;
        public float[] ris;
    }

    [RequireComponent(typeof(RosConnector))]
    public class ReachabilitySubscriber : UnitySubscriber<MessageTypes.MapCreator.WorkSpace>
    {
        public bool IsMessageReceived { get; private set; }

        public bool writeToFile = true;
        public Map map;

        protected override void Start()
        {
            IsMessageReceived = false;

            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.MapCreator.WorkSpace message)
        {
            //if (IsMessageReceived)
            //    return;

            IsMessageReceived = true;

            map = new Map();
            map.points = new Vector3[message.WsSpheres.Length];
            map.ris = new float[message.WsSpheres.Length];

            for (int i = 0; i < message.WsSpheres.Length; i++)
            {
                map.points[i] = GetPoint(message.WsSpheres[i]).Ros2Unity();
                map.ris[i] = message.WsSpheres[i].ri;
            }

            if (writeToFile)
                Serialize();
        }

        private Vector3 GetPoint(MessageTypes.MapCreator.WsSphere message)
        {
            return new Vector3(message.point.x, message.point.y, message.point.z);
        }

        private void Serialize()
        {
            string path = @"C:\Temp";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string mapJson = JsonUtility.ToJson(map, true);
            File.WriteAllText(path + "/map.json", mapJson);
        }       
    }
}
