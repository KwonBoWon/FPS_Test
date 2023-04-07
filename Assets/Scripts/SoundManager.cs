using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    #region singleton
    private void Awake() // 객체 생성시 최초실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(instance.gameObject); 
        }
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    
    public Sound[] effectSounds;
    public Sound[] bgmSounds;
    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];  
    }
    public void PlaySE(string _name)
    {
        for(int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = audioSourceEffects[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 오디오소스가 사용중입니다.");
                return;
            }
            Debug.Log(_name + "사운드가 사운드매니저에 등록되지 않았습니다.");
        }
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }

    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다");
    }
}
