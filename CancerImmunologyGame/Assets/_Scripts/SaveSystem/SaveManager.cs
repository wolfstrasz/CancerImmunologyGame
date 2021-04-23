using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ImmunotherapyGame.Core;


namespace ImmunotherapyGame
{
    namespace SaveSystem
    {

        public class SaveManager : Singleton<SaveManager>
        {

            public static string savePath = "";
            public static bool debugToJason = true;

            // FIX IT TO WORK FOR WEBGL AS YOU NEED TO USE PLAYER PREFS ONLY


            // Save data 
            public bool ClearAllSaveData()
			{
                if (File.Exists(savePath))
                {
                    DirectoryInfo directory = new DirectoryInfo(savePath);
                    directory.Delete(true);
                    Directory.CreateDirectory(savePath);
                    return true;
                }
                else
                {
                    Debug.Log("Save file not found in " + savePath);
                    return false;
                }
            }
            /// <summary>
            /// The save data type must be a serializable data!
            /// </summary>
            /// <typeparam name="SaveDataType"></typeparam>
            /// <param name="saveData"></param>
            /// <returns></returns>
            public bool SaveData<SaveDataType>(ref SaveDataType saveData) where SaveDataType : ISavable
            {

                FileStream file = File.Create(savePath + Utils.RemoveNamespacesFromAssemblyType(typeof(SaveDataType).ToString()) + ".savefile");

                if (file == null)
                {
                    return false;
                }
                else
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, saveData);

                    if (debugToJason)
                    {
                        string jason = JsonUtility.ToJson(saveData);
                        Debug.Log("Saving as JSON: " + jason);
                    }

                    return true;
                }
            }

            public void AsynchSaveData<SaveDataType>(ref SaveDataType saveData, OnSaveCallback callback) where SaveDataType : ISavable
            {
                StartCoroutine(AsyncSaveData(saveData, callback));
            }

            private IEnumerator AsyncSaveData<SaveDataType>(SaveDataType saveData, OnSaveCallback callback ) where SaveDataType : ISavable
            {
                FileStream file = File.Create(savePath + Utils.RemoveNamespacesFromAssemblyType(typeof(SaveDataType).ToString()) + ".savefile");

                if (file == null)
                {
                    callback(false);
                }
                else
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, saveData);

                    if (debugToJason)
                    {
                        string jason = JsonUtility.ToJson(saveData);
                        Debug.Log("Saving as JSON: " + jason);
                    }

                    callback(true);
                }
                yield return null;
            }

            // Load data
            public bool LoadData<LoadDataType>(ref LoadDataType storeDataContainer) where LoadDataType : ISavable
            {
                string filePath = savePath + Utils.RemoveNamespacesFromAssemblyType(typeof(LoadDataType).ToString()) + ".savefile";
                if (!File.Exists(filePath))
                {
                    Debug.Log("Save file not found in " + filePath);
                    return false;
                }
                else 
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(filePath, FileMode.Open);
                    storeDataContainer = (LoadDataType)bf.Deserialize(file);

                    file.Close();               //Always make sure to close the file stream

                    return true;
                }
            }


            public void LoadDataAsync<LoadDataType>(ref LoadDataType storeDataContainer, OnLoadCallback callback) where LoadDataType : ISavable
            {
                StartCoroutine(LoadDataAsync(storeDataContainer, callback));
			}


            private IEnumerator LoadDataAsync<LoadDataType>(LoadDataType storeDataContainer, OnLoadCallback callback) where LoadDataType : ISavable
            {
                string filePath = savePath + Utils.RemoveNamespacesFromAssemblyType(typeof(LoadDataType).ToString()) + ".savefile";
                if (!File.Exists(filePath))
                {
                    Debug.Log("Save file not found in " + filePath);
                    callback(false);
                }
                else
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(filePath, FileMode.Open);
                    storeDataContainer = (LoadDataType)bf.Deserialize(file);

                    file.Close();               //Always make sure to close the file stream
                    callback(true);
                }

                yield return null;

            }

        }



        public delegate void OnSaveCallback(bool success);
        public delegate void OnLoadCallback(bool success);

        public interface ISavable { }
    }
}