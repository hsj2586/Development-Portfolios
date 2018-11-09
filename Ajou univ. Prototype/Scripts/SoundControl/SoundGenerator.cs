using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    // '소리'라는 것을 구체화 한 클래스
    Vector3 source; // 소리의 근원지
    float volume; // 소리의 크기
    float range; // 소리의 범위

    public Sound(Vector3 source, float volume, float range)
    {
        this.source = source;
        this.volume = volume;
        this.range = range;
    }

    public Vector3 Source
    {
        get { return this.source; }
    }

    public float Volume
    {
        get { return this.volume; }
    }

    public float Range
    {
        get { return this.range; }
    }
}

public static class SoundGenerator
{
    // 소리 발생기의 기능을 하는 전역 클래스.
    public static void SoundTransmission(Sound sound, Transform subject)
    {
        Collider[] listeners = Physics.OverlapSphere(subject.position, sound.Range);
        foreach (var enemy in listeners)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().ListeningToSounds(sound);
            }
        }
    }
}
