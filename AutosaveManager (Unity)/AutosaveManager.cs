using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


namespace Autosave
{
    // Interface for objects that can be saved and loaded
    public interface ISaveable
    {
        SaveData GetSaveData();
        void RestoreState(SaveData saveData, Action onComplete);
    }

    // Abstract base class for save data
    public abstract class SaveData
    {
        // Convert this object to a JSON string
        public abstract string ToJson();
        // Overwrite this object's fields with the values from the JSON string
        public abstract void FromJson(string json);
    }

    // Example save data class for the player
    [System.Serializable]
    public class PlayerSaveData : SaveData
    {
        public int playerHealth;
        public Vector3 playerPosition;

        public override string ToJson()
        {
            // Convert this object to a JSON string
            return JsonUtility.ToJson(this);
        }

        public override void FromJson(string json)
        {

            // Overwrite this object's fields with the values from the JSON string
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    // Event for save start
    [System.Serializable]
    public class SaveStartEvent : UnityEvent { }

    // Event for save complete
    [System.Serializable]
    public class SaveCompleteEvent : UnityEvent { }

    // Event for load start
    [System.Serializable]
    public class LoadStartEvent : UnityEvent { }

    // Event for load complete
    [System.Serializable]
    public class LoadCompleteEvent : UnityEvent { }

    // AutosaveManager class for managing saving and loading
    public class AutosaveManager : MonoBehaviour
    {
        public static bool ShouldLoadOnStart { get; private set; }
        public static bool IsSaving { get; private set; }
        public static bool IsLoading { get; private set; }

        public static AutosaveManager Instance;

        public SaveStartEvent OnSaveStart;
        public SaveCompleteEvent OnSaveComplete;
        public LoadStartEvent OnLoadStart;
        public LoadCompleteEvent OnLoadComplete;


        private Coroutine saveCoroutine;
        private Coroutine loadCoroutine;


        [Header("Loading Screen Assets")]
        [SerializeField] private GameObject loadingScreen;


        [Header("Save File Name")]
        [SerializeField] private string saveFileName = "saveData.json";
        [SerializeField] private string path;

        [Header("Autosave Config")]
        [SerializeField] bool encrypt = false;
        [SerializeField] private bool initOnAwake = false;
        

        private ISaveable[] saveables;
        [SerializeField] private GameObject[] saveableObjects;

        private void Awake()
        {
            path = Application.persistentDataPath + saveFileName;
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Multiple AutosaveManagers in the scene, destroying " + this.gameObject.name);
                Destroy(gameObject);
                return;
            }

            //DontDestroyOnLoad(gameObject);

            // Find all saveable objects in the scene if initOnAwake is toggled on
            if(initOnAwake) InitSaveables();


            //Uncomment the following lines if you want to reset the events on awake
            //OnSaveStart = new SaveStartEvent();
            //OnSaveComplete = new SaveCompleteEvent();
            //OnLoadStart = new LoadStartEvent();
            //OnLoadComplete = new LoadCompleteEvent();


            if (ShouldLoadOnStart)
            {
                Load(Application.persistentDataPath + saveFileName, encrypt);
            }
        }

        // Find all saveable objects in the scene and store them in an array
        private void InitSaveables()
        {
            MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>(true);
            List<ISaveable> saveables = new List<ISaveable>();
            List<GameObject> saveableObjects = new List<GameObject>();
            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                ISaveable saveable = monoBehaviour.gameObject.GetComponent<ISaveable>();
                if (saveable != null)
                {
                    saveables.Add(saveable);
                    saveableObjects.Add(monoBehaviour.gameObject);
                }
            }
            this.saveableObjects = saveableObjects.ToArray();
            this.saveables = saveables.ToArray();
        }

        [ContextMenu("Update ISaveable List")]
        public void UpdateList()
        {
            InitSaveables();
        }


        [ContextMenu("Save Data")]
        public void Save()
        {
            Save(Application.persistentDataPath + saveFileName, encrypt);
        }

        // Save the game state to a file
        public void Save(string filePath, bool encrypt)
        {
            if (saveCoroutine == null)
            {
                saveCoroutine = StartCoroutine(SaveCoroutine(filePath, encrypt));
            }
            else
            {
                Debug.LogError("Save already in progress");
                return;
            }


        }
        
        private IEnumerator SaveCoroutine(string filePath, bool encrypt)
        {
            IsSaving = true;
            OnSaveStart.Invoke();

            // Clear the file before saving new data
            File.WriteAllText(filePath, "");

            foreach (ISaveable saveable in saveables)
            {
                SaveData saveData = saveable.GetSaveData();
                string json = saveData.ToJson();
                if (encrypt)
                {
                    json = Encrypt(json);
                }
                // Append the JSON data to the file with a delimiter
                File.AppendAllText(filePath, json + Environment.NewLine);
                yield return null; // Wait for one frame to avoid blocking the main thread
            }

            IsSaving = false;
            saveCoroutine = null;
            OnSaveComplete.Invoke();
        }




        [ContextMenu("Load Data")]
        public void Load()
        {
            Load(Application.persistentDataPath + saveFileName, encrypt);
        }


        // Load the game state from a file
        public void Load(string filePath, bool decrypt)
        {
            if (loadCoroutine == null)
            {
                loadCoroutine = StartCoroutine(LoadCoroutine(filePath, decrypt));
            }
            else
            {
                Debug.LogError("Load already in progress");
                return;
            }

        }

        private IEnumerator LoadCoroutine(string filePath, bool decrypt)
        {
            // Set the loading flag and invoke the load start event
            IsLoading = true;
            OnLoadStart.Invoke();
            if (loadingScreen != null) loadingScreen.SetActive(true);

            // Wait for one frame to avoid blocking the main thread
            yield return null;

            // Load the save data from the file
            string[] jsonLines = File.ReadAllLines(filePath);

            // Decrypt the data
            if (decrypt)
            {
                for (int i = 0; i < jsonLines.Length; i++)
                {
                    jsonLines[i] = Decrypt(jsonLines[i]);
                }
            }

            // Restore the save data for each saveable object
            int numSaveables = saveables.Length;
            int numSaveablesRestored = 0;

            for (int i = 0; i < saveables.Length; i++)
            {
                ISaveable saveable = saveables[i];
                // Create a new instance of SaveData for each saveable object
                SaveData saveData = saveable.GetSaveData();
                saveData.FromJson(jsonLines[i]);

                // Use a local variable to capture the correct saveable instance
                ISaveable currentSaveable = saveable;

                // Restore the state asynchronously
                currentSaveable.RestoreState(saveData, () =>
                {
                    // Callback invoked when restore state is complete
                    numSaveablesRestored++;
                });

                yield return null; // Wait for one frame to avoid blocking the main thread
            }

            // Wait for all saveables to complete restoring state before finishing load
            while (true)
            {
                if (numSaveablesRestored == numSaveables)
                {
                    // All saveables have completed restoring state
                    IsLoading = false;
                    OnLoadComplete.Invoke();
                    if (loadingScreen != null) loadingScreen.SetActive(false);
                    yield return null;
                    yield break;
                }
                else
                {
                    yield return null; // Wait for one frame to avoid blocking the main thread
                }
            }
        }

        // Encrypt the data
        private string Encrypt(string data)
        {
            try
            {
                // Example encryption, replace with your encryption method
                byte[] encryptedData = Encoding.UTF8.GetBytes(data);
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception e)
            {
                Debug.LogError("Encryption failed: " + e.Message);
                return null;
            }
        }

        // Decrypt the data
        private string Decrypt(string data)
        {
            try
            {
                // Example decryption, replace with your decryption method
                byte[] decryptedData = Convert.FromBase64String(data);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception e)
            {
                Debug.LogError("Decryption failed: " + e.Message);
                return null;
            }
        }
    }
}

