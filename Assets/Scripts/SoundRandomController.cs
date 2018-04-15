/* Easy to use script that can randomize between audio clips.
 * Trigger this easily by adding a public SoundRandomController to your other script
 * and trigger it with SoundRandomControll.Trigger(yourname);
 * 
 * Copyright (C) 2017 Bjørn Jacobsen
 * This script is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This script is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this script.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sound.RandomController
{
    [RequireComponent(typeof(AudioSource))]

    public class SoundRandomController : MonoBehaviour {

        public AudioClip[] Soundfiles;
        private AudioSource SoundEmitter;
        public float PitchMinimum = 1f;
        public float PitchMaximum = 1f;
        public bool RetriggerPrevention = true;
        public string TriggerName;


        

    // Use this for initialization
    void Start()
        {
            SoundEmitter = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    PlaySoundClip();
            //    Debug.Log("the key was pressed");
            //}

            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    SoundEmitter.Play();
            //    Debug.Log("it should play!!!");
            //}

        }

        private void PlaySoundClip()
        {
            float pitch = Random.Range(PitchMinimum, PitchMaximum);
            SoundEmitter.pitch = pitch;

            //float Randompitch = Random.Range(PitchMinimum, PitchMaximum);
            //SoundEmitter.pitch = Randompitch;

            // Determine a random number between 1 and the size of the audio file pool
            int n = Random.Range(1, Soundfiles.Length);
            SoundEmitter.clip = Soundfiles[n];
            SoundEmitter.PlayOneShot(SoundEmitter.clip);
            // Debug.Log(n);



            //make last played audioclip unplayable

            if (RetriggerPrevention)
            {
                Soundfiles[n] = Soundfiles[0];
                Soundfiles[0] = SoundEmitter.clip;
                Debug.Log("Retrigger Prevention is Active");
            }

        }

        public static void Trigger(SoundRandomController src)
        {
            Debug.Log("The Trigger Was Triggered");
            if (src != null) src.PlaySoundClip();
        }

    }

}