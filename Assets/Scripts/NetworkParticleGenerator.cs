using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public struct Particle
{
    public Vector3 pos;
    public Vector3 start;
    public Vector3 end;
    public Vector3 vec;
    public float time;
    public float lifeTime;
    public int active;

    public Particle(Vector3 pos, Vector3 start,Vector3 end)
    {
        this.pos = pos;
        this.start = start;
        this.end = end;
        this.vec = new Vector3(0,0,0);
        time = 0;
        lifeTime = 0;
        active = 0;
    }
}

public class NetworkParticleGenerator : MonoBehaviour
{
    public GameObject[] destinations;
    public int NParticle = 100;

    public ComputeShader cpShader;
    public ComputeBuffer particleBuffer;

    int updateKernel;
    int emitKernel;
    int initKernel;

    // Use this for initialization
    void Start()
    {
        InitBuffer();

        initKernel = cpShader.FindKernel("Init");
        emitKernel = cpShader.FindKernel("Emit");
        updateKernel = cpShader.FindKernel("Update");

        cpShader.SetBuffer(initKernel, "particles", particleBuffer);
        cpShader.SetFloat("globalTime", Time.frameCount);
        cpShader.Dispatch(initKernel, particleBuffer.count / 8 + 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        cpShader.SetBuffer(updateKernel, "particles", particleBuffer);
        cpShader.SetFloat("deltaTime", Time.deltaTime);
        cpShader.Dispatch(updateKernel, particleBuffer.count / 8 + 1, 1, 1);

        cpShader.SetBuffer(emitKernel, "particles", particleBuffer);
        cpShader.Dispatch(emitKernel, particleBuffer.count / 8 + 1, 1, 1);
    }

    public Vector3[] GetDestinations(){
        
        Vector3[] pos = new Vector3[destinations.Length];

        for (int i = 0; i < destinations.Length; i++){
            pos[i] = destinations[i].transform.position;
        }

        return pos;
    }

    void InitBuffer()
    {
        particleBuffer = new ComputeBuffer(NParticle, Marshal.SizeOf(typeof(Particle)));

        Particle[] particles = new Particle[particleBuffer.count];
        
        for (int i = 0; i < particleBuffer.count; i++)
        {
            particles[i] = new Particle(this.gameObject.transform.position, this.gameObject.transform.position, this.destinations[Random.Range(0, this.destinations.Length)].transform.position);
        }
        particleBuffer.SetData(particles);
    }

    void OnDisable()
    {
        particleBuffer.Release();
    }
}
