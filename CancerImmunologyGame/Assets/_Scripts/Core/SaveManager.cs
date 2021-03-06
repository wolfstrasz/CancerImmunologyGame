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
            [SerializeField]
            private string saveDirectory = "GameData";
            [SerializeField]
            private bool debugToJason = true;

            // Save data 
            public bool ClearAllSaveData()
            {
                if (Directory.Exists(saveDirectory))
                {
                    DirectoryInfo directory = new DirectoryInfo(saveDirectory);
                    directory.Delete(true);
                    Directory.CreateDirectory(saveDirectory);
                    return true;
                }
                else
                {
                    Debug.Log("Save Manager: Save directory file not found in " + saveDirectory);
                    return false;
                }
            }

            public void ClearSaveData<SaveDataType>()
			{
                if (Directory.Exists(saveDirectory))
                {
                    string filePath = saveDirectory + Utils.RemoveNamespacesFromAssemblyType(typeof(SaveDataType).ToString()) + ".savefile";
                    if (File.Exists(filePath))
					{
                        File.Delete(filePath);
					} else
					{
                        Debug.Log("Save Manager: File (" + filePath + ") cannot be deleted as it does not exist.");
					}
                }
                else
                {
                    Debug.Log("Save Manager: Save directory file not found in " + saveDirectory);
                }
            }

            /// <summary>
            /// The save data type must be a serializable data!
            /// </summary>
            /// <typeparam name="SaveDataType"></typeparam>
            /// <param name="saveData"></param>
            /// <returns></returns>
            public void SaveData<SaveDataType>(SaveDataType saveData) where SaveDataType : SaveableObject
            {

                if (!Directory.Exists(saveDirectory))
				{
                    try
                    {
                        var folder = Directory.CreateDirectory(saveDirectory);
                    }
                    catch
                    {
                        Debug.LogError("Save Manager: Did not manage to create save directory!");
                        return;
                    }
                }

                DirectoryInfo directory = new DirectoryInfo(saveDirectory);
    
                FileStream file = null;
                try
                {
                    file = File.Create(saveDirectory + "\\" + Utils.RemoveNamespacesFromAssemblyType(typeof(SaveDataType).ToString()) + ".savefile");
                }
                catch
                {
                    file = null;
                    Debug.LogWarning("Save Manager: Did not manage to save because file was not able to be created");
                    return;
                }

                if (file != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, saveData);

                    if (debugToJason)
                    {
                        string jason = JsonUtility.ToJson(saveData);
                        Debug.Log("Save Manager: Saving as JSON: " + jason);
                    }
                    file.Close();
                }
            }


            // Load data
            public LoadDataType LoadData<LoadDataType>() where LoadDataType : SaveableObject
            {
                if (!Directory.Exists(saveDirectory))
                {
                    Debug.Log("Save Manager: Load directory file not found in " + saveDirectory);
                    return null;
                }

                Debug.Log("Save Manager: Loading from directory: " + saveDirectory);
                FileInfo directory = new FileInfo(saveDirectory);

                string filePath = saveDirectory + "\\" +  Utils.RemoveNamespacesFromAssemblyType(typeof(LoadDataType).ToString()) + ".savefile";

                if (!File.Exists(filePath))
                {
                    Debug.Log("Save Manager: Save file not found in " + filePath);
                    return null;
                }
                else
                {
                    FileStream file = File.Open(filePath, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    LoadDataType storeDataContainer = (LoadDataType)bf.Deserialize(file);

                    file.Close();               //Always make sure to close the file stream
                    return storeDataContainer;
                }
            }


          

        }

        [System.Serializable]
        public abstract class SaveableObject
		{

		}
    }
}