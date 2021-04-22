using UnityEngine;

namespace ImmunotherapyGame
{
    public class DevPreload : MonoBehaviour
    {
        void Awake()
        {
            if (!GlobalGameData.isInitialised)
            {
                Debug.Log("Loading _preload scene");
                UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
            }
        }
    }
}