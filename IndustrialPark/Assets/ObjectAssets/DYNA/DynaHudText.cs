﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using static IndustrialPark.ConverterFunctions;

namespace IndustrialPark
{
    public class DynaHudText : DynaBase
    {
        public override string Note => "Version is always 1";

        public DynaHudText() : base()
        {
            TextboxID = 0;
            TextID = 0;
        }

        public DynaHudText(IEnumerable<byte> enumerable) : base (enumerable)
        {
            UnknownFloat1 = Switch(BitConverter.ToSingle(data, 0x0));
            UnknownFloat2 = Switch(BitConverter.ToSingle(data, 0x4));
            UnknownFloat3 = Switch(BitConverter.ToSingle(data, 0x8));
            UnknownFloat4 = Switch(BitConverter.ToSingle(data, 0xC));
            UnknownFloat5 = Switch(BitConverter.ToSingle(data, 0x10));
            UnknownFloat6 = Switch(BitConverter.ToSingle(data, 0x14));
            TextboxID = Switch(BitConverter.ToUInt32(data, 0x2C));
            TextID = Switch(BitConverter.ToUInt32(data, 0x30));
        }

        public override byte[] ToByteArray()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat1)));
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat2)));
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat3)));
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat4)));
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat5)));
            list.AddRange(BitConverter.GetBytes(Switch(UnknownFloat6)));
            list.AddRange(BitConverter.GetBytes(Switch(TextboxID)));
            list.AddRange(BitConverter.GetBytes(Switch(TextID)));

            return list.ToArray();
        }

        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat1{ get; set; }
        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat2{ get; set; }
        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat3{ get; set; }
        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat4{ get; set; }
        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat5{ get; set; }
        [TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat6{ get; set; }
        public AssetID TextboxID { get; set; }
        public AssetID TextID { get; set; }
    }
}