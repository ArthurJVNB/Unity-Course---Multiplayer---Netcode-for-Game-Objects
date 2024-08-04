using Unity.Netcode;

namespace Project
{
    public struct CustomNetworkDataAnotherExample : INetworkSerializable
    {
        public CustomNetworkDataExample CustomData;

        public CustomNetworkDataAnotherExample(bool randomize)
        {
            CustomData = new(randomize);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref CustomData);
        }

        public override string ToString()
        {
            return $"{CustomData}";
        }
    }
}
