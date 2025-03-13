
using QFSW.QC;
using SavingSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomConsole : MonoBehaviour
{

    [Zenject.Inject]
    private void Construct(Storage storage)
    {
        Storage = storage;
    }
    public Storage Storage { get; private set; }
    [Command(aliasOverride: "delete-save")]
    public void DeleteSave()
    {
        PlayerPrefs.SetInt("isTutorialPartOneShowed", 0);
        PlayerPrefs.SetInt("isTutorialPartTwoShowed", 0);
        PlayerPrefs.DeleteKey("IsTutorialShowed");
        PlayerPrefs.DeleteKey("Language");
        
        Storage.InitializeDefaultStorageData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Command(aliasOverride: "add-money", description: "Adds money to the player")]
    public void AddMoney(int money)
    {
        Storage.StorageData.Money += money;
    }
    [Command(aliasOverride:"גפ")]
    [Command(aliasOverride: "da", description: "Delete save and Add money")]
    public void DeleteSaveAndAddMoney()
    {
        DeleteSave();
        AddMoney(99999);
    }
    
    [Command(aliasOverride:"ol")]
    [Command(aliasOverride: "open-levels", description: "Open all levels")]
    public void OpenLevels()
    {
        foreach (var storageDataLevel in Storage.StorageData.Levels)
        {
            storageDataLevel.PassedDistance = Mathf.Infinity;
        }
        Storage.StorageData.LastLevel = "Level 3";
    }
}
