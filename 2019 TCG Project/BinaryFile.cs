using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BinaryFile
{
    public static void BinarySerialize<T>(T t, string filepath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filepath, FileMode.Create);
        formatter.Serialize(stream, t);
        stream.Close();
    }

    public static T BinaryDeserialize<T>(string filepath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filepath, FileMode.Open);
        T data = (T)formatter.Deserialize(stream);
        stream.Close();
        return data;
    }

    public static List<List<object>> SettingSerializeData(List<List<object>> itemList) // 바이너리 파일 저장 세팅
    {
        List<List<object>> tempList = new List<List<object>>(itemList);

        for (int i = 0; i < itemList.Count; i++)
        {
            tempList[i] = itemList[i].GetRange(0, itemList[i].Count);

            if (tempList[i][0].Equals(1))
            {
                tempList[i][2] = tempList[i][1].Equals(0) ?
                    string.Format(itemList[i][2].ToString().Split(' ')[0] + ".prefab") :
                    string.Format(itemList[i][2].ToString().Split(' ')[0] + ".image");
            }
        }
        return tempList;
    }

    public static List<List<object>> SettingDeserializeData(List<List<object>> itemList) // 바이너리 파일 로드 세팅
    {
        List<List<object>> tempList = new List<List<object>>(itemList);

        for (int i = 0; i < itemList.Count; i++)
        {
            tempList[i] = itemList[i].GetRange(0, itemList[i].Count);

            if (tempList[i][0].Equals(1))
            {
                if (tempList[i][1].Equals(0))
                {
                    string prefabName = itemList[i][2].ToString().Substring(0, itemList[i][2].ToString().IndexOf(".prefab"));
                    tempList[i][2] = Resources.Load<GameObject>(string.Format("prefabs/StagePrefabData/" + prefabName));
                }
                else if (tempList[i][1].Equals(1))
                {
                    string imageName = itemList[i][2].ToString().Substring(0, itemList[i][2].ToString().IndexOf(".image"));
                    tempList[i][2] = Resources.Load<Sprite>(string.Format("images/StageImageData/" + imageName));
                }
            }
        }
        return tempList;
    }
}
