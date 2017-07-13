using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toggles
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class GenerateBulletMesh : MonoBehaviour
    {

        public float m_width;

        public float m_tailLength;
        public float m_middleLength;
        public float m_headLength;

        private MeshFilter _meshFilter;
        private Mesh _mesh;

        private Vector3[] _vertices;
        private int[] _triangle0;    // for head
        private int[] _triangle1;    // for middle
        private int[] _triangle2;    // for tail
        private Vector2[] _uvs;

        private void OnEnable()
        {
            SetInitReference();
        }

        public void GenerateBullet()
        {
            _vertices = new Vector3[]
            {
                new Vector3(0, m_tailLength + m_middleLength + m_headLength, 0f),
                new Vector3(m_width, m_tailLength + m_middleLength + m_headLength, 0f),
                new Vector3(0, m_tailLength + m_middleLength, 0f),
                new Vector3(m_width, m_tailLength + m_middleLength, 0f),
                new Vector3(0, m_tailLength, 0f),
                new Vector3(m_width, m_tailLength, 0f),
                new Vector3(0, 0, 0),
                new Vector3(m_width, 0, 0)
            };

            _triangle0 = new int[]
            {
                0, 1, 2,
                1, 3 ,2
            };

            _triangle1 = new int[]
            {
                2, 3, 4,
                3, 5, 4
            };

            _triangle2 = new int[]
            {
                4, 5, 6,
                5, 7, 6
            };

            _mesh.Clear();

            _mesh.subMeshCount = 3;
            _mesh.vertices = _vertices;
            _mesh.SetTriangles(_triangle0, 0);
            _mesh.SetTriangles(_triangle1, 1);
            _mesh.SetTriangles(_triangle2, 2);

            _meshFilter.mesh = _mesh;
        }

                       
        void SetInitReference()
        {
            _meshFilter = GetComponent<MeshFilter>();
            if (_meshFilter == null)
                _meshFilter = this.gameObject.AddComponent<MeshFilter>();

            _mesh = new Mesh();
        }

    }
}


