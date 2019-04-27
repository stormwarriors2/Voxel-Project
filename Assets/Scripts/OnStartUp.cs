using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// OnStartup
/// Creates and generates a land mass.
/// </summary>
public class OnStartUp : MonoBehaviour
{
    /// <summary>
    /// Voxel World Generator (How many are spawned)
    /// </summary>
    [Range(1, 3)] public float VoxelWorld = 2;
    /// <summary>
    /// World Transform Distance of the local distance of the object
    /// </summary>
    private float worldTransformDst;
    /// <summary>
    /// is the World Object REference for spawning the voxels.
    /// </summary>
    public GameObject worldobj;
    // Start is called before the first frame update
    /// <summary>
    /// Start based on voxel ocean idea of creating world on start instead of single object in scene
    /// </summary>
    void Start()
    {
        worldTransformDst = Random.Range(25, 50); // get a random range
        Vector3 tempos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); // get the local transform position of the camera

        float voxelX = VoxelWorld; //only done once
        float voxelZ = VoxelWorld / VoxelWorld; // only done once
        for (int x = 0; x < voxelX; x++)
        {

            for (int z = 0; z < voxelZ; z++)
            {
                if (voxelX > x && voxelZ > z)
                {
                    Vector3 worldPosition = new Vector3(x * worldTransformDst + tempos.x, tempos.y - worldTransformDst, z * worldTransformDst + tempos.z);
                    GameObject worldspawn = Instantiate(worldobj, worldPosition, Quaternion.identity);
                }
            }
            
        }
     
    }

    /// <summary>
    /// Void Function update that detects 
    /// </summary>
    private void Update()
    {
        /*
        Vector3 pos = this.transform.position;
        if (pos.x > 200 && pos.z > 200)
        {
            ///    RemoveOnSpawn.DestroyObject.gameObject
            /// DELETE OBJECTS WHEN PLAYER IS FAR ENOUGH AWAY
        }
        */
    }
}
