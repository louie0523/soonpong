using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;

    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    public AudioSource Bgm;
    public AudioSource Sfx;

    public GameObject Canvas;

    public Slider BgmSlider;
    public Slider SfxSlider;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Setting();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        
    }

    void Setting()
    {
        AudioClip[] clip = Resources.LoadAll<AudioClip>("Sfx/");

        foreach(AudioClip c in clip)
        {
            clips.Add(c.name, c);
        }

        Bgm = transform.GetChild(0).GetComponent<AudioSource>();
        Sfx = transform.GetChild(1).GetComponent<AudioSource>();
        Canvas = transform.GetChild(2).gameObject;
    }

    public void PlayBgm(string name)
    {
        if (!clips.ContainsKey(name))
        {
            Debug.Log("존재하지 않는 사운드클립");
            return;
        }

        AudioClip cc = clips[name];

        Bgm.clip = cc;
        Bgm.loop = true;
        Bgm.Play();
    }

    public void PlaySfx(string name)
    {
        if (!clips.ContainsKey(name))
        {
            Debug.Log("존재하지 않는 사운드클립");
            return;
        }

        AudioClip cc = clips[name];

        Sfx.PlayOneShot(cc);
    }
}
