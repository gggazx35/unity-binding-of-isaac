using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//namespace UnityEngine.Random;

public enum eSfx
{
    IsaacHurt,
    IsaacHurt1,
    IsaacHurt2,
    TearFire,
    TearFire1,
    TearBlcok,
    MonstroGrunt,
    MonstroBlob,
    ZombieGrunt,
    ZombieGrunt1,
    DoorClose,
    DoorOpen,
    HeartOut,
    Beep,
    Whimper,
    Whimper2,
    Explode,
    Explode1,
    Explode2
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    //Random rnd = new Random();
    // Start is called before the first frame update
    
    

    public eSfx GenerateRandomESfx(params eSfx[] _sfxs)
    {
        int i = Random.Range(0, _sfxs.Length);
        return (eSfx)_sfxs[i];
    }

    public eSfx GenerateRandomESfxL(eSfx[] _sfxs)
    {
        int i = Random.Range(0, _sfxs.Length);
        return (eSfx)_sfxs[i];
    }

    public void PlaySfxByEnum(eSfx _sfx)
    {
        for(int i = 0; i < channels; i++)
        {
            int loop = (channelIndex + i) % channels;

            if (SfxPlayers[loop].isPlaying)
            {
                continue;
            }

            channelIndex = loop;
            SfxPlayers[loop].clip = SfxClips[(int)_sfx];
            SfxPlayers[loop].Play();
            break;
        }

    }
    
    [Header("#Isaac")]
    public AudioClip[] Tear;
    public AudioClip[] IsaacHurt;

    [Header("#Zombie")]
    public AudioClip[] ZombieGrunt;

    [Header("BGM")]
    public AudioClip BgmClip;
    public AudioClip BgmClipBoss;
    AudioSource BgmPlayer;

    [Header("#SFX")]
    public AudioClip[] SfxClips;
    public float SfxVolume;
    public int channels;
    AudioSource[] SfxPlayers;
    int channelIndex;

    void Awake()
    {
        instance = this;
        Init();
        PlayBasementBGM(true);
    }

    public void PlayBasementBGM(bool _isPlaying)
    {
        if(_isPlaying)
        {
            BgmPlayer.Play();
        } else
        {
            BgmPlayer.Stop();
        }
    }

    public void SetBgmToBossBgm()
    {
        PlayBasementBGM(false);
        BgmPlayer.clip = BgmClipBoss;
        PlayBasementBGM(true);
    }

    void Init()
    {
        GameObject BgmObject = new GameObject("BgmPlayer");
        BgmObject.transform.parent = transform;
        BgmPlayer = BgmObject.AddComponent<AudioSource>();
        BgmPlayer.playOnAwake = false;
        BgmPlayer.loop = true;
        BgmPlayer.clip = BgmClip;

        GameObject SfxObject = new GameObject("SfxPlayer");
        SfxObject.transform.parent = transform;
        SfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            SfxPlayers[i] = SfxObject.AddComponent<AudioSource>();
            SfxPlayers[i].playOnAwake = false;
            SfxPlayers[i].volume = SfxVolume;

        }
    }
}
