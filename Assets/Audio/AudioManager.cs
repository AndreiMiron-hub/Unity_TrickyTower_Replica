using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Sfx, Music };
    int ActiveMusicSourceIndex;
    public float MasterVolumePercent { get; private set; }
    public float SfxVolumePercent { get; private set; }
    public float MusicVolumePercent { get; private set; }

    AudioSource[] MusicSources;
    public static AudioManager Instance;

    private void Awake()
    {
        MasterVolumePercent = PlayerPrefs.GetFloat("MasterVolume");
        SfxVolumePercent = PlayerPrefs.GetFloat("SfxVolume");
        MusicVolumePercent = PlayerPrefs.GetFloat("MusicVolume");

        if (MasterVolumePercent == 0)
        {
            MasterVolumePercent = 0.3f;
        }

        if (SfxVolumePercent == 0)
        {
            SfxVolumePercent = 1f;
        }

        if (MusicVolumePercent == 0)
        {
            MusicVolumePercent = 1f;
        }

        if (Instance != null)
        {
            Instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MusicSources = new AudioSource[2];

            for (int i = 0; i < MusicSources.Length; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                MusicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }
        }
    }

    public void SetVolume(float setVolumePercent, AudioChannel audioChannel)
    {
        switch (audioChannel)
        {
            case AudioChannel.Master:
                MasterVolumePercent = setVolumePercent;
                break;
            case AudioChannel.Sfx:
                SfxVolumePercent = setVolumePercent;
                break;
            case AudioChannel.Music:
                MusicVolumePercent = setVolumePercent;
                break;
        }

        MusicSources[0].volume = MusicVolumePercent * MasterVolumePercent;
        MusicSources[1].volume = MusicVolumePercent * MasterVolumePercent;

        PlayerPrefs.SetFloat("MasterVolume", 0.3f);
        PlayerPrefs.SetFloat("SfxVolume", 1);
        PlayerPrefs.SetFloat("MusicVolume", 1);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip musicClip, float fadeDuration = 1)
    {
        ActiveMusicSourceIndex = 1 - ActiveMusicSourceIndex;
        MusicSources[ActiveMusicSourceIndex].clip = musicClip;
        MusicSources[ActiveMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip is not null)
        {
            AudioSource.PlayClipAtPoint(clip, position, SfxVolumePercent * MasterVolumePercent);
        }
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            MusicSources[ActiveMusicSourceIndex].volume = Mathf.Lerp(0, MusicVolumePercent * MasterVolumePercent, percent);
            MusicSources[1 - ActiveMusicSourceIndex].volume = Mathf.Lerp(percent, MusicVolumePercent * MasterVolumePercent, 0);
        }

        yield return null;
    }
}
