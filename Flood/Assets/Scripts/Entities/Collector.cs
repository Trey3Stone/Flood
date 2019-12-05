using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Entity {
	// Start is called before the first frame update

	const float COLLECTION_RATE = 0.4f;


	void Start() {
		MaxHealth = 10;
		Health = MaxHealth;
	}

	// Update is called once per frame
	void Update()
    {
		GameManager.Self.Core?.Deposit(COLLECTION_RATE * Time.deltaTime);

		CheckDamage();
	}
}
