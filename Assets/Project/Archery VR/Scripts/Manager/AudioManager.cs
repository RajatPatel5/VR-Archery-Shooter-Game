using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.VRArchery.Managers
{


    public class AudioManager : MonoBehaviour
    {

        public static AudioManager inst;

        [SerializeField] public AudioSource audioSource;


        public Audio[] clips;


        void Start()
        {
            inst = this;
        }

        public void PlayAudio(AudioName name)
        {
            foreach (var item in clips)
            {
                if (item.name == name)
                {
                    audioSource.PlayOneShot(item.clip);
                    break;
                }
            }
        }

        [System.Serializable]
        public class Audio
        {
            public AudioName name;
            public AudioClip clip;
        }

        public enum AudioName
        {            
            Onclick,
            BetterLuckNextTime,
            Excellent,
            GoodShort,
            GoodOne,
            Wooh,
            Onhitwall,                        
        }
    }
}