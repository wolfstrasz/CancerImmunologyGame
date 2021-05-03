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
            private string buildString = "build4";
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
                    Debug.Log("Save directory file not found in " + saveDirectory);
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
                        Debug.Log("File (" + filePath + ") cannot be deleted as it does not exist.");
					}
                }
                else
                {
                    Debug.Log("Save directory file not found in " + saveDirectory);
                }
            }

            /// <summary>
            /// The save data type must be a serializable data!
            /// </summary>
            /// <typeparam name="SaveDataType"></typeparam>
            /// <param name="saveData"></param>
            /// <returns></returns>
            public void SaveData<SaveDataType>(SaveDataType saveData) where SaveDataType : SavableObject
            {

                if (!Directory.Exists(saveDirectory))
				{
                    try
                    {
                        var folder = Directory.CreateDirectory(saveDirectory);
                    }
                    catch
                    {
                        Debug.LogError("Did not manage to create save directory!");
                        return;
                    }
                }

                FileStream file = null;
                try
                {
                    file = File.Create(saveDirectory + Utils.RemoveNamespacesFromAssemblyType(typeof(SaveDataType).ToString()) + ".savefile");
                }
                catch
                {
                    file = null;
                    Debug.LogWarning("Did not manage to save because file was not able to be created");
                    return;
                }

                if (file != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, saveData);

                    if (debugToJason)
                    {
                        string jason = JsonUtility.ToJson(saveData);
                        Debug.Log("Saving as JSON: " + jason);
                    }
                    file.Close();
                }
            }


            // Load data
            public LoadDataType LoadData<LoadDataType>() where LoadDataType : SavableObject
            {
                if (!Directory.Exists(saveDirectory))
                {
                    Debug.Log("Load directory file not found in " + saveDirectory);

                    return null;
                }

                Debug.Log("Save Manager: Loading from directory: " + saveDirectory);
                string filePath = saveDirectory + Utils.RemoveNamespacesFromAssemblyType(typeof(LoadDataType).ToString()) + ".savefile";

                if (!File.Exists(filePath))
                {
                    Debug.Log("Save file not found in " + filePath);
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
        public abstract class SavableObject
		{

		}
    }
}