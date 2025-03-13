using Scriptable_Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject FinishLineObject;
    private LevelConfig _levelConfig;
    [Inject]
    public void Construct(LevelConfig levelConfig)
    {
        _levelConfig = levelConfig;
    }
    void Start()
    {
        float startPosY = FinishLineObject.GetComponent<SpriteRenderer>().size.x;
        FinishLineObject.transform.position = new Vector3(_levelConfig.LevelDistance, startPosY);
        FinishLineObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
