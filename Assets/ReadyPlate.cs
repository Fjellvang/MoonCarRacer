using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReadyPlate : MonoBehaviour {

    private int _readyPlayers = 0;
    bool start = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _readyPlayers++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _readyPlayers--;
        }
    }

  
    public TextMesh CountDown;
    float countdown;
    private void Update()
    {
        if (start)
        {
            foreach (var car in GameManager.Instance.PlayerCars)
            {
                if (car == null)
                    continue;
                car.SetActive(false);
            }
            SceneManager.LoadScene(1);
            return;
        }
        if (_readyPlayers >= 1)
        {
            countdown -= Time.deltaTime;
        }
        else
            countdown = 10.0f;
        CountDown.text = countdown.ToString("N1");
        if (countdown < 0)
            start = true;
    }

}
