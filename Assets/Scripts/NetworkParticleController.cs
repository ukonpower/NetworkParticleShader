using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkParticleController : MonoBehaviour {

    public Shader shader;

    GameObject[] generators;
    Material material;

    void Start () {
        generators = GameObject.FindGameObjectsWithTag("BaseObj");
        material = new Material(shader);
    }
	
	void Update () {
		
	}

    void OnRenderObject()
    {
        for(int i = 0; i < generators.Length; i++)
        {
            ComputeBuffer particleBuffer = generators[i].GetComponent<NetworkParticleGenerator>().particleBuffer;
            material.SetBuffer("particleBuffer", particleBuffer);
            material.SetPass(0);
            Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
        }    
    }
}
