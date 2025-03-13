using DG.Tweening;
using Plane;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedBordersOnHit : MonoBehaviour
{
    [SerializeField] private Material _redBordersMaterial;
    [SerializeField] private float _fadeOutTime;
    private PlaneView _plane;
    public static RedBordersOnHit Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        _redBordersMaterial.DOFade(0, 0);

    }
    [Zenject.Inject]
    public void Construct(Plane.PlaneView plane, Plane.PlaneController planeController)
    {
        _plane = plane;
        _plane.OnSlowed += ActivateRedBorders;
    }
    public void ActivateRedBorders(float fix)
    {
        ShowAndHideRedBorders();
    }
    private void ShowAndHideRedBorders()
    {
        _redBordersMaterial.DOFade(1, 0);
        _redBordersMaterial.DOFade(0, _fadeOutTime);
    }
    public void ShowBordersInWind()
    {
        _redBordersMaterial.DOFade(1, 0.1f);
    }
    public void HideBordersInWind()
    {
        _redBordersMaterial.DOFade(0, _fadeOutTime);
    }
    private void OnDisable()
    {
        _plane.OnSlowed -= ActivateRedBorders;
    }
}
