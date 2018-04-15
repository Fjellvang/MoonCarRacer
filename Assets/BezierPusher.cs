using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPusher : MonoBehaviour {

    private bool _move;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _move = true;
        }
    }
    private void FixedUpdate()
    {
        if(_move)
        {
            FollowBezier.Instance.MoveForward();
            _move = false;
        }
    }
}
