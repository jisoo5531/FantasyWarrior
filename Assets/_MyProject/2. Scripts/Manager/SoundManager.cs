using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] audioClips;
    // ���� Ŭ���� ������ Dictionary
    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public AudioSource audioSource;

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ���� ����Ǿ SoundManager ����
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource ������Ʈ ����
        audioSource = GetComponent<AudioSource>();

        // ���� Ŭ���� Dictionary�� �߰� (�̸� �߰��ϰų�, �������� �ε� ����)
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

    // ���带 ����ϴ� �Լ�
    public void PlaySound(string clipName)
    {
        if (soundClips.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(soundClips[clipName]);
        }
        else
        {
            Debug.LogWarning($"SoundManager: {clipName} ���带 ã�� �� �����ϴ�.");
        }        
    }

    // ������� (BGM)�� ����ϴ� �Լ�
    public void PlayBGM(string clipName)
    {
        if (soundClips.ContainsKey(clipName))
        {
            audioSource.clip = soundClips[clipName];
            audioSource.loop = true;  // ��������� �ݺ� ���
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"SoundManager: {clipName} ��������� ã�� �� �����ϴ�.");
        }
    }

    // ���� ���� �Լ�
    public void StopSound()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
