using UnityEngine;

namespace RosSharp.RosBridgeClient
{

    public class Point32Subscriber : UnitySubscriber<MessageTypes.Geometry.Point32>
    {
        public Transform PublishedTransform;

        private float x;
        private float y;
        private float z;
        private bool isMessageReceived;

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.Point32 message)
        {
            Vector3 point = GetPoint(message).Ros2Unity();
            
            this.x = point.x;
            this.y = point.y;
            this.z = point.z;
        }
        private void ProcessMessage()
        {
            PublishedTransform.position = new Vector3(this.x, this.y, this.z);
        }

        private Vector3 GetPoint(MessageTypes.Geometry.Point32 message)
        {
            return new Vector3((float)message.x, (float)message.y, (float)message.z);
        }
    }
}