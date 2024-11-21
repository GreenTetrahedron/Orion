namespace Orion.Transport.ConnectionServices
{
    public struct MessageBytes
    {
        public byte[] Data;
        public int DataByteLength;

        public MessageBytes(byte[] data, int dataByteLength)
        {
            Data = data;
            DataByteLength = dataByteLength;
        }
    }
}
