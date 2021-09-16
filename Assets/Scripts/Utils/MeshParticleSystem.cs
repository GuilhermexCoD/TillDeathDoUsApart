using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 15000;

    //Set in the Editor using Pixel Values
    [System.Serializable]
    public struct ParticleUV_Pixels
    {
        public Vector2Int uv00Pixel;
        public Vector2Int uv11Pixel;
    }

    //Holds normalized texture UV Coordinates
    private struct UV_Coords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    private Mesh mesh;

    private Vector3[] vertices;

    [SerializeField]
    private ParticleUV_Pixels[] uvPixelArray;
    [SerializeField]
    private UV_Coords[] uvCoordsArray;

    private Vector2[] uv;
    private int[] triangles;

    private int quadIndex;
    private bool needUpdateMesh;


    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();

        vertices = new Vector3[4 * MAX_QUAD_AMOUNT];
        uv = new Vector2[4 * MAX_QUAD_AMOUNT];
        triangles = new int[6 * MAX_QUAD_AMOUNT];

        UpdateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void UpdateUV(Material material, ParticleUV_Pixels[] uvPixelArray)
    {
        GetComponent<MeshRenderer>().material = material;

        this.uvPixelArray = uvPixelArray;

        UpdateUV();
    }

    private void UpdateUV()
    {

        //Set up internal UV normalized array
        Material material = GetComponent<MeshRenderer>().material;
        Texture texture = material.mainTexture;
        int textureWidth = texture.width;
        int textureHeight = texture.height;

        List<UV_Coords> uvCoordsList = new List<UV_Coords>();
        for (int i = 0; i < uvPixelArray.Length; i++)
        {
            ParticleUV_Pixels uvPixel = uvPixelArray[i];

            var uvCoord = new UV_Coords
            {
                uv00 = new Vector2((float)uvPixel.uv00Pixel.x / textureWidth, (float)uvPixel.uv00Pixel.y / textureHeight),
                uv11 = new Vector2((float)uvPixel.uv11Pixel.x / textureWidth, (float)uvPixel.uv11Pixel.y / textureHeight)
            };

            uvCoordsList.Add(uvCoord);
        }

        uvCoordsArray = uvCoordsList.ToArray();
    }

    private void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public int AddQuad(Vector3 position, float rotation, Vector3 size, int uvIndex)
    {
        if (quadIndex >= MAX_QUAD_AMOUNT)
            return quadIndex;

        UpdateQuad(quadIndex, position, rotation, size, uvIndex);

        int currentQuadIndex = quadIndex;

        quadIndex++;

        needUpdateMesh = true;

        return currentQuadIndex;
    }

    public void UpdateQuad(int quadIndex, Vector3 position, float rotation, Vector3 quadSize, int uvIndex, bool skewed = true)
    {
        position = position - transform.position;

        //Relocate Vertices
        int vIndex = quadIndex * 4;

        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        if (skewed)
        {
            vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
            vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
            vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, +quadSize.y);
            vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, -quadSize.y);
        }
        else
        {
            vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation - 180) * quadSize;
            vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation - 270) * quadSize;
            vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation - 0) * quadSize;
            vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation - 90) * quadSize;
        }

        UV_Coords uvCoord = uvCoordsArray[uvIndex];

        //UV
        uv[vIndex0] = uvCoord.uv00;
        uv[vIndex1] = new Vector2(uvCoord.uv00.x, uvCoord.uv11.y);
        uv[vIndex2] = uvCoord.uv11;
        uv[vIndex3] = new Vector2(uvCoord.uv11.x, uvCoord.uv00.y);

        //Create Triangles
        int tIndex = quadIndex * 6;

        triangles[tIndex] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;

        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;

        needUpdateMesh = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (needUpdateMesh)
        {
            UpdateMesh();
            needUpdateMesh = false;
        }
    }
}
