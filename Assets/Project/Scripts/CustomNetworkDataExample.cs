using System;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    // Must be value type and must implement INetworkSerializable
    public struct CustomNetworkDataExample : INetworkSerializable
    {
        public int IntValue;
        public bool BoolValue;
        public float FloatValue;
        public string StringValue;
        public Vector3 VectorValue;

        public CustomNetworkDataExample(bool randomize)
        {
            if (randomize)
            {
                DateTime dateTime = System.DateTime.Now;
                IntValue = dateTime.Millisecond;
                BoolValue = dateTime.Millisecond % 2 == 0;
                FloatValue = (float)dateTime.Second / dateTime.Millisecond;
                StringValue = dateTime.ToLongDateString();
                VectorValue = new(dateTime.Minute, dateTime.Second, dateTime.Millisecond);
                return;
            }

            IntValue = default;
            BoolValue = default;
            FloatValue = default;
            StringValue = default;
            VectorValue = default;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref IntValue);
            serializer.SerializeValue(ref BoolValue);
            serializer.SerializeValue(ref FloatValue);
            serializer.SerializeValue(ref StringValue);
            serializer.SerializeValue(ref VectorValue);
        }

        public override string ToString()
        {
            return $"{IntValue}; {BoolValue}; {FloatValue}; {StringValue}; {VectorValue}";
        }
    }
}
