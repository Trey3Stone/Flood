using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public GameObject Prefab;

	[SerializeField]
	public Vector2Int HexPos;
	public static int HexSize;

	public float MaxHealth;
	public float Health;

	//public GameObject platform = null;

	private void Awake() {
		print(this);
		HexPos = HexHelper.WorldToHex(transform.position, WorldManager.Self.World.Size);


		DerivedAwake();
	}


	protected virtual void DerivedAwake() { }

	protected virtual void CheckDamage() {
		float vol = FluidManager.Self.GetFluid(HexPos.x, HexPos.y);
		if (vol > 0) {
			print(vol);
			Health -= vol;
			if (Health <= 0) {
				OnDeath();
				FluidManager.Self.AddDiff(HexPos, 0.5f, -(vol + Health));
			} else {
				FluidManager.Self.AddDiff(HexPos, 0.5f, -vol);
			}
		}
	}

	protected virtual void OnDeath() {
		WorldManager.Self.World.TileMap[HexPos] = false;
		GameObject.Destroy(gameObject);
	}

}
