using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;

namespace MoreShipUpgrades.Misc.Util
{
    public class StringContainer : INetworkSerializable
    {
        public string SomeText;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsWriter)
            {
                serializer.GetFastBufferWriter().WriteValueSafe(SomeText);
            }
            else
            {
                serializer.GetFastBufferReader().ReadValueSafe(out SomeText);
            }
        }
    }
}
