using System;
using System.Collections.Generic;

using RakNet;

using SkySaga.Game.Extensions;
using SkySaga.Game.Interfaces;

namespace SkySaga.Game.Packets.Common;

public class WalletData : ISerializableType
{
    public List<CurrencyData> CurrencyList = [];
    private const int CurrencyListDefaultCount = 8;

    public class CurrencyData : ISerializableType
    {
        public uint? NameHash;
        public int Value;

        public void Serialize(BitStream bitStream)
        {
            bitStream.WriteOptional(NameHash, (value) =>
            {
                bitStream.Write((int)value);
            });

            if (Value <= 1023)
            {
                bitStream.Write0();

                bitStream.WriteBits(BitConverter.GetBytes(Value), 32 - Util.NumBitsRequiredUInt32(1023), true);
            }
            else
            {
                bitStream.Write1();

                bitStream.WriteBits(BitConverter.GetBytes(Value), 32 - Util.NumBitsRequiredUInt32(0xFFFFFFFF), true);
            }
        }
    }

    public void Serialize(BitStream bitStream)
    {
        // Count is optimised
        if (CurrencyList.Count < CurrencyListDefaultCount)
        {
            bitStream.WriteBits(BitConverter.GetBytes(CurrencyList.Count), 32 - Util.NumBitsRequiredUInt32(CurrencyListDefaultCount), true);
        }
        else
        {
            bitStream.WriteBits(BitConverter.GetBytes(CurrencyListDefaultCount), 32 - Util.NumBitsRequiredUInt32(CurrencyListDefaultCount), true);

            bitStream.Write1();
            bitStream.Write(CurrencyList.Count);
        }

        foreach (var currency in CurrencyList)
            currency.Serialize(bitStream);
    }
}