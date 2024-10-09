using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] audioClips;
    // 사운드 클립을 저장할 Dictionary
    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public AudioSource audioSource;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 SoundManager 유지
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource 컴포넌트 참조
        audioSource = GetComponent<AudioSource>();

        // 사운드 클립을 Dictionary에 추가 (미리 추가하거나, 동적으로 로드 가능)
        soundClips.Add("GameStart", Resources.Load<AudioClip>("Sounds/GameStartButton"));        
        soundClips.Add("Warning", Resources.Load<AudioClip>("Sounds/WarnAmountUpDown"));        
        soundClips.Add("LevelUp", Resources.Load<AudioClip>("Sounds/LevelUp"));        
        soundClips.Add("QuestComplete", Resources.Load<AudioClip>("Sounds/QuestComplete"));        
        soundClips.Add("WarningBuy", Resources.Load<AudioClip>("Sounds/Warning"));        
        soundClips.Add("LearnSkill", Resources.Load<AudioClip>("Sounds/GetSkill"));        
        soundClips.Add("EquipItemSkill", Resources.Load<AudioClip>("Sounds/ChangeEquip"));        
        soundClips.Add("Cancel", Resources.Load<AudioClip>("Sounds/CancelButton"));        
        soundClips.Add("DialogSelect", Resources.Load<AudioClip>("Sounds/DialogSelect"));        
        soundClips.Add("PanelOpenClose", Resources.Load<AudioClip>("Sounds/PanelOpenClose"));        
        soundClips.Add("ShopBuy", Resources.Load<AudioClip>("Sounds/ShopBuy"));        
        soundClips.Add("GoblinBGM", Resources.Load<AudioClip>("Sounds/GoblinBGM"));        
        soundClips.Add("HumanBGM", Resources.Load<AudioClip>("Sounds/HumanBGM"));        
        
        
    }

    // 사운드를 재생하는 함수
    public void PlaySound(string clipName)
    {
        if (soundClips.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(soundClips[clipName]);
        }
        else
        {
            Debug.LogWarning($"SoundManager: {clipName} 사운드를 찾을 수 없습니다.");
        }        
    }

    // 배경음악 (BGM)을 재생하는 함수
    public void PlayBGM(string clipName)
    {
        if (soundClips.ContainsKey(clipName))
        {
            audioSource.clip = soundClips[clipName];
            audioSource.loop = true;  // 배경음악은 반복 재생
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"SoundManager: {clipName} 배경음악을 찾을 수 없습니다.");
        }
    }

    // 사운드 정지 함수
    public void StopSound()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
