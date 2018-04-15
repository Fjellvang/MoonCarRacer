using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiiveLookAtScripts : MonoBehaviour {

	public GameObject toFollow;
	public GameObject toLookAt;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = toFollow.transform.position + offset;
		transform.LookAt(toLookAt.transform);	
	}
}
