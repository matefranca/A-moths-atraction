using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController instance;
    public float volumeAudio;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            ChangeVolume(volumeAudio);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volumeAudio;
    }

    public void ChangeVolume(float volume)
    {
        volumeAudio = volume;
        AudioListener.volume = volumeAudio;
    }
}
