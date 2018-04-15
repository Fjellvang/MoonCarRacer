using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMe : MonoBehaviour {

	void Start () {
        GetComponentInChildren<Rigidbody>().AddExplosionForce(100.0f, transform.position, 2.0f);
        StartCoroutine(RemoveSelf());
	}

    private IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
