using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public struct Particle
{
    public Vector3 pos;
    public Vector3 end;
    public Vector3 vec;
    public float time;
    public float lifeTime;
    public int active;

    public Particle(Vector3 pos, Vector3 end)
    {
        this.pos = pos;
        this.end = end;
        this.vec = new Vector3(0,0,0);
        time = 0f;
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

    // Use this for initialization
    void Start()
    {
        InitBuffer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector3[] GetSender(){
        
        Vector3[] pos = new Vector3[destinations.Length];

        for (int i = 0; i < destinations.Length; i++){
            pos[i] = destinations[i].transform.position;
        }

        return pos;
    }

    void InitBuffer()
    {
        particleBuffer = new ComputeBuffer(NParticle, Marshal.SizeOf(typeof(Particle)), ComputeBufferType.Default);

        Particle[] particles = new Particle[particleBuffer.count];

        for (int i = 0; i < particleBuffer.count; i++)
        {
            particles[i] = new Particle(this.gameObject.transform.position, this.destinations[Random.Range(0, this.destinations.Length)].transform.position);
        }
        particleBuffer.SetData(particles);
    }
}
