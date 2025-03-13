using System.Collections;
using System.Collections.Generic;
using SavingSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AddMoneyCheat : MonoBehaviour
{

    [SerializeField] private Button _button;
    [Inject] private Storage _storage;
    
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _storage.StorageData.Money = 10000;
            gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
