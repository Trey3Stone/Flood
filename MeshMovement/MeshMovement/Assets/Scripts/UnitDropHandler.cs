using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDropHandler : MonoBehaviour, IDropHandler 
{
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform hotbar = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(hotbar, Input.mousePosition))
        {
            // Drop/Create object
        }
    }
}
