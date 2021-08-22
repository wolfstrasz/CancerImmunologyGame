using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using ImmunotherapyGame.UI;
using ImmunotherapyGame.Audio;

using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Core
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        [SerializeField] private InterfaceControlPanel panel = null;

        [Header("UI Audio Control")]
        [SerializeField] private Slider masterVolumeSlider = null;
        [SerializeField] private Slider musicVolumeSlider = null;
        [SerializeField] private Slider sfxVolumeSlider = null;
        [SerializeField] private Slider uiVolumeSlider = null;

        [Header("Graphics & Input Control")]
        [SerializeField] private MenuDropdown inputDropdown = null;
        [SerializeField] private InputActionAsset inputAsset = null;
        [SerializeField] [ReadOnly]private int currentSchemeIndex = 0;

        [SerializeField] private Toggle fullscreenToggle = null;
        [SerializeField] [ReadOnly] private bool currentToggle = false;

        [SerializeField] private MenuDropdown resolutionDropdown = null;
        [SerializeField] [ReadOnly] private int currentResolutionIndex = 0;
        [SerializeField] [ReadOnly] private string currentResolution = "";
        private Resolution[] resolutions = null;

		public void Initialise()
		{


#if UNITY_WEBGL
#else
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

            LoadGraphicsSettings(currentResolutionIndex);
            LoadInputSettings(currentSchemeIndex);
            ApplyChangedSettings();

#endif 

            LoadVolumeSettings();
			// Register to volume sliders
			masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
			musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
			sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
			uiVolumeSlider.onValueChanged.AddListener(delegate { SetUIVolume(); });

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
            AudioManager.Instance.SetVolume(AudioChannel.Master, volume * 100f);
            PlayerPrefs.SetFloat("MasterVolumePreference", volume);
        }

        private void SetSFXVolume()
        {
            float volume = sfxVolumeSlider.value;
            AudioManager.Instance.SetVolume(AudioChannel.SFX, volume * 100f);
            PlayerPrefs.SetFloat("SFXVolumePreference", volume);

        }

        private void SetUIVolume()
        { 
            float volume = uiVolumeSlider.value;
            Debug.Log(" ++++++++ SETTING UI VOLUME: " + volume);
            AudioManager.Instance.SetVolume(AudioChannel.UI, volume * 100f);
            PlayerPrefs.SetFloat("UIVolumePreference", volume);

        }

        private void SetMusicVolume()
		{
            float volume = musicVolumeSlider.value;
            AudioManager.Instance.SetVolume(AudioChannel.Master, volume * 100f);
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
            float value = 0f;
			value = PlayerPrefs.GetFloat("MasterVolumePreference", 1f);
            masterVolumeSlider.value = value;
            AudioManager.Instance.SetVolume(AudioChannel.Master, value * 100f);
            PlayerPrefs.SetFloat("SFXVolumePreference", value);

            value = PlayerPrefs.GetFloat("MusicVolumePreference", 1f);
            musicVolumeSlider.value = value;
            AudioManager.Instance.SetVolume(AudioChannel.Music, value * 100f);
            PlayerPrefs.SetFloat("SFXVolumePreference", value);

            value = PlayerPrefs.GetFloat("SFXVolumePreference", 1f);
            sfxVolumeSlider.value = value;
            AudioManager.Instance.SetVolume(AudioChannel.SFX, value * 100f);
            PlayerPrefs.SetFloat("SFXVolumePreference", value);

            value = PlayerPrefs.GetFloat("UIVolumePreference", 1f);
            uiVolumeSlider.value = value;
            AudioManager.Instance.SetVolume(AudioChannel.UI, value * 100f);
            PlayerPrefs.SetFloat("SFXVolumePreference", value);

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


        public void Open()
		{

#if UNITY_WEBGL
#else
            ReloadValues();
#endif
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
