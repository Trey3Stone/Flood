using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Entity
{
	// Start is called before the first frame update

	float Capacity = 500;
	public float Contents { get; private set; }


	void Start() {
		MaxHealth = 50;
		Health = MaxHealth;
	}

	// Update is called once per frame
	void Update()
    {


		CheckDamage();
	}

	public bool Withdraw(float amount) {
		if (amount <= Contents) {
			Contents -= amount;
			return true;
		} else {
			return false;
		}
	}

	public void Deposit(float amount) {
		Contents = Mathf.Min(Contents + amount, Capacity);
	}

	protected override void OnDeath() {
		WorldManager.Self.World.TileMap[HexPos] = false;
		GameObject.Destroy(gameObject);

		// GAME OVER
	}

}
