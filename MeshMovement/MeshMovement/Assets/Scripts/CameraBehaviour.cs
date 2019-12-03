using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private float scrollSpeed = 1.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(0, scrollAmount * scrollSpeed, scrollAmount * scrollSpeed, Space.World);   
    }
}
