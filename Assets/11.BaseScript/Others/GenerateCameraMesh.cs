using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Generte a mesh from camera view port
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GenerateCameraMesh : MonoBehaviour
{

    public Camera m_Camera;

    public Mesh m_Mesh;

    private void Start()
    {
        transform.position = new Vector3(m_Camera.transform.position.x,
                                        m_Camera.transform.position.y,
                                        0f);

        Vector3 size = m_Camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        List<Vector3> vertices = new List<Vector3>(4);
        vertices.Add(new Vector3(-size.x, size.y, 0f));
        vertices.Add(new Vector3(size.x, size.y, 0f));
        vertices.Add(new Vector3(-size.x, -size.y, 0f));
        vertices.Add(new Vector3(size.x, -size.y, 0f));

        int[] triangles = new int[]
        {
            0, 1, 3,
            0, 3, 2
        };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 1f),
            new Vector2(1, 1f),
            new Vector2(0, 0f),
            new Vector2(1, 0f)
        };

        m_Mesh = new Mesh();
        m_Mesh.SetVertices(vertices);
        m_Mesh.SetTriangles(triangles, 0);
        m_Mesh.uv = uvs;

        m_Mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = m_Mesh;
    }

}
