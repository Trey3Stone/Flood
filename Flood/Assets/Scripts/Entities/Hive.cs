﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Entity
{
	const float EMISSION_RATE = 2.0f; // Fluid / Second

	public new static int HexSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		FluidManager.Self.AddDiff(HexPos, 0.1f, (EMISSION_RATE + FluidManager.Self.GetFluid(HexPos.x, HexPos.y) / 10) * Time.deltaTime);
    }

	protected override void CheckDamage() {
		
	}
}
