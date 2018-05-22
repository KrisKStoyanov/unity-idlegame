using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController : MonoBehaviour {

    private DataManager dc_dataManager;

    void OnEnable()
    {
        SetInitialReferences();
        if (dc_dataManager != null)
        {
            dc_dataManager.EventSaveData += SaveData;
            dc_dataManager.EventLoadData += LoadData;
            dc_dataManager.EventResetData += ResetData;
        }
    }

    void OnDisable()
    {
        dc_dataManager.EventSaveData -= SaveData;
        dc_dataManager.EventLoadData -= LoadData;
        dc_dataManager.EventResetData -= ResetData;
    }

    private void Start()
    {
        if (dc_dataManager == null)
        {
            SetInitialReferences();
            dc_dataManager.EventSaveData += SaveData;
            dc_dataManager.EventLoadData += LoadData;
            dc_dataManager.EventResetData += ResetData;
        }
    }

    private void SetInitialReferences()
    {
        if (dc_dataManager == null)
        {
            dc_dataManager = GameMaster.gm_dataManager;
        }
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
        DataStorage data = new DataStorage();

        data.storage_cash = GameMaster.instance.GetCash();

        data.storage_idleCash = GameMaster.instance.GetIdleCash();
        data.storage_superCash = GameMaster.instance.GetSuperCash();

        formatter.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat")) // Checks to see if the file exists
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open); // Same as in SaveData

            DataStorage data = (DataStorage)formatter.Deserialize(file); // The data that the formatter is reading from the file must be cast as being a certain type to preserve data types. In this case it will be of type DataForStore, the class below
            file.Close();

            // Sets all of the local variables for use in the game to those stored in the binary file
            GameMaster.instance.SetCash(data.storage_cash);

            GameMaster.instance.SetIdleCash(data.storage_idleCash);
            GameMaster.instance.SetSuperCash(data.storage_superCash);
        }
    }

    public void ResetData()
    {
        BinaryFormatter formatter = new BinaryFormatter(); // Creates a new BinaryFormatter which is needed to 'encode' and 'decode' the binary file the data is to be stored in

        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat"); // This is to save the file to unity's self created data path which exists on most platforms such as windows and android

        DataStorage data = new DataStorage(); // Initialises a brand new instance of a DataForStore class

        // Sets all of the variables to be saved to those currently stored locally
        data.storage_cash = 10;
        data.storage_idleCash = 0;
        data.storage_superCash = 0;

        //data.storage_collector.ResetProgress();
        //data.storage_transporter.ResetProgress();

        //for (int mineshaftId = 0; mineshaftId < data.storage_mineshafts.Count; mineshaftId++)
        //{
        //    data.storage_mineshafts[mineshaftId].ResetProgress();
        //}
        //data.storage_mineshafts.Clear();


        formatter.Serialize(file, data); // The data must be serialised to be saved
        file.Close();
        LoadData();
    }
}

[Serializable]
class DataStorage
{
    public float storage_cash;
    public float storage_idleCash;
    public int storage_superCash;
}