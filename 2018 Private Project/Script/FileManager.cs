using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class JsonHelper // 리스트형 데이터 저장용 wrapper class
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }
    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = list;
        return JsonUtility.ToJson(wrapper);
    }
    public static string ToJson<T>(List<T> list, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = list;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}

public static class FileManager
{
    // 파일 저장, 불러오기 기능을 하는 전역 클래스.

    public static void WriteTextFile(string str, string path)
    {
        File.WriteAllText(path, str);
    }

    public static string ReadDataFile(string path)
    {
        string str;
        str = File.ReadAllText(path);
        if (str == null) return null;
        return str;
    }

    public static void DataGenerate<T>(string path, T data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        WriteTextFile(json, path);
    }

    public static void ListDataGenerate<T>(string path, List<T> data)
    {
        string json = JsonHelper.ToJson<T>(data, prettyPrint: true);
        WriteTextFile(json, path);
    }

    public static void AccountDataGenerate(string path, Account account)
    {
        AccountData file = new AccountData(account.Access_name, account.Access_level, account.Access_gold, account.Access_numofcharacter, account.Access_currentstage);
        DataGenerate<AccountData>(path, file);

        AllyCharacter temp;
        for (int i = 0; i < account.Access_numofcharacter; i++)
        {
            temp = account.Access_character(i).GetComponent<AllyCharacter>();
            DataGenerate<CharacterData>("SaveFile/UserData/" + temp.Access_prefabname + ".txt",
                new CharacterData(temp.Access_charactername, "Ally", temp.Access_faceimage, temp.Access_skillimage, temp.Access_atktype, temp.Access_atkpower, temp.Access_atkspeed, temp.Access_defpower,
                temp.Access_maxhealthpoint, temp.Access_skillname, temp.Access_level, temp.Access_exp,
                temp.Access_prefabname, temp.Access_prefabname + "AudioList.txt", temp.Access_prefabname + "EquipList.txt", temp.Access_Class));
        }
    }

    public static Account AccountDataLoad(string path)
    {
        string file = ReadDataFile(path);
        if (file == null) return null;

        AccountData json = JsonUtility.FromJson<AccountData>(file);
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < json.numofCharacter; i++)
        {
            list.Add(CharacterDataLoad("SaveFile/UserData/Character" + (i + 1).ToString() + ".txt"));
        }

        return new Account(json.username, json.level, json.gold, json.numofCharacter, json.current_stage, list);
    }

    public static GameObject CharacterDataLoad(string path)
    {
        string file = ReadDataFile(path);
        if (file == null) return null;

        CharacterData json = JsonUtility.FromJson<CharacterData>(file);
        GameObject prefab;
        Character character;
        List<AudioClip> audio_list2 = new List<AudioClip>();
        List<string> audio_list;

        prefab = ((GameObject)Resources.Load("Prefabs/" + json.prefabname));

        if (json.tag == "Ally")
        {
            if (prefab.GetComponent<AllyCharacter>() == null)
                prefab.AddComponent<AllyCharacter>();
            character = prefab.GetComponent<AllyCharacter>();
            audio_list = ListDataLoad<string>("SaveFile/UserData/" + json.audioclip);
            character.Access_equipments = ListDataLoad<EquipmentData>("SaveFile/UserData/" + json.eqipment); // 장비 데이터 로드
            if (!character.GetComponent(json.skillname))
                character.gameObject.AddComponent(Type.GetType(json.skillname));
            SkillDataLoad("SaveFile/SkillData/" + json.skillname + ".txt", character.GetComponent(json.skillname)); // 스킬 데이터 로드
        }
        else
        {
            if (prefab.GetComponent<EnemyCharacter>() == null)
                prefab.AddComponent<EnemyCharacter>();
            character = prefab.GetComponent<EnemyCharacter>();
            audio_list = ListDataLoad<string>("SaveFile/EnemyData/" + json.audioclip);
        }
        character.Access_prefabname = json.prefabname;
        character.Access_charactername = json.character_name;
        character.Access_atktype = json.atktype;
        character.Access_atkpower = json.atkpower;
        character.Access_atkspeed = json.atkspeed;
        character.Access_defpower = json.defpower;
        character.Access_healthpoint = json.healthpoint;
        character.Access_maxhealthpoint = json.healthpoint;
        character.Access_level = json.level;
        character.Access_exp = json.exp;
        character.Access_faceimage = json.faceimage;
        character.Access_skillimage = json.skillimage;
        character.Access_skillname = json.skillname;
        character.Access_Class = json.character_class;

        for (int i = 0; i < audio_list.Count; i++)
        {
            audio_list2.Add(Resources.Load<AudioClip>("Sound/" + audio_list[i]));
        }
        character.Access_audioclips = audio_list2;

        return character.gameObject;
    }

    public static void SkillDataLoad(string path, Component component)
    {
        string file = ReadDataFile(path);
        if (file == null) return;
        SkillData json = JsonUtility.FromJson<SkillData>(file);
        Skill temp = (Skill)component;
        temp.Access_skillcooltime = json.skill_cooltime;
        temp.Access_skillimage = json.skill_image;
        temp.Access_skillname = json.skill_name;
        temp.Access_skill_manual = json.skill_manual;
    }

    public static List<T> ListDataLoad<T>(string path)
    {
        string file = ReadDataFile(path);
        if (file == null) return null;

        List<T> json = JsonHelper.FromJson<T>(file);
        return json;
    }

    public static Stage StageDataLoad(string path)
    {
        string file = ReadDataFile(path);
        if (file == null) return null;

        StageData json = JsonUtility.FromJson<StageData>(file);

        return new Stage(json.stage_idx, json.stage_grade,
            Resources.Load<Material>("Materials/" + json.stage_texture),
            ListDataLoad<string>("SaveFile/StageData/RewardList" + json.stage_idx.ToString() + ".txt"),
            ListDataLoad<string>("SaveFile/StageData/EnemyList" + json.stage_idx.ToString() + ".txt"), json.stage_exp);
    }
}
