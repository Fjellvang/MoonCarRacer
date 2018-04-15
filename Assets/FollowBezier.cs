using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;
using System.Linq;
public class FollowBezier : BaseSingleton<FollowBezier>{


    public BezierCurve bezierCurve;

    public Transform BezierCam;
    public Transform BezierDeath;

    private Vector3 _bezierDeathzoneGoto;
    private Vector3 _bezierCamGoto;
    WaitForSeconds _wait5ms = new WaitForSeconds(0.05f);
    public float BezierT;
    public float DeathZoneT;
    public float MoveForwardSpeed = 0.05f;
    public float StaticCamSpeed = 0.2f;
    public float ExpoCamSpeed = 1.0f;
    [SerializeField]
    List<CarDistance> PlayerCars;

    

    // Use this for initialization
    void Start () {

        SpawnPlayers();
        _bezierCamGoto = bezierCurve.GetPointAt(BezierT);
        PlayerCars = new List<CarDistance>();
        // print(bezierCurve.length);
    }
    
    void SpawnPlayers()
    {
        Vector3 spawnPoint = bezierCurve.GetPointAt(0);
        Vector3 forwardPoint = bezierCurve.GetPointAt(0.01f);
        int i = 0;
        spawnPoint += Vector3.left * 1.25f;
        foreach (var car in GameManager.Instance.PlayerCars)
        {
            if (car == null)
                continue;
            if (i % 2 == 1)
                RefreshCar(car, spawnPoint + Vector3.right * 3.5f * i,forwardPoint);
            else
                RefreshCar(car, spawnPoint - Vector3.right * 3.5f * i,forwardPoint);
            i++;
        }

    }
    private void RefreshCar(GameObject car, Vector3 sp, Vector3 fp)
    {
        car.transform.position = sp;
        car.transform.rotation = Quaternion.LookRotation(fp - sp);
        car.GetComponent<Rigidbody>().velocity = Vector3.zero;
        car.SetActive(true);
    }
    private void FixedUpdate()
    {
        //check who is closest
        CheckWhosInLead();
        float dist = Vector3.Distance(BezierCam.position, _bezierCamGoto) / ExpoCamSpeed;
        if (dist < 0.1f)
            return;
        BezierCam.position = Vector3.MoveTowards(BezierCam.position, _bezierCamGoto,StaticCamSpeed * dist);
        BezierCam.rotation = Quaternion.LookRotation(_bezierCamGoto- BezierCam.position);
        BezierDeath.position = Vector3.MoveTowards(BezierDeath.position, _bezierDeathzoneGoto, StaticCamSpeed * dist);
        BezierDeath.rotation = Quaternion.LookRotation(_bezierDeathzoneGoto- BezierDeath.position);
    }

    public void CheckWhosInLead()
    {

        //check if everyone is collected
        
        if(PlayerCars.Count < GameManager.Instance.PlayerCount) //
        {
            for (int i = PlayerCars.Count; i < GameManager.Instance.PlayerCount; i++)
            {
                PlayerCars.Add(new CarDistance(GameManager.Instance.PlayerCars[i], SmallestDistance(GameManager.Instance.PlayerCars[i].transform)));
            }
        }
        foreach (var car  in PlayerCars)
        {
            car.dist = SmallestDistance(car.car.transform);
        }
        PlayerCars.Sort();
    }
    public void DeliverPoints()
    {
        CheckWhosInLead();
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            print("score mig " + i);
            GameManager.Instance.PlayerScores[PlayerCars[i].car.GetComponent<RearWheelDrive>().PlayerNum] += GameManager.Instance.Scoring[i];
        }
        print("Scores: Car1=" + GameManager.Instance.PlayerScores[0] + " Car2=" + GameManager.Instance.PlayerScores[1] + " Car3=" + GameManager.Instance.PlayerScores[2] + " Car4=" + GameManager.Instance.PlayerScores[3]);
    }
    class CarDistance : IComparable<CarDistance>
    {
        public GameObject car;
        public float dist;
        public CarDistance(GameObject go, float dis)
        {
            car = go;
            dist = dis;
        }

        public int CompareTo(CarDistance other)
        {
            return this.dist.CompareTo(other.dist);
        }
    }
    private float SmallestDistance(Transform car)
    {
        float dist = Vector3.Distance(BezierCam.position, car.position);
        float dist1 = Vector3.Distance(BezierCam.position + BezierCam.right, car.position);
        float dist2 = Vector3.Distance(BezierCam.position - BezierCam.right, car.position);

        if (dist < dist1 && dist < dist2)
            return dist;
        else if (dist1 < dist2 && dist1 < dist)
            return dist1;
        else
            return dist2;
    }
    public void MoveForward()
    {
        BezierT += (1.0f * Time.fixedDeltaTime * MoveForwardSpeed);
        if (BezierT > 1.0f)
            BezierT -= 1.0f;
        float dzT = DeathZoneT;
        if(BezierT - DeathZoneT < 0)
        {
            dzT = 1.0f - DeathZoneT;
        }
        _bezierCamGoto = bezierCurve.GetPointAt(BezierT);
        _bezierDeathzoneGoto = bezierCurve.GetPointAt(BezierT-dzT);
    }
}
