using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;
    private static GameObject container;

    public static SoundManager getInstance
    {
        get
        {
            if (instance == null)
            {
                container = new GameObject();
                container.name = "SoundManager";
                instance = container.AddComponent(typeof(SoundManager)) as SoundManager;
            }
            return instance;
        }
    }

    public void EffectSoundPlay(string type)
    {
        GameObject effectAudioObj = GameObject.Find(type);
        if (effectAudioObj != null)
        {
            AudioSource effectAudio = effectAudioObj.GetComponent<AudioSource>();
            if (effectAudio != null) effectAudio.Play(0); 
        }
    }
}
