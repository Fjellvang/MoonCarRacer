using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseClasses
{
    public class nVector : MonoBehaviour{

        public static Vector3 up;
        public static Vector3 one;
        public static Vector3 down;
        public static Vector3 zero;
        private void Awake()
        {
            up = new Vector3 (0,1,0);
            one = Vector3.one;
            zero = Vector3.zero;
            down = new Vector3(0,-1,0);
        }
    }
}
