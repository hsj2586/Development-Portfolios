using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 사운드를 담당하는 싱글턴 패턴의 클래스
    private static SoundManager soundmanager = null;
    [SerializeField]
    GameObject soundpool;
    [SerializeField]
    AudioSource mainsound;
    List<GameObject> soundpool_list;

    void Awake()
    {
        if (soundmanager == null)
        {
            soundmanager = this;
        }
        else if (soundmanager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        soundpool_list = new List<GameObject>();
        for (int i = 0; i < soundpool.transform.childCount; i++)
        {
            soundpool_list.Add(soundpool.transform.GetChild(i).gameObject);
        }
    }

    public static SoundManager Instance
    {
        get
        {
            return soundmanager;
        }
    }

    public GameObject AudioPoolPop()
    {
        if (soundpool_list.Count != 0)
        {
            GameObject temp = soundpool_list[0];
            soundpool_list.RemoveAt(0);
            temp.SetActive(true);
            return temp;
        }
        else
            return null;
    }

    public void AudioPoolPush(GameObject obj)
    {
        obj.SetActive(false);
        soundpool_list.Add(obj);
    }

    public void PlayMainSound(AudioClip clip)
    {
        mainsound.clip = clip;
        mainsound.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        GameObject obj = AudioPoolPop();
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
        StartCoroutine(SoundDelay(2f, obj));
    }

    IEnumerator SoundDelay(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        AudioPoolPush(obj);
    }
}
