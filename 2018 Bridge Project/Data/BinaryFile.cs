using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
}
