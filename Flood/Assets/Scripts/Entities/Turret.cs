using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{

	[SerializeField]
	private Transform Base;

	[SerializeField]
	private Transform Gun;

	[SerializeField]
	private LineRenderer lineRenderer;

	public new static int HexSize = 1;

	public const int RANGE = 10;
	public const float FIREDELAY = 0.5f;

	private float lastFire = 0;

	private float pVel = 0;
	private float yVel = 0;

	private Vector3 shootAng;

	// Start is called before the first frame update
	void Start()
    {
		MaxHealth = 20;
		Health = MaxHealth;

		shootAng = transform.rotation.eulerAngles;
		lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time - lastFire >= 0.05f) {
			lineRenderer.enabled = false;
		}

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
				if (GameManager.Self.Core.Withdraw(0.1f)) {
					FluidManager.Self.AddDiff(minPos, 2.1f, -1f);
					shootAng = Quaternion.LookRotation(HexHelper.HexToWorld(minPos, WorldManager.Self.World) - Gun.position).eulerAngles;
					lineRenderer.enabled = true;
					
				}
			}
		}

		//Gun.localEulerAngles = new Vector3(Mathf.SmoothDampAngle(Gun.localEulerAngles.x, shootAng.x - 8, ref pVel, 0.1f), 0, 0);
		//Base.localEulerAngles = new Vector3(0, Mathf.SmoothDampAngle(Gun.localEulerAngles.y, shootAng.y, ref yVel, 0.1f), 0);

		Gun.localEulerAngles = new Vector3(Mathf.Min(30, Mathf.MoveTowardsAngle(Gun.localEulerAngles.x, shootAng.x, 360 * Time.deltaTime)), 0, 0);
		Base.localEulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(Base.localEulerAngles.y, shootAng.y, 360 * Time.deltaTime), 0);

		CheckDamage();
	}

}
