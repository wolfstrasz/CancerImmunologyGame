using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using ImmunotherapyGame.UI;
using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Core
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        [SerializeField]
        private GameObject panel = null;
        [SerializeField]
        private AudioMixer audioMixer = null;

        [Header("UI Audio Control")]
        [SerializeField]
        private Slider masterVolumeSlider = null;
        [SerializeField]
        private Slider musicVolumeSlider = null;
        [SerializeField]
        private Slider sfxVolumeSlider = null;
        [SerializeField]
        private Slider uiVolumeSlider = null;

        [Header("Graphics & Input Control")]
        [SerializeField]
        private GameObject applyButton = null;

        [SerializeField]
        private MenuDropdown inputDropdown = null;

        [SerializeField]
        private TMP_Dropdown resolutionDropdown = null;
        private Resolution[] resolutions = null;
 

        [SerializeField]
        private Toggle fullscreenToggle = null;
        private int currentResolutionIndex = 0;


        private void OnSkip()
		{
            Debug.Log("ES current GO: " + EventSystem.current.currentSelectedGameObject);
		}

        public void Initialise()
		{
            // Add Volume listeners
            masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
            musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
            uiVolumeSlider.onValueChanged.AddListener(delegate { SetUIVolume(); });

            //// Add Graphics & Input listeners
            //resolutionDropdown.ClearOptions();
            //List<string> options = new List<string>();
            //resolutions = Screen.resolutions;
            //currentResolutionIndex = 0;

            //for (int i = 0; i < resolutions.Length; i++)
            //{
            //    string option = resolutions[i].width + " x " + resolutions[i].height;
            //    options.Add(option);
            //    if (resolutions[i].width == Screen.currentResolution.width
            //          && resolutions[i].height == Screen.currentResolution.height)
            //        currentResolutionIndex = i;
            //}

            //resolutionDropdown.AddOptions(options);
            //resolutionDropdown.RefreshShownValue();

            //LoadResolutionSettings(currentResolutionIndex);

            LoadVolumeSettings();

            // Text Input Dropdown;
            inputDropdown.ClearOptions();
            inputDropdown.AddOptions(textInputDropdownPopulationValues);
            inputDropdown.RefreshShowValue();
            inputDropdown.onValueChanged += delegate { SetInputPreference(); };
            panel.SetActive(false);
        }

		private void SetMasterVolume()
        {
            float volume = masterVolumeSlider.value;
            Debug.Log("Master Volume: " + volume);
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MasterVolumePreference", volume);

        }

        private void SetSFXVolume()
        {
            float volume = sfxVolumeSlider.value;
            Debug.Log("SFX Volume: " + volume);

            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolumePreference", volume);

        }

        private void SetUIVolume()
        { 
            float volume = uiVolumeSlider.value;
            Debug.Log("UI Volume: " + volume);

            audioMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("UIVolumePreference", volume);

        }

        private void SetMusicVolume()
		{
            float volume = musicVolumeSlider.value;
            Debug.Log("Music Volume: " + volume);

            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolumePreference", volume);
        }


        public void SetFullscreen()
        {
			bool isFullscreen = fullscreenToggle.isOn;
            Debug.Log(isFullscreen);
            Screen.fullScreen = isFullscreen;
            Debug.Log(Screen.fullScreen);
			PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(fullscreenToggle.isOn));
		}

        public void SetResolution()
        {
			int resolutionIndex = resolutionDropdown.value;
			Resolution resolution = resolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

			PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
			PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(fullscreenToggle.isOn));
		}

        public void SetInputPreference()
		{
            Debug.Log("Setting new Input preference");
			//int difficultyIndex = inputDropdown.CurrentValue;
			//PlayerPrefs.SetInt("InputPreference", difficultyIndex);
		}


        private void LoadResolutionSettings(int currentResolutionIndex)
		{
			if (PlayerPrefs.HasKey("ResolutionPreference"))
			{
				resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
			}
			else
			{
				resolutionDropdown.value = currentResolutionIndex;
                PlayerPrefs.SetInt("ResolutionPreference", currentResolutionIndex);
			}

			if (PlayerPrefs.HasKey("FullscreenPreference"))
            {
                bool value = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
                fullscreenToggle.isOn = value;
                Screen.fullScreen = PlayerPrefs.GetInt("FullscreenPreference") == 1;
            }
            else
			{
                fullscreenToggle.isOn = true;
                Screen.fullScreen = true;
                PlayerPrefs.SetInt("FullscreenPreference", 1);
			}
		}

        private void LoadVolumeSettings()
		{
            float value = 0f;

            if (PlayerPrefs.HasKey("MasterVolumePreference"))
			    value = PlayerPrefs.GetFloat("MasterVolumePreference");
            else
			    value = 1f;
            UpdateSliderValue(masterVolumeSlider, value);


            if (PlayerPrefs.HasKey("MusicVolumePreference"))
			    value = PlayerPrefs.GetFloat("MusicVolumePreference");
            else
			    value = 1f;
            UpdateSliderValue(musicVolumeSlider, value);


            if (PlayerPrefs.HasKey("SFXVolumePreference"))
			    value = PlayerPrefs.GetFloat("SFXVolumePreference");
            else
			    value = 1f;
            UpdateSliderValue(sfxVolumeSlider, value);


            if (PlayerPrefs.HasKey("UIVolumePreference"))
			    value = PlayerPrefs.GetFloat("UIVolumePreference");
            else
			    value = 1f;
            UpdateSliderValue(uiVolumeSlider, value);

		}

        private void UpdateSliderValue (Slider slider, float value)
		{

            if (slider.value != value)
            {
                slider.value = value;
            }
            else
            {
                slider.value = 0f;
                slider.value = 1f;
                slider.value = value;
            }
        }

        private void LoadInputSettings()
		{
            if (PlayerPrefs.HasKey("InputPreference"))
                inputDropdown.CurrentValue = PlayerPrefs.GetInt("InputPreference");
            else
                inputDropdown.CurrentValue = 0;
        }

        public void Open()
		{
            panel.SetActive(true);
		}


        [Header("DropdownPopulations")]
        [SerializeField]
        List<string> textInputDropdownPopulationValues;

    }
}
