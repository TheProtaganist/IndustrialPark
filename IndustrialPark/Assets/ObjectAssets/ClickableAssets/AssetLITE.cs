﻿using HipHopFile;
using SharpDX;
using System.ComponentModel;

namespace IndustrialPark
{
    public class AssetLITE : BaseAsset, IRenderableAsset, IClickableAsset
    {
        private Matrix world;
        private BoundingBox boundingBox;

        public static bool dontRender = false;

        public AssetLITE(Section_AHDR AHDR, Game game, Platform platform) : base(AHDR, game, platform)
        {
            _position = new Vector3(ReadFloat(0x30), ReadFloat(0x34), ReadFloat(0x38));
            CreateTransformMatrix();
            ArchiveEditorFunctions.renderableAssets.Add(this);
        }

        public void CreateTransformMatrix()
        {
            world = Matrix.Translation(_position);
            CreateBoundingBox();
        }

        protected void CreateBoundingBox()
        {
            var vertices = new Vector3[SharpRenderer.cubeVertices.Count];

            for (int i = 0; i < SharpRenderer.cubeVertices.Count; i++)
                vertices[i] = (Vector3)Vector3.Transform(SharpRenderer.cubeVertices[i] * 0.5f, world);

            boundingBox = BoundingBox.FromPoints(vertices);
        }

        public float? GetIntersectionPosition(SharpRenderer renderer, Ray ray)
        {
            if (!ShouldDraw(renderer))
                return null;

            if (ray.Intersects(ref boundingBox, out float distance))
                return distance;
            return null;
        }
        
        public bool ShouldDraw(SharpRenderer renderer)
        {
            if (isSelected)
                return true;
            if (dontRender)
                return false;
            if (isInvisible)
                return false;

            if (AssetMODL.renderBasedOnLodt)
            {
                if (GetDistanceFrom(renderer.Camera.Position) < SharpRenderer.DefaultLODTDistance)
                    return renderer.frustum.Intersects(ref boundingBox);
                return false;
            }

            return renderer.frustum.Intersects(ref boundingBox);
        }

        public void Draw(SharpRenderer renderer)
        {
            renderer.DrawCube(world, isSelected);
        }

        [Browsable(false)]
        public bool SpecialBlendMode => true;

        protected override int EventStartOffset => 0x44;
        
        [Category("Light")]
        public byte UnknownByte08
        {
            get => ReadByte(0x8);
            set => Write(0x8, value);
        }

        [Category("Light")]
        public byte UnknownByte09
        {
            get => ReadByte(0x9);
            set => Write(0x9, value);
        }

        [Category("Light")]
        public byte UnknownByte0A
        {
            get => ReadByte(0xA);
            set => Write(0xA, value);
        }

        [Category("Light")]
        public byte UnknownByte0B
        {
            get => ReadByte(0xB);
            set => Write(0xB, value);
        }

        [Category("Light")]
        public int UnknownInt0C
        {
            get => ReadInt(0xC);
            set => Write(0xC, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat10
        {
            get => ReadFloat(0x10);
            set => Write(0x10, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat14
        {
            get => ReadFloat(0x14);
            set => Write(0x14, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat18
        {
            get => ReadFloat(0x18);
            set => Write(0x18, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat1C
        {
            get => ReadFloat(0x1C);
            set => Write(0x1C, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat20
        {
            get => ReadFloat(0x20);
            set => Write(0x20, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat24
        {
            get => ReadFloat(0x24);
            set => Write(0x24, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat28
        {
            get => ReadFloat(0x28);
            set => Write(0x28, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat2C
        {
            get => ReadFloat(0x2C);
            set => Write(0x2C, value);
        }

        private Vector3 _position;
        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float PositionX
        {
            get { return _position.X; }
            set
            {
                _position.X = value;
                Write(0x30, _position.X);
                CreateTransformMatrix();
            }
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float PositionY
        {
            get { return _position.Y; }
            set
            {
                _position.Y = value;
                Write(0x34, _position.Y);
                CreateTransformMatrix();
            }
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float PositionZ
        {
            get { return _position.Z; }
            set
            {
                _position.Z = value;
                Write(0x38, _position.Z);
                CreateTransformMatrix();
            }
        }
        
        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat3C
        {
            get => ReadFloat(0x3C);
            set => Write(0x3C, value);
        }

        [Category("Light"), TypeConverter(typeof(FloatTypeConverter))]
        public float UnknownFloat40
        {
            get => ReadFloat(0x40);
            set => Write(0x40, value);
        }
        
        public BoundingBox GetBoundingBox()
        {
            return boundingBox;
        }

        public float GetDistanceFrom(Vector3 cameraPosition)
        {
            return Vector3.Distance(cameraPosition, _position);
        }
    }
}