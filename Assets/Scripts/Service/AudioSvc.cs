using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 音效服务
/// </summary>
public class AudioSvc : MonoBehaviour {
    private static AudioSvc instance = null;
    private AudioSvc() { }

    public AudioSource BGAudio;
    public AudioSource UIAudio;

    public static AudioSvc Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void InitSvc()
    {
        Debug.Log("Init AudioSvc...");
    }

    public void PlayBGAudio(string name,bool isloop = true)
    {
        AudioClip clip = ResSvc.Instance.LoadAudio("ResAudio/" + name,true);
        if (BGAudio.clip==null||BGAudio.clip.name!=clip.name)
        {
            BGAudio.clip = clip;
            BGAudio.loop = isloop;
            BGAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip clip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        UIAudio.clip = clip;
        UIAudio.Play();
    }
}
