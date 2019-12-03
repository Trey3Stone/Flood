using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurretBehaviour : MonoBehaviour
{
    private float speed = 100.0f;
    private float amount = 0.05f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time) * 40 + 180;
        //Vector3 temp = transform.position;

        //temp.y = Mathf.Sin(Time.time * speed) * amount;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.position = temp;
    }
}
