using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Voxel Script
/// Spawns Voxels in terrian generation mimicking minecraft's style...
/// Ideas were from all over the place on this one, but most of it is from what I saw in our voxel ocean, and our examples
/// Also ideas are from the Example from Processing's Terrian Generation.
/// For your answer about continually spawning chunks you tie it to the camera's location X / Z 
/// the further you get away from a spawner, unloads / deletes the object and creates a new one around the player.
/// Each Voxel is 4 quads, 4 quads are far easier for rendering than a cube. I could generate the cube mesh but it is far easier to do this.
/// </summary>
public class VoxelScript : MonoBehaviour
{
    /// <summary>
    /// Current Voxel Type is the current voxel being used
    /// </summary>
    private GameObject currentVoxelType;
    /// <summary>
    /// Is an array of game objects that are pulled from the voxel
    /// </summary>
    public GameObject[] voxelColorType;
    /// <summary>
    /// Amp is the amplitude or height of the perlin noise
    /// </summary>
    [Range(5, 100)] public float amp = 10f;
    /// <summary>
    /// Is the frequency of spawning Voxels
    /// </summary>
    [Range(5, 100)] public float freq = 10f;
    /// <summary>
    /// Cols is the Columns and how many voxels are present HEIGHT
    /// </summary>
    [Range(50, 300)] public int cols = 50;
    /// <summary>
    /// Rows is the rows of voxels WIDTH
    /// </summary>
    [Range(50, 300)] public int rows = 50;
    /// <summary>
    /// Is the Position of the voxel spawner and each voxel
    /// </summary>
    private Vector3 Pos;
    /// <summary>
    /// Threshold for the Voxels
    /// </summary>
    [Range(2f, 100f)] public float thresh = 2f;
    /// <summary>
    /// Mesh on render
    /// </summary>
    MeshFilter mesh;
    /// <summary>
    /// checks to see if you want the object floored or not
    /// </summary>
    [Range(0f,1f)]public float floored = 1f;

    /// <summary>
    ///  When Project starts generates the terrian
    /// </summary>
    void Start()
    {
        generateTerrian();
    }
    /// <summary>
    /// Generates the Terrian of Voxels
    /// Calculates each Y coordinate and X coordinates and Z coords of blocks
    /// Y is not SET by cols or rows, instead is set by a perlin noise field
    /// </summary>
    public void generateTerrian()
    {
       Pos = this.transform.position;
        thresh = Perlin.Noise(1, 2, 50);
            
            for (int x = 0; x < cols; x++)
        {


            for (int z = 0; z < rows; z++)
            {

                float y = Perlin.Noise((Pos.x + x) / freq, 0, (Pos.z + z) / freq) * amp;
                Vector3 worldPosition = new Vector3(Pos.x + x, Pos.y + y, Pos.z + z);
                Vector3 noiseCoords = worldPosition / thresh;
                float density = Perlin.Noise(noiseCoords);
                density -= worldPosition.y * thresh;
                if (floored == 1)
                {
                    y = Mathf.Floor(y);
                }
                if (density > (freq / amp) * thresh) continue;
                ///This was for fun wanted to see what happens if i made comparative statements with a perlin noise added in
                if (y > x && y >= z)
                {

                    currentVoxelType = voxelColorType[0];
                    y = Perlin.Noise((Pos.x + x) * freq, (Pos.y + y), (Pos.z + z)) * freq;
                }
                if (y > amp / 2)
                    {
                    y = Perlin.Noise((Pos.x + worldPosition.x) / freq, (Pos.y + worldPosition.y) /freq, (Pos.z + worldPosition.z)) / freq;
                    currentVoxelType = voxelColorType[0];

                    }
                    else if (y < amp / 2)
                    {
                        currentVoxelType = voxelColorType[1];
                        if (y < amp / freq)
                        {
                        y = Perlin.Noise((Pos.x + worldPosition.x), (Pos.y + worldPosition.y), (Pos.z + worldPosition.z));
                        currentVoxelType = voxelColorType[2];
                        }
                    }
                
                    GameObject newVoxel =
                        GameObject.Instantiate(currentVoxelType, worldPosition, Quaternion.identity);

                    newVoxel.transform.position =
                        new Vector3(Pos.x + x, Pos.y + y, Pos.z + z);
                }
            
        }
    }

    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
    }
    /// <summary>
    /// Makes a cube with data taken from X,Y,Z coords
    /// </summary>
    /// <returns></returns>
    private Mesh MakeCube()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        // Front face
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        // Back face
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);

        // Left face 
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);

        // Right face
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);

        // Top face
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(16);
        tris.Add(17);
        tris.Add(18);
        tris.Add(18);
        tris.Add(19);
        tris.Add(16);

        // Bottom face 
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(20);
        tris.Add(21);
        tris.Add(22);
        tris.Add(22);
        tris.Add(23);
        tris.Add(20);

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        return mesh;
    }
}