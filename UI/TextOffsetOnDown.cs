using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using NaughtyAttributes;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextOffsetOnDown : ValidatedMonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Self] private Button _button;
    [SerializeField, Child] private TextMeshProUGUI _text;
    [SerializeField] private Vector2 _offset;

    public void OnPointerDown(PointerEventData eventData) => ShiftText(_offset);
    public void OnPointerUp(PointerEventData eventData) => ShiftText(-_offset);
    private void ShiftText(Vector2 offset) => _text.rectTransform.anchoredPosition += offset;
}