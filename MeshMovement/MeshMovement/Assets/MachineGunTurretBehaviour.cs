using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurretBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time) * 40;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
