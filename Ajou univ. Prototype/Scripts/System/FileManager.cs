using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class JsonHelper // 리스트형 데이터 저장용 wrapper class, 직접 참조할 필요없이 아래 FileManager 클래스를 이용하면 됨.
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

public static class FileManager // static형이기 때문에 구체화된 객체나 오브젝트를 생성할 필요없이 바로 사용할 수 있음.
                                // 제네릭 (<T>) 메소드로 구현했기 때문에 호출시에 알맞은 형만 지정해서 사용하면 됨.
                                // 저장 or 불러오기를 하려는 데이터가 단일형이냐 List형이냐에 따라 선별적으로 메소드를 사용 해야함.
{
    static string ReadDataFile(string path)
    {
        string str;
        TextAsset txt =  Resources.Load(path) as TextAsset;
        str = txt.text;
        if (str == null) return null;
        return str;
    }

    public static void DataGenerate<T>(string path, T data) // 단일 자료형 데이터를 생성하는 메소드
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(path, json);
    }

    public static void ListDataGenerate<T>(string path, List<T> data) // 리스트 자료형 데이터를 생성하는 메소드
    {
        string json = JsonHelper.ToJson<T>(data, prettyPrint: true);
        File.WriteAllText(path, json);
    }

    public static T DataLoad<T>(string path) // 단일 자료형 데이터를 load 하는 메소드
    {
        string file = ReadDataFile(path);
        T json = JsonUtility.FromJson<T>(file);
        return json;
    }
    
    public static List<T> ListDataLoad<T>(string path) // 리스트 자료형 데이터를 load 하는 메소드
    {
        string file = ReadDataFile(path);
        if (file == null) return null;
        List<T> json = JsonHelper.FromJson<T>(file);
        return json;
    }

    public static List<T> ConvertDataToList<T>(string file)
    {
        if (file == null) return null;
        List<T> json = JsonHelper.FromJson<T>(file);
        return json;
    }
}
