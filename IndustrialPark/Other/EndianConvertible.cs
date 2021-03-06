﻿using HipHopFile;
using IndustrialPark.Models;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IndustrialPark
{
    public abstract class EndianConvertibleWithData : EndianConvertible
    {
        public abstract byte[] Data { get; set; }

        [Browsable(false)]
        public Platform platform { get; protected set; }
        [Browsable(false)]
        public Game game { get; protected set; }
        public void SetGamePlatform(Game game, Platform platform)
        {
            this.game = game;
            this.platform = platform;
            endianness = EndianConverter.PlatformEndianness(platform);
        }

        public EndianConvertibleWithData(Endianness endianness) : base(endianness) { }
        public EndianConvertibleWithData(Game game, Platform platform) : base(EndianConverter.PlatformEndianness(platform))
        {
            this.game = game;
            this.platform = platform;
        }
        
        protected float ReadFloat(int j)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToSingle(new byte[] {
                Data[j + 3],
                Data[j + 2],
                Data[j + 1],
                Data[j] }, 0);

            return BitConverter.ToSingle(Data, j);
        }

        protected byte ReadByte(int j)
        {
            return Data[j];
        }

        protected short ReadShort(int j)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToInt16(new byte[] {
                Data[j + 1],
                Data[j] }, 0);

            return BitConverter.ToInt16(Data, j);
        }

        protected ushort ReadUShort(int j)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToUInt16(new byte[] {
                Data[j + 1],
                Data[j] }, 0);

            return BitConverter.ToUInt16(Data, j);
        }

        protected int ReadInt(int j)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToInt32(new byte[] {
                Data[j + 3],
                Data[j + 2],
                Data[j + 1],
                Data[j] }, 0);

            return BitConverter.ToInt32(Data, j);
        }

        protected uint ReadUInt(int j)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToUInt32(new byte[] {
                Data[j + 3],
                Data[j + 2],
                Data[j + 1],
                Data[j] }, 0);

            return BitConverter.ToUInt32(Data, j);
        }
        
        protected virtual void Write(int j, float value)
        {
            byte[] split = BitConverter.GetBytes(value).ToArray();

            if (endianness == Endianness.Big)
                split = split.Reverse().ToArray();

            for (int i = 0; i < 4; i++)
                Data[j + i] = split[i];
        }

        protected virtual void Write(int j, byte value)
        {
            Data[j] = value;
        }

        protected virtual void Write(int j, short value)
        {
            byte[] split = BitConverter.GetBytes(value);

            if (endianness == Endianness.Big)
                split = split.Reverse().ToArray();

            for (int i = 0; i < 2; i++)
                Data[j + i] = split[i];
        }

        protected virtual void Write(int j, ushort value)
        {
            byte[] split = BitConverter.GetBytes(value);

            if (endianness == Endianness.Big)
                split = split.Reverse().ToArray();

            for (int i = 0; i < 2; i++)
                Data[j + i] = split[i];
        }

        protected virtual void Write(int j, int value)
        {
            byte[] split = BitConverter.GetBytes(value);

            if (endianness == Endianness.Big)
                split = split.Reverse().ToArray();

            for (int i = 0; i < 4; i++)
                Data[j + i] = split[i];
        }

        protected virtual void Write(int j, uint value)
        {
            byte[] split = BitConverter.GetBytes(value);

            if (endianness == Endianness.Big)
                split = split.Reverse().ToArray();

            for (int i = 0; i < 4; i++)
                Data[j + i] = split[i];
        }

        protected static uint Mask(uint bit)
        {
            return (uint)Math.Pow(2, bit);
        }

        protected static uint InvMask(uint bit)
        {
            return uint.MaxValue - Mask(bit);
        }

        protected float? TriangleIntersection(Ray r, IList<Triangle> triangles, IList<Vector3> vertices, Matrix world)
        {
            float? smallestDistance = null;

            foreach (var t in triangles)
            {
                Vector3 v1 = (Vector3)Vector3.Transform(vertices[t.vertex1], world);
                Vector3 v2 = (Vector3)Vector3.Transform(vertices[t.vertex2], world);
                Vector3 v3 = (Vector3)Vector3.Transform(vertices[t.vertex3], world);

                if (r.Intersects(ref v1, ref v2, ref v3, out float distance))
                {
                    if (smallestDistance == null || distance < smallestDistance)
                        smallestDistance = distance;
                }
            }

            return smallestDistance;
        }
    }

    public class EndianConvertible
    {
        protected Endianness endianness;

        public EndianConvertible(Endianness endianness)
        {
            this.endianness = endianness;
        }

        public float Switch(float a)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToSingle(BitConverter.GetBytes(a).Reverse().ToArray(), 0);
            return a;
        }

        public int Switch(int a)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToInt32(BitConverter.GetBytes(a).Reverse().ToArray(), 0);
            return a;
        }

        public uint Switch(uint a)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToUInt32(BitConverter.GetBytes(a).Reverse().ToArray(), 0);
            return a;
        }

        public short Switch(short a)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToInt16(BitConverter.GetBytes(a).Reverse().ToArray(), 0);
            return a;
        }

        public ushort Switch(ushort a)
        {
            if (endianness == Endianness.Big)
                return BitConverter.ToUInt16(BitConverter.GetBytes(a).Reverse().ToArray(), 0);
            return a;
        }

        public string Switch(string a)
        {
            if (endianness == Endianness.Big)
                return new string(a.Reverse().ToArray());
            return a;
        }
    }
}
