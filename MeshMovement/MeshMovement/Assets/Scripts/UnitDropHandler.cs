using Dummiesman;
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
            string objPath = string.Empty;
            string error = string.Empty;
            GameObject loadedObject;
            GameObject gun;
            Vector3 pos = new Vector3(1085, 923, 475);

            loadedObject = new OBJLoader().Load("C:/Users/samspam/Flood/MeshMovement/MeshMovement/Assets/Models/Turret.obj");
            loadedObject.transform.position = pos;
            loadedObject.transform.rotation = transform.rotation = Quaternion.AngleAxis(180, Vector3.up); ;

            gun = GameObject.Find("Turret/Gun_LOD0");
            gun.AddComponent<MachineGunTurretBehaviour>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
