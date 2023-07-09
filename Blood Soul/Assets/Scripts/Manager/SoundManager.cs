using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffect
{
    //Player
    public static string PlayerWalk = "PlayerWalk";
    public static string PlayerSword = "PlayerSword";
    public static string PlayerSkill = "PlayerSkill";
    public static string PlayerRoll = "PlayerRoll";

    //Hydra
    public static string HydraBreath = "HydraBreath";
    public static string HydraEarthQuake = "HydraEarthQuake";
    public static string HydraHit = "HydraHit";
    public static string HydraTail = "HydraTail";
    public static string HydraWalk = "HydraWalk";

    //Addtion
    public static string GetItem = "GetItem";
    public static string Elevator = "Elevator";
}

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> sfxSounds = new Dictionary<string, AudioClip>();
    public AudioSource BGM;

    public AudioClip[] Sfxs;
    public AudioClip[] Bgms;

    private void Awake()
    {
        SetInst();
        foreach (AudioClip auido in Sfxs)
        {
            sfxSounds.Add(auido.name, auido);
        }
    }

    //사운드 재생
    public void PlaySFX(string soundName, float volume = 1f, float speed = 1f, float deleteTime = 0f)
    {
        AudioSource audioSource = new GameObject("sound").AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
        audioSource.clip = sfxSounds[soundName];
        audioSource.pitch = speed;

        StartCoroutine(SFXBlur(audioSource, deleteTime));
        audioSource.Play();

        if (deleteTime != 0)
        {
            Destroy(audioSource.gameObject, deleteTime);
            return;
        }
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    //사운드가 서서히 줄어들게 하는 함수
    private IEnumerator SFXBlur(AudioSource source, float deleteTime)
    {
        float t = deleteTime;

        while (true)
        {
            yield return null;

            if (source == null) yield break;
            t -= Time.deltaTime;
            source.volume = t / deleteTime;
        }
    }

    //BGM 재생
    public void PlayBGM(int num = 0, float volum = 0.8f)
    {
        BGM.volume = volum;

        BGM.clip = Bgms[num];
        BGM.Play();
    }
    public void StopBGM()
    {
        BGM.Stop();
    }
}
