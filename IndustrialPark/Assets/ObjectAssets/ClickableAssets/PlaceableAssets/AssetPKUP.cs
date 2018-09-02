﻿using HipHopFile;
using SharpDX;

namespace IndustrialPark
{
    public class AssetPKUP : PlaceableAsset
    {
        public static bool dontRender = false;

        protected override bool DontRender()
        {
            return dontRender;
        }

        public AssetPKUP(Section_AHDR AHDR) : base(AHDR)
        {
        }

        public override void Setup(SharpRenderer renderer)
        {
            _pickEntryID = ReadUInt(0x54);

            base.Setup(renderer);
        }
        
        protected override float? TriangleIntersection(Ray r, float initialDistance)
        {
            if (dontRender)
                return null;

            uint _modelAssetId;
            try { _modelAssetId = AssetPICK.pick.pickEntries[_pickEntryID].unknown4; }
            catch { return initialDistance; }

            bool hasIntersected = false;
            float smallestDistance = 1000f;

            if (ArchiveEditorFunctions.renderingDictionary.ContainsKey(_modelAssetId))
            {
                RenderWareModelFile rwmf;

                if (ArchiveEditorFunctions.renderingDictionary[_modelAssetId] is AssetMINF MINF)
                {
                    if (MINF.HasRenderWareModelFile())
                        rwmf = ArchiveEditorFunctions.renderingDictionary[_modelAssetId].GetRenderWareModelFile();
                    else return initialDistance;
                }
                else rwmf = ArchiveEditorFunctions.renderingDictionary[_modelAssetId].GetRenderWareModelFile();

                foreach (RenderWareFile.Triangle t in rwmf.triangleList)
                {
                    Vector3 v1 = (Vector3)Vector3.Transform(rwmf.vertexListG[t.vertex1], world);
                    Vector3 v2 = (Vector3)Vector3.Transform(rwmf.vertexListG[t.vertex2], world);
                    Vector3 v3 = (Vector3)Vector3.Transform(rwmf.vertexListG[t.vertex3], world);

                    if (r.Intersects(ref v1, ref v2, ref v3, out float distance))
                    {
                        hasIntersected = true;

                        if (distance < smallestDistance)
                            smallestDistance = distance;
                    }
                }

                if (hasIntersected)
                    return smallestDistance;
                else return null;
            }

            return initialDistance;
        }

        public override void Draw(SharpRenderer renderer)
        {
            if (dontRender) return;

            if (AssetPICK.pick != null)
            {
                if (AssetPICK.pick.pickEntries.ContainsKey(_pickEntryID))
                {
                    if (ArchiveEditorFunctions.renderingDictionary.ContainsKey(AssetPICK.pick.pickEntries[_pickEntryID].unknown4))
                    {
                        ArchiveEditorFunctions.renderingDictionary[AssetPICK.pick.pickEntries[_pickEntryID].unknown4].Draw(renderer, world, isSelected);
                    }
                    else
                    {
                        renderer.DrawCube(world, isSelected);
                    }
                }
                else
                {
                    renderer.DrawCube(world, isSelected);
                }
            }
            else
            {
                renderer.DrawCube(world, isSelected);
            }
        }
        
        private uint _pickEntryID;
        public AssetID PickEntryID
        {
            get { return _pickEntryID; }
            set
            {
                _pickEntryID = value;
                Write(0x54, value);
            }
        }
    }
}