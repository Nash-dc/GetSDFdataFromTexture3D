using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

/// <summary>
/// JsonUtility
/// </summary>
static public class JsonSerialization
{
    /// <summary>
    /// Archive number
    /// </summary>
    public static uint serialNumber;

    static string FilePath(ISerializableData data)
    {
        if (string.IsNullOrEmpty(data.FolderName))
        {
            return Application.persistentDataPath + "/" + data.FileName + ".json";
        }
        return Application.persistentDataPath + "/" + data.FolderName + "/" + data.FileName + ".json";
    }
    static string FolderPath(ISerializableData data)
    {
        if (string.IsNullOrEmpty(data.FolderName))
        {
            return Application.persistentDataPath;
        }
        return Application.persistentDataPath + "/" + data.FolderName;
    }


    /// <summary>
    /// Serialize
    /// </summary>
    /// <param name="data"></param>
    public static void Serialize(ISerializableData data)
    {
        string path = FolderPath(data);  
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = FilePath(data);
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        Debug.Log("json path: " + path);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Deserilization
    /// </summary>
    /// <param name="data"></param>
    public static bool Deserialize(ISerializableData data)
    {
        string path = FilePath(data);
        Debug.Log("Store path：" + path);
        try
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
        }
        catch (FileNotFoundException e)
        {
            Serialize(data);
            Debug.Log(e.Message + "at" + path + "\nstored current data");
            return false;
        }

        string json = File.ReadAllText(path);
        //FromJsonOverwrite is a mathod to rewrite data 
        //FromJson is a method to creat a new object
        JsonUtility.FromJsonOverwrite(json, data);

        return true;
    }

    public static bool Delete(ISerializableData data)
    {
        string path = FilePath(data);

        if (!File.Exists(path))
        {
            Debug.Log("Delete failed,the file does not exist：" + path);
            return false;
        }

        File.Delete(path);
        return true;
    }

    public static string StringSerialize(object data)
    {
        return JsonUtility.ToJson(data, false);
    }
    public static void StringDeserialize(string str, object obj)
    {
        JsonUtility.FromJsonOverwrite(str, obj);
    }
}

/// <summary>
/// Archiveable Type Interface
/// </summary>
public interface ISerializableData
{
    string FileName { get; }
    string FolderName { get; }
}