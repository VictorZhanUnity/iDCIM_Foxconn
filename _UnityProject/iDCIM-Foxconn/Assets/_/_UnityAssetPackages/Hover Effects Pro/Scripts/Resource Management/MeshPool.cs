using UnityEngine;

namespace HoverEffectsPro
{
    public class MeshPool : Singleton<MeshPool>
    {
        private Mesh _unitCone;
        private Mesh _unitPyramid;
        private Mesh _unitBox;
        private Mesh _unitWireBox;
        private Mesh _unitCoordSystem;

        public Mesh UnitCone
        {
            get
            {
                if (_unitCone == null) _unitCone = CylinderMesh.CreateCylinder(1.0f, 0.0f, 1.0f, 30, 30, 1, 1, Color.white);
                return _unitCone;
            }
        }
        public Mesh UnitPyramid
        {
            get
            {
                if (_unitPyramid == null) _unitPyramid = PyramidMesh.CreatePyramid(Vector3.zero, 1.0f, 1.0f, 1.0f, Color.white);
                return _unitPyramid;
            }
        }
        public Mesh UnitBox
        {
            get
            {
                if (_unitBox == null) _unitBox = BoxMesh.CreateBox(1.0f, 1.0f, 1.0f, Color.white);
                return _unitBox;
            }
        }
        public Mesh UnitWireBox
        {
            get
            {
                if (_unitWireBox == null) _unitWireBox = BoxMesh.CreateWireBox(1.0f, 1.0f, 1.0f, Color.white);
                return _unitWireBox;
            }
        }
        public Mesh UnitCoordSystem
        {
            get
            {
                if (_unitCoordSystem == null) _unitCoordSystem = LineMesh.CreateCoordSystemAxesLines(1.0f, Color.white);
                return _unitCoordSystem;
            }
        }
    }
}
