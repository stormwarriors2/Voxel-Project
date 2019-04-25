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
    [Range(50, 1000)] public int cols = 100;
    /// <summary>
    /// Rows is the rows of voxels WIDTH
    /// </summary>
    [Range(50, 1000)] public int rows = 100;
    /// <summary>
    /// Is the Position of the voxel spawner and each voxel
    /// </summary>
    private Vector3 Pos;
    /// <summary>
    /// Threshold for the Voxels
    /// </summary>
    [Range(2f, 100f)] public float thresh = 2f;
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

        for (int x = 0; x < cols; x++)
        {


            for (int z = 0; z < rows; z++)
            {

                float y = Perlin.Noise((Pos.x + x) / freq, 0, (Pos.z + z) / freq) * amp;

                Vector3 worldPosition = new Vector3(Pos.x + x, Pos.y + y, Pos.z + z);
                Vector3 noiseCoords = worldPosition / thresh;
                float density = Perlin.Noise(noiseCoords);
                density -= worldPosition.y * thresh;
               
                y = Mathf.Floor(y);
               
                if (density > (freq / amp) * thresh) continue;
                ///This was for fun wanted to see what happens if i made comparative statements with a perlin noise added in
                if (y > x && y >= z)
                {


                    y = Perlin.Noise((Pos.x + x), (Pos.y + y), (Pos.z + z)) / freq;
                }
                if (y > amp / 2)
                    {
                        currentVoxelType = voxelColorType[0];

                    }
                    else if (y < amp / 2)
                    {
                        currentVoxelType = voxelColorType[1];
                        if (y < amp / freq)
                        {
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
}