using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;
using System;

public class GameManager : BaseSingleton<GameManager>
{

    public GameObject ExplosionPrefab;

    private Vector3 _noMansLand = new Vector3(1000, 1000, 1000);
    private WaitForSeconds _wait1Sec = new WaitForSeconds(1.0f);
    [HideInInspector] public int[] Scoring = { 3, 2, 1, -1 };

    private int[] playerScores = {0,0,0,0};

    private GameObject[] playerCars = new GameObject[4];

    private int playerCount;

    public int PlayerCount
    {
        get
        {
            return playerCount;
        }

        set
        {
            playerCount = value;
        }
    }

    public GameObject[] PlayerCars
    {
        get
        {
            return playerCars;
        }

        set
        {
            playerCars = value;
        }
    }

    public int[] PlayerScores
    {
        get
        {
            return playerScores;
        }

        set
        {
            playerScores = value;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1) // PLAY SCENE
        {
            if (PlayerCount == 0)
                Debug.LogWarning("NO PLAYERS FROM MENU");
          
        }
    }

    public void DestroyCar(GameObject go)
    {
        if (!go.activeSelf)
            return;
        go.SetActive(false);
        Instantiate(ExplosionPrefab,go.transform.position,go.transform.rotation); //to explode the vehicle
        go.transform.position = _noMansLand;
        FollowBezier.Instance.DeliverPoints();
        StartCoroutine(Respawn(go));
    }

    private IEnumerator Respawn(GameObject go)
    {
        yield return _wait1Sec;
        Vector3 spawnPoint = FollowBezier.Instance.bezierCurve.GetPointAt(FollowBezier.Instance.BezierT + FollowBezier.Instance.DeathZoneT);
        go.transform.position = spawnPoint + Vector3.up;
        go.transform.rotation = Quaternion.LookRotation(spawnPoint - FollowBezier.Instance.bezierCurve.GetPointAt(FollowBezier.Instance.BezierT));
        go.SetActive(true);
    }

    
}
