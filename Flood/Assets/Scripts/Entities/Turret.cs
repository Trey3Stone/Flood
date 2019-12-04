using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{

	public new static int HexSize = 1;

	public const int RANGE = 10;
	public const float FIREDELAY = 0.5f;

	private float lastFire = 0;

	private Vector3 shootPos;

	// Start is called before the first frame update
	void Start()
    {
		MaxHealth = 20;
		Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time - lastFire >= FIREDELAY) {
			lastFire = Time.time;

			Vector2Int minPos = Vector2Int.zero;
			float minDistSqr = float.MaxValue;

			for (int ix = Mathf.Max(0, HexPos.x - RANGE); ix <= Mathf.Min(WorldManager.Self.World.GridSize - 1, HexPos.x + RANGE); ix++) {
				for (int iy = Mathf.Max(0, HexPos.y - RANGE); iy <= Mathf.Min(WorldManager.Self.World.GridSize - 1, HexPos.y + RANGE); iy++) {
					Vector2Int pos = new Vector2Int(ix, iy);
					float distSqr = (pos - HexPos).sqrMagnitude;
					if (distSqr >= minDistSqr)
						continue;
				
					float val = FluidManager.Self.GetFluid(pos.x, pos.y);
					if (val > 0) {
						minDistSqr = distSqr;
						minPos = pos;
					}

				}
			}

			if (minDistSqr != float.MaxValue) {
				FluidManager.Self.AddDiff(minPos, 2.1f, -1f);
				shootPos = HexHelper.HexToWorld(minPos, WorldManager.Self.World);
			}
		}

		CheckDamage();
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, shootPos);

	}

}
