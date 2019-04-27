using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CombineMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CombineMesh()
    {
        Quaternion rot = transform.rotation;
        Vector3 pos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Mesh Meshfinal = new Mesh();


        CombineInstance[] combineAll = new CombineInstance[filters.Length];



        for (int i = 0; i < filters.Length; i++)
        {
            if(filters[i].transform == transform) continue;
            combineAll[i].subMeshIndex = 0;
            combineAll[i].mesh = filters[i].sharedMesh;
            combineAll[i].transform = filters[i].transform.localToWorldMatrix;
        } 

        Meshfinal.CombineMeshes(combineAll);

        GetComponent<MeshFilter>().sharedMesh = Meshfinal;

        transform.rotation = rot;
        transform.position = pos;
    }
}
