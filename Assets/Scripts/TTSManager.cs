using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static MainMode;

public class TTSManager : MonoBehaviour
{
    public static TTSManager instance;

    public AudioClip[] ManTTSClips;
    public AudioClip[] WomanTTSClips;

    public bool isMan = false;

    [SerializeField] AudioSource audioSource;

    public LocationData currentLocation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayTTS()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            for (int i = 0; i < WomanTTSClips.Length; i++)
            {
                if (isMan)
                {
                    if (currentLocation.place.Equals(ManTTSClips[i].name))
                    {
                        audioSource.clip = ManTTSClips[i];
                        audioSource.Play();
                    }
                }
                else
                {
                    if (currentLocation.place.Equals(WomanTTSClips[i].name))
                    {
                        audioSource.clip = WomanTTSClips[i];
                        audioSource.Play();
                    }
                }
            }
        }
    }

    public void ChangeVoice()
    {
        audioSource.Stop();
        isMan = !isMan;
    }
}
