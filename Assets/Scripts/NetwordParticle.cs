using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

//public struct Particle
//{
//    public Vector3 pos;
//    public Vector3 start;
//    public Vector3 end;
//    public float time;
//    public float lifeTime;
//    public int active;

//    public Particle(Vector3 pos,Vector3 start,Vector3 end)
//    {
//        this.pos = pos;
//        this.start = start;
//        this.end = end;
//        time = 0f;
//        lifeTime = 0;
//        active = 0;
//    }
//}

public class NetwordParticle : MonoBehaviour {

    Vector4[] basePoints;

    public int Nparticle =  1000;
    public Shader shader;
    Material material;

    public ComputeShader cpShader;
    public ComputeBuffer particleBuffer;

    int updateKernel;
    int emitKernel;
    int initKernel;

    // Use this for initialization

	void Start () {
        material = new Material(shader);
        InitBuffer();

        basePoints = new Vector4[GameObject.FindGameObjectsWithTag("BaseObj").Length];

		for(int i = 0; i < GameObject.FindGameObjectsWithTag("BaseObj").Length; i++)
        {
            basePoints[i] = GameObject.FindGameObjectsWithTag("BaseObj")[i].transform.position;
        }

        initKernel = cpShader.FindKernel("Init");
        emitKernel = cpShader.FindKernel("Emit");
        updateKernel = cpShader.FindKernel("Update");

        cpShader.SetBuffer(initKernel, "particles", particleBuffer);

        cpShader.SetInt("baseNum", basePoints.Length);
        cpShader.SetVectorArray("basePoints", basePoints);

        cpShader.Dispatch(0, particleBuffer.count / 8 + 1, 1, 1);

    }

    void InitBuffer()
    {
        particleBuffer = new ComputeBuffer(Nparticle, Marshal.SizeOf(typeof(Particle)), ComputeBufferType.Default);

        Particle[] particles = new Particle[particleBuffer.count];

        for(int i = 0; i < particleBuffer.count; i++)
        {
            Vector3 initvec = new Vector3(0, 0, 0);
            particles[i] = new Particle(initvec, initvec, initvec);
        }

        particleBuffer.SetData(particles);
    }


    // Update is called once per frame
    void Update ()
    {
        cpShader.SetBuffer(updateKernel, "particles", particleBuffer);
        cpShader.SetFloat("deltaTime", Time.deltaTime);
        cpShader.Dispatch(updateKernel,particleBuffer.count / 8 + 1, 1, 1);

        cpShader.SetBuffer(emitKernel, "particles", particleBuffer);
        cpShader.SetFloat("globalTime", Time.frameCount);
        cpShader.Dispatch(emitKernel, particleBuffer.count / 8 + 1, 1, 1);

    }

    void OnRenderObject()
    {
        material.SetBuffer("particleBuffer", particleBuffer);
        material.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }

    void OnDisable()
    {
        particleBuffer.Release();
    }
}
