using UnityEngine;
using System.Collections;
using Sound.RandomController;

public class RearWheelDrive : MonoBehaviour
{

    private WheelCollider[] wheels;
    public int PlayerNum = 0;

    public float JumpCooldown;
    [SerializeField] private float _jumpCooldown;
    private Rigidbody _rig;

    public float JumpPower;
    public Vector3 JumpDirection;
    private Vector3 _jumpVec;
    public float maxAngle = 30;
    public float maxTorque = 300;
    public GameObject wheelShape;


    private string[] _inputs = { "MotorTurn", "MotorForward", "MotorBack", "Jump" };
    public string[] Inputs;

    private float timeNotgrounded = 0;
    public float flipForce = 50;
    public float flipTorque = 10;
    public float timeToFlip = 3;

    public SoundRandomController soundController, jumpSoundController;
    public AudioClip EngineSoundfile;
    private AudioSource SoundEmitter;
    public float startingPitch = 1;
    public float pitchRange = 0.2f;

    // here we find all the WheelColliders down in the hierarchy
    public void Start()
    {
        _rig = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelCollider>();
        int inputnum = 0;
        Inputs = new string[4];
        foreach (var input in _inputs)
        {
            Inputs[inputnum] = input + PlayerNum.ToString();
            inputnum++;
        }

        for (int i = 0; i < wheels.Length; ++i)
        {
            var wheel = wheels[i];
            //wheel.ConfigureVehicleSubsteps(5f, 5, 5);
            // create wheel shapes only when needed
            if (wheelShape != null)
            {
                var ws = GameObject.Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
        SoundEmitter = GetComponent<AudioSource>();
        SoundEmitter.pitch = startingPitch;
        if (!SoundEmitter.isPlaying)
        {
            SoundEmitter.loop = true;
            SoundEmitter.Play();
        }
    }

    // this is a really simple approach to updating wheels
    // here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
    // this helps us to figure our which wheels are front ones and which are rear
    public void Update()
    {
        float angle = maxAngle * Input.GetAxis(Inputs[0]);
        float torque = maxTorque * ((1.0f + Input.GetAxis(Inputs[2])) * -1) + maxTorque * (1.0f + Input.GetAxis(Inputs[1]));

        if (_jumpCooldown > 0)
            _jumpCooldown -= Time.deltaTime;
        //check is any of the wheels are grounded to perform a jump
        bool grounded = false;
        foreach (var wheel in wheels)
        {
            if (wheel.isGrounded)
            {
                grounded = true;
                break;
            }
        }

        //FlipCar(grounded);
        if (Input.GetButtonDown(Inputs[3]) && _jumpCooldown < 0)
        {
            if (!grounded)
            {
                FlipCar();
            }
            else
            {
                _jumpVec = JumpDirection * JumpPower;
                _rig.AddRelativeForce(_jumpVec, ForceMode.Impulse);
            }
            _jumpCooldown = JumpCooldown;
            SoundRandomController.Trigger(jumpSoundController);
        }


        foreach (WheelCollider wheel in wheels)
        {
            // a simple car where front wheels steer while rear ones drive
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;


            if (wheel.transform.localPosition.z < 0)
            {
                wheel.motorTorque = (torque / (Mathf.Abs(wheel.rpm / 3) + 1));
            }
            //Debug.Log(string.Format("{0}, {1},{2}",wheel.rpm, torque, torque/Mathf.Abs(wheel.rpm)+1)); 
            // update visual wheels if any
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // assume that the only child of the wheelcollider is the wheel shape
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }

        }
    }
    public void FlipCar(bool grounded)
    {
        if (!grounded)
        {
            timeNotgrounded += Time.deltaTime;
            if (timeNotgrounded > timeToFlip)
            { //change to not hardcoded variable
                Debug.Log("FLIP");
                _rig.AddForce(Vector3.up * flipForce, ForceMode.Impulse);
                _rig.AddRelativeTorque(Vector3.forward * flipTorque, ForceMode.Impulse);
                timeNotgrounded = 0;
            }
            return;
        }
        timeNotgrounded = 0;

    }
    public void FlipCar()
    {
        _rig.AddForce(Vector3.up * flipForce, ForceMode.Impulse);
        _rig.AddRelativeTorque(Vector3.forward * flipTorque, ForceMode.Impulse);
        timeNotgrounded = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            SoundRandomController.Trigger(soundController);
        }
    }

    private void PlaySoundClip(float torque)
    {

        float pitch = startingPitch + (torque / 3000) * pitchRange;
        //Debug.Log(torque);
        SoundEmitter.pitch = pitch;//Random.Range(startingPitch - m_PitchRange, startingPitch + m_PitchRange);
                                   //SoundEmitter.pitch = Random.Range(startingPitch - pitchRange, startingPitch + pitchRange);

    }
}
