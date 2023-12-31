using System;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundType
{
    Bgm,
    Sfx,
    Max
}

public class SoundManager : Singleton<SoundManager>
{
    private class AudioInfo
    {
        public AudioSource audioSource;
        public float audioVolume;
    }

    private readonly string path = "Sounds/";
    private readonly Dictionary<string, AudioClip> audioClips = new();

    private readonly Dictionary<ESoundType, AudioInfo> audioInfos = new();

    protected override bool IsDontDestroying => true;
    private const string AUDIO_SOURCE_NAME_BY_PITCH = "Pitch"; 

    protected override void OnCreated()
    {
        var clips = Resources.LoadAll<AudioClip>(path);
        foreach (var clip in clips)
            audioClips.Add(clip.name, clip);

        for (var soundType = ESoundType.Bgm; soundType < ESoundType.Max;soundType++)
            AddAudioInfo(soundType);

        var audioSource = audioInfos[ESoundType.Sfx].audioSource;
        PoolManager.Instance.JoinPoolingData(AUDIO_SOURCE_NAME_BY_PITCH, audioSource.gameObject);
    }

    protected override void OnReset()
    {
        StopAllSounds();
    }

    public void StopAllSounds()
    {
        foreach (var audioInfo in audioInfos.Values)
            audioInfo.audioSource.Stop();
    }

    public void UpdateVolume(ESoundType soundType, float sound)
    {
        audioInfos[soundType].audioVolume = sound;
        audioInfos[soundType].audioSource.volume = sound;
    }

    private void AddAudioInfo(ESoundType soundType)
    {
        var audioSourceObj = new GameObject(soundType.ToString());
        audioSourceObj.transform.SetParent(transform);

        var audioInfo = new AudioInfo
        {
            audioSource = audioSourceObj.AddComponent<AudioSource>(),
            audioVolume = 1
        };
        if(soundType == ESoundType.Bgm)
            audioInfo.audioSource.loop = true;
        audioInfos.Add(soundType, audioInfo);
    }

    public AudioSource GetAudioSource(ESoundType soundType = ESoundType.Sfx)
    {
        return audioInfos[soundType].audioSource;
    }

    public AudioClip PlaySound(string soundName, ESoundType soundType = ESoundType.Sfx, float multipleVolume = 1, float pitch = 1)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            audioInfos[soundType].audioSource.Stop();
            return null;
        }
        if (!audioClips.ContainsKey(soundName))
        {
            Debug.Log("그 소리 없음!");
            return null;
        }

        var clip = audioClips[soundName];
        var audioInfo = audioInfos[soundType];
        var audioSource = audioInfo.audioSource;

        if (Math.Abs(pitch - audioSource.pitch) > 0.03f)
        {
            audioSource = PoolManager.Instance.Init(AUDIO_SOURCE_NAME_BY_PITCH, transform).GetComponent<AudioSource>();
            audioSource.pitch = pitch;
        }

        if (soundType.Equals(ESoundType.Bgm))
        {
            audioSource.clip = clip;
            audioSource.volume = audioInfo.audioVolume * multipleVolume;
            audioSource.time = 0;
            audioSource.Play();
        }
        else //SFX
            audioSource.PlayOneShot(clip, audioInfo.audioVolume * multipleVolume);

        return clip;
    }
}