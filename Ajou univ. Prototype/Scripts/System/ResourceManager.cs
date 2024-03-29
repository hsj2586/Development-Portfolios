﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // 인게임에서의 리소스를 관리하는 싱글턴 스크립트.
    public static ResourceManager Instance { get; private set; }

    [SerializeField]
    private Dictionary<string, Sprite> itemSprites;
    private Dictionary<string, AudioClip> audioClips;
    private Dictionary<string, List<string>> dialogs;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);

        LoadItemSprites();
        LoadSoundClips();
        LoadDialogs();
    }

    private void LoadDialogs()
    {
        dialogs = new Dictionary<string, List<string>>();

        TextAsset[] items = Resources.LoadAll< TextAsset>("Dialogs/");

        foreach (TextAsset item in items)
        {
            dialogs.Add(item.name,FileManager.ConvertDataToList<string>(item.text));
        }
    }

    private void LoadItemSprites()
    {
        itemSprites = new Dictionary<string, Sprite>();
        Sprite[] items = Resources.LoadAll<Sprite>("ItemIcons/");

        foreach (Sprite item in items)
        {
            itemSprites.Add(item.name, item);
        }
    }
    private void LoadSoundClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] items = Resources.LoadAll<AudioClip>("Sound/");
        foreach (AudioClip item in items)
        {
            audioClips.Add(item.name, item);
        }
    }

    public Sprite GetItemIcon(string _itemName)
    {
        return itemSprites[_itemName];
    }

    public AudioClip GetAudioClip(string _clipname)
    {
        return audioClips[_clipname];
    }

    public List<string> GetDialog(string _clipname)
    {
        return dialogs[_clipname];
    }
}
