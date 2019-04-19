using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 싱글턴을 이용한 소리 출력 기능의 스크립트.
    private static AudioManager soundManager = null;
    [SerializeField]
    GameObject audioPool; // 각종 소리 소스의 풀
    [SerializeField]
    AudioSource BgmResource; // BGM 리소스

    Stack<GameObject> audioPoolList;

    void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
        }
        else if (soundManager != this)
            Destroy(gameObject);

        audioPoolList = new Stack<GameObject>();
        for (int i = 0; i < audioPool.transform.childCount; i++)
        {
            audioPoolList.Push(audioPool.transform.GetChild(i).gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static AudioManager Instance
    {
        get
        {
            return soundManager;
        }
    }

    public GameObject AudioPoolPop()
    {
        if (audioPoolList.Count != 0)
        {
            GameObject temp = audioPoolList.Pop();
            temp.SetActive(true);
            return temp;
        }
        else
            return null;
    }

    public void AudioPoolPush(GameObject obj)
    {
        obj.SetActive(false);
        audioPoolList.Push(obj);
    }

    public void PlaySoundOneShot(string clipName)
    {
        GameObject obj = AudioPoolPop();
        AudioSource temp = obj.GetComponent<AudioSource>();
        temp.clip = Resources.Load<AudioClip>("Sound/SoundEffect/" + clipName);
        temp.loop = false;
        temp.Play();
        StartCoroutine(SoundDelay(temp, 2));
    }

    public void PlayMainSound(string clipName)
    {
        BgmResource.clip = Resources.Load<AudioClip>("Sound/BGM/" + clipName);
        BgmResource.Play();
    }

    public void StopMainSound()
    {
        if (BgmResource.isPlaying)
        {
            BgmResource.Stop();
        }
    }

    IEnumerator SoundDelay(AudioSource param, float time) // 오디오 풀의 소리 반환
    {
        yield return new WaitForSeconds(time);
        AudioPoolPush(param.gameObject);
    }

    public void SetBgmVolume(float value)
    {
        BgmResource.volume = value;
    }
}
