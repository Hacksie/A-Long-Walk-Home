using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace HackedDesign
{
    public class PlayerPreferences : MonoBehaviour
    {
        public float masterVolume;
        public float sfxVolume;
        public float musicVolume;
        public bool infiniteWaves;
        public bool mechControls = false; 
        public int difficulty = 2;

        public static PlayerPreferences Instance { get; private set; }

        PlayerPreferences()
        {
            Instance = this;
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("InfiniteWaves", infiniteWaves ? 1 : 0);
            PlayerPrefs.SetInt("MechControls", mechControls ? 1 : 0);
        }

        public void Load()
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
            infiniteWaves = PlayerPrefs.GetInt("InfiniteWaves", 0) == 1 ?  true : false;
            mechControls = PlayerPrefs.GetInt("MechControls", 0) == 1 ? true : false;
            SetPreferences();
        }

        public void SetPreferences()
        {
            AudioManager.Instance.SetMasterVolume(this.masterVolume);
            AudioManager.Instance.SetSFXVolume(this.sfxVolume);
            AudioManager.Instance.SetMusicVolume(this.musicVolume);
        }

    }
}