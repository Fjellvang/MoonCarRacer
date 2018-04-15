using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTheLosers : MonoBehaviour {

	void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.DestroyCar(other.GetComponentInParent<RearWheelDrive>().gameObject);
        }
    }
}
