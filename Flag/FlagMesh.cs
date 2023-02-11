using UnityEngine;

public class FlagMesh : MonoBehaviour
{
    public int horizontalVertices = 10;
    public int verticalVertices = 10;

    public float width = 10f;
    public float height = 5f;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[(horizontalVertices + 1) * (verticalVertices + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[horizontalVertices * verticalVertices * 6];

        float stepX = width / horizontalVertices;
        float stepY = height / verticalVertices;

        for (int i = 0, y = 0; y <= verticalVertices; y++)
        {
            for (int x = 0; x <= horizontalVertices; x++, i++)
            {
                vertices[i] = new Vector3(x * stepX - width / 2, y * stepY - height / 2, 0);
                uv[i] = new Vector2((float)x / horizontalVertices, (float)y / verticalVertices);
            }
        }

        for (int ti = 0, vi = 0, y = 0; y < verticalVertices; y++, vi++)
        {
            for (int x = 0; x < horizontalVertices; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + horizontalVertices + 1;
                triangles[ti + 5] = vi + horizontalVertices + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}