using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCarInStartMenu : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DespawnPlayer(other.GetComponentInParent<RearWheelDrive>().PlayerNum, other.transform);
        }
    }
    void DespawnPlayer(int id, Transform tran)
    {
        if (GameManager.Instance.PlayerCars[id] == null)
            return;
        GameManager.Instance.PlayerCount -= 1;
        GameObject car = Instantiate(GameManager.Instance.ExplosionPrefab, tran.position, tran.rotation);
        GameManager.Instance.PlayerCars[id] = null;
        Destroy(tran.parent.gameObject);
    }
}
