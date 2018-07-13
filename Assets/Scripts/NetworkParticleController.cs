using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkParticleController : MonoBehaviour {

    GameObject[] generators;
	// Use this for initialization
	void Start () {
        generators = GameObject.FindGameObjectsWithTag("BaseObj");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
