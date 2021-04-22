using UnityEngine;

namespace ImmunotherapyGame
{  
    public class DDOL : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
            Debug.Log("DDOL: " + this.name);
        }
    }
}