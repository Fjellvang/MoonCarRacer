using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour {

    private Transform[] spawnPoints = new Transform[5];
    // Use this for initialization
    public GameObject CarPrefab;
	void Start () {
        int i = 0;
        foreach (var spawn in GetComponentsInChildren<Transform>())
        {
            spawnPoints[i] = spawn;
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(Input.GetButtonDown("Jump0"))
        {
            SpawnPlayer(0);
        }
        if (Input.GetButtonDown("Jump1"))
        {
            SpawnPlayer(1);
        }
        if (Input.GetButtonDown("Jump2"))
        {
            SpawnPlayer(2);
        }
        if (Input.GetButtonDown("Jump3"))
        {
            SpawnPlayer(3);
        }
    }
    void SpawnPlayer(int id)
    {
        if (GameManager.Instance.PlayerCars[id] != null)
            return;
       
        GameManager.Instance.PlayerCount += 1;
        GameObject car = Instantiate(CarPrefab, spawnPoints[id].position, Quaternion.identity);
        car.GetComponent<RearWheelDrive>().PlayerNum = id;
        GameManager.Instance.PlayerCars[id] = car;
        DontDestroyOnLoad(car);
    }
    

}
