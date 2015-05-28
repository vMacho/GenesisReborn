using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static List<GameInfo> savedGames = new List<GameInfo>();
    public static GameInfo checkpoint = new GameInfo();

    public static void SaveCheckPoint()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + Application.loadedLevelName);
        }

        checkpoint = GameInfo.current;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd");
        bf.Serialize(file, SaveLoad.checkpoint);
        file.Close();

        Debug.Log("Guardado checkpoint " + Application.persistentDataPath + "/" + Application.loadedLevelName);
    }

    public static void LoadCheckPoint()
    {
        if (File.Exists(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd", FileMode.Open);
            SaveLoad.checkpoint = (GameInfo)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void ResetCheckPoint()
    {
        if (File.Exists(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd"))
        {
            File.Delete(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd");
        }
    }

    public static void SaveGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + Application.loadedLevelName + "/checkpoint.gd"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + Application.loadedLevelName);
        }

        savedGames.Add(GameInfo.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + Application.loadedLevelName + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void LoadGame( string level )
    {
        SaveLoad.savedGames.Clear();

        if (File.Exists(Application.persistentDataPath + "/" + level + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + level + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<GameInfo>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void DeleteGame(string level)
    {
        if (File.Exists(Application.persistentDataPath + "/" + level + "/savedGames.gd"))
        {
            File.Delete(Application.persistentDataPath + "/" + level + "/savedGames.gd");
        }        
    }
}