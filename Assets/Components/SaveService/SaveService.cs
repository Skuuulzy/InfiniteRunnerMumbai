using System;
using System.IO;
using UnityEngine;

namespace Components.SaveService
{
    public static class SaveService
    {
        private const string FILE_NAME = "InfiniteDiscountSave.json";
        private static string FilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

        public static void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(FilePath, json);

            Debug.Log("Data successfully saved at: " + FilePath);
        }
        
        public static SaveData Load()
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Something went wrong with data loading: " + e);
                return null;
            }
        }
    }
}