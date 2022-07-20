using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HackedDesign
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [Header("SFX")]
        [SerializeField] private AudioSource warningSFX;
        [SerializeField] private AudioSource overdriveSFX;
        [SerializeField] private AudioSource coolantSFX;
        [SerializeField] private AudioSource pickupSFX;
        [SerializeField] private AudioSource repairSFX;
        [SerializeField] private AudioSource testSFX;
        [Header("Music")]
        [SerializeField] private AudioSource menuMusic;
        [SerializeField] private AudioSource introMusic;
        [SerializeField] private AudioSource loopMusic;
        [SerializeField] private AudioSource deadMusic;

        public static AudioManager Instance { get; private set; }

        AudioManager()
        {
            Instance = this;
        }

        public void SetMasterVolume(float value)
        {
            mixer.SetFloat("Master", Mathf.Log10(value) * 20);
        }

        public void SetSFXVolume(float value)
        {
            mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
            //PlayTest();
        }

        public void SetMusicVolume(float value)
        {
            mixer.SetFloat("Music", Mathf.Log10(value) * 20);
        }

        public void PlayWarning()
        {
            if (warningSFX != null && !warningSFX.isPlaying)
            {
                warningSFX.Play();
            }
        }

        public void PlayOverdrive()
        {
            if (overdriveSFX != null && !overdriveSFX.isPlaying)
            {
                overdriveSFX.Play();
            }
        }

        public void PlayRepair()
        {
            if (warningSFX != null && !warningSFX.isPlaying)
            {
                warningSFX.Play();
            }
        }

        public void PlayCoolantDump()
        {
            if (coolantSFX != null && !coolantSFX.isPlaying)
            {
                coolantSFX.Play();
            }
        }

        public void PlayPickup()
        {
            if (pickupSFX != null && !pickupSFX.isPlaying)
            {
                pickupSFX.Play();
            }
        }

        public void PlayTest()
        {
            if (testSFX != null)
            {
                testSFX.Play();
            }
        }

        public void PlayMenuMusic()
        {
            if (menuMusic != null)
            {
                menuMusic.Play();
            }
        }

        public void StopMenuMusic()
        {
            if (menuMusic != null)
            {
                menuMusic.Stop();
            }
        }

        public void PlayIntroMusic()
        {
            introMusic?.Play();
        }

        public bool IntroMusicIsPlaying { get => introMusic.isPlaying; }

        public void StopIntroMusic()
        {
            introMusic?.Stop();
        }

        public void PlayLoopMusic()
        {

            if (loopMusic != null && !loopMusic.isPlaying && loopMusic.time == 0)
            {
                loopMusic?.Play();
            }
            else if (loopMusic != null && !loopMusic.isPlaying && loopMusic.time != 0)
            {
                loopMusic?.UnPause();
            }
        }

        public void PauseLoopMusic()
        {
            if (loopMusic != null && loopMusic.isPlaying)
            {
                loopMusic?.Pause();
            }
        }

        public void StopLoopMusic()
        {
            loopMusic?.Stop();
        }

        public void PlayDeadMusic()
        {
            if (deadMusic != null && !deadMusic.isPlaying)
            {
                deadMusic?.Play();
            }
        }

        public void StopDeadMusic()
        {
            deadMusic?.Stop();
        }

    }
}
