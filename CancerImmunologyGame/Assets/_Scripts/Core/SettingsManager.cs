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
        private InterfaceControlPanel panel = null;
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
        private MenuDropdown inputDropdown = null;
        [SerializeField]
        private InputActionAsset inputAsset = null;
        [ReadOnly]
        private int currentSchemeIndex = 0;

        [SerializeField]
        private Toggle fullscreenToggle = null;
        private bool currentToggle = false;

        [SerializeField]
        private MenuDropdown resolutionDropdown = null;
        private Resolution[] resolutions = null;

        private int currentResolutionIndex = 0;
        [ReadOnly]
        private string currentResolution = "";

        public void Initialise()
		{
            // Volume init
            masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
            musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
            uiVolumeSlider.onValueChanged.AddListener(delegate { SetUIVolume(); });

            // Add Graphic init
			List<string> options = new List<string>();
			resolutions = Screen.resolutions;
			currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
			{
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);
				if (resolutions[i].width == Screen.currentResolution.width
					  && resolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionIndex = i;
                    currentResolution = resolutions[i].ToString();
                }
			}

            resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions(options);
            //Debug.Log("Add resolution options" + "// -> " + currentResolution);
            resolutionDropdown.CurrentValue = currentResolutionIndex;
            //Debug.Log("Select resolution: " + resolutionDropdown.CurrentValue);
			resolutionDropdown.RefreshShownValue();

            // Text Input Dropdown;
            options = new List<string>();
            options.Add("Any");
            currentSchemeIndex = 0;

            foreach (InputControlScheme scheme in inputAsset.controlSchemes)
			{
                options.Add(scheme.name);
			}

            inputDropdown.ClearOptions();
            inputDropdown.AddOptions(options);
            //Debug.Log("Add input options");
            inputDropdown.CurrentValue = currentSchemeIndex;
            //Debug.Log("Select input option: " + inputDropdown.CurrentValue);
            inputDropdown.RefreshShownValue();


            LoadVolumeSettings();
            LoadGraphicsSettings(currentResolutionIndex);
            LoadInputSettings(currentSchemeIndex);

            ApplyChangedSettings();
            panel.gameObject.SetActive(false);
        }

        public void ApplyChangedSettings()
		{
            Debug.Log("APPLYING CHANGES");
            ApplyGraphicsSettings();
            ApplyInputSettings();
		}

        private void ApplyInputSettings()
		{
            // Apply Input changes
            currentSchemeIndex = inputDropdown.CurrentValue;
            //Debug.Log("Setting new Input preference:" + currentSchemeIndex);
            // TODO: apply global scheme!
            PlayerPrefs.SetInt("InputPreference", currentSchemeIndex);
        }
        
        private void ApplyGraphicsSettings()
		{
            // Apply Graphics changes
            currentResolutionIndex = resolutionDropdown.CurrentValue;
            Resolution resolution = resolutions[currentResolutionIndex];
            //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            //Debug.Log("Setting new Resolution preference: " + resolution.width + " : " + resolution.height);
            PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.CurrentValue);

            currentToggle = fullscreenToggle.isOn;
            //Debug.Log("Setting new fullscreen preference: " + currentToggle);
            //Screen.fullScreen = isFullscreen;
            //Debug.Log(Screen.fullScreen);
            PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(fullscreenToggle.isOn));
        }

        private void SetMasterVolume()
        {
            float volume = masterVolumeSlider.value;
			//Debug.Log("Master Volume: " + volume);
			audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MasterVolumePreference", volume);

        }

        private void SetSFXVolume()
        {
            float volume = sfxVolumeSlider.value;
            //Debug.Log("SFX Volume: " + volume);

            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolumePreference", volume);

        }

        private void SetUIVolume()
        { 
            float volume = uiVolumeSlider.value;
            //Debug.Log("UI Volume: " + volume);

            audioMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("UIVolumePreference", volume);

        }

        private void SetMusicVolume()
		{
            float volume = musicVolumeSlider.value;
            //Debug.Log("Music Volume: " + volume);

            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolumePreference", volume);
        }

        private void LoadGraphicsSettings(int currentResolutionIndex)
		{
            Debug.Log("Loading Graphics Settings");
			if (PlayerPrefs.HasKey("ResolutionPreference"))
			{
				resolutionDropdown.CurrentValue = PlayerPrefs.GetInt("ResolutionPreference");
			}
			else
			{
				resolutionDropdown.CurrentValue = currentResolutionIndex;
                PlayerPrefs.SetInt("ResolutionPreference", currentResolutionIndex);
			}
            
            //Debug.Log("Loaded: " + resolutionDropdown.CurrentValue);

            //Debug.Log("Loading Toggle");
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
            currentToggle = fullscreenToggle.isOn;
            //Debug.Log("Loaded: " + currentToggle);
		}

        private void LoadVolumeSettings()
		{
            //Debug.Log("Loading volume");
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

        private void LoadInputSettings(int currentIndex)
        {
            //Debug.Log("Loading Input");
            if (PlayerPrefs.HasKey("InputPreference"))
                inputDropdown.CurrentValue = PlayerPrefs.GetInt("InputPreference");
            else
                inputDropdown.CurrentValue = currentIndex;

            currentSchemeIndex = inputDropdown.CurrentValue;
            //Debug.Log("Loaded: " + currentSchemeIndex);
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


        public void Open()
		{
            ReloadValues();
            panel.Open();
		}

        private void ReloadValues()
		{
            //Debug.Log("Reloading Values: ");
            inputDropdown.CurrentValue = currentSchemeIndex;
            //Debug.Log("Input: " + inputDropdown.CurrentValue);

            resolutionDropdown.CurrentValue = currentResolutionIndex;
            //Debug.Log("Resolution: " + resolutionDropdown.CurrentValue);

            fullscreenToggle.isOn = currentToggle;
            //Debug.Log("Fullscreen: " + fullscreenToggle.isOn);
		}

    }
}
