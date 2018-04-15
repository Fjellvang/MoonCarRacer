using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//namespace Sound.SoundController
//{
    [RequireComponent(typeof(AudioSource))]

    public class SoundController : MonoBehaviour
    {

        public AudioClip[] Soundfiles;
        private AudioSource SoundEmitter;
        public float PitchMinimum = 1f;
        public float PitchMaximum = 1f;
        public bool RetriggerPrevention = true;
        


        // Use this for initialization
        void Start()
        {
            SoundEmitter = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlaySoundClip("BrakeLoop");
                Debug.Log("the key was pressed");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                SoundEmitter.Play();
                Debug.Log("it should play!!!");
            }

        }

        public void PlaySoundClip(string sound)
        {
            float pitch = Random.Range(PitchMinimum, PitchMaximum);
            SoundEmitter.pitch = pitch;

            int n = 0;
        // Determine which number sound to pay
        if (sound == "BrakeLoop") n = 1;
        if (sound == "Engine1") n = 2;
        if (sound == "EngineWithNoise") n = 3;

		SoundEmitter.clip = Soundfiles[n];
		SoundEmitter.PlayOneShot(SoundEmitter.clip);
		 Debug.Log(n);
		



            //make last played audioclip unplayable

            if (RetriggerPrevention)
            {
                Soundfiles[n] = Soundfiles[0];
                Soundfiles[0] = SoundEmitter.clip;
                Debug.Log("Retrigger Prevention is Active");
            }

        }

        public static void Trigger(SoundController src, string sound)
        {
            Debug.Log("The SoundTrigger Was Triggered");
            if (src != null) src.PlaySoundClip(sound);
        }

    }

//}