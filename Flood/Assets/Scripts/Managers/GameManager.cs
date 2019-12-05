using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public const int SIZE = 127; // Must be odd. Ideally power of 2 - 1

	public bool Paused { get; set; } = false;
	protected override void DerivedAwake() {
		DontDestroyOnLoad(gameObject);

		Debug.Assert(SIZE % 2 == 1, "Even-row offset hex arrays work best with odd size-length hexagons");
	}

	public Core Core { get; private set; } = null;

	[SerializeField]
	private GameObject UnitMenu;

	// Start is called before the first frame update
	void Start()
    {
		WorldManager.Self.Create(SIZE);
		FluidManager.Self.Start(SIZE); // Must occur after WorldManager start

		Application.targetFrameRate = 120;

    }

	// Update is called once per frame
	void Update() {
		//if (Input.GetMouseButton(0) != Input.GetMouseButton(1)) {

		if (Core != null) {
			if (Input.GetMouseButtonDown(0)) {

				//float weight = 0.1f * (Input.GetMouseButton(0) ? 1 : -1);

				//print("Mouse Down");
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				//Ray ray = new Ray(new Vector3(0, 100, 0), Vector3.down);

				//print(ray);

				if (Physics.Raycast(ray, out RaycastHit hit)) {
					//print("Mouse Hit: " + hit.point);
					//FluidManager.Self.AddDiff(hit.point, 4, weight);

					Vector2Int gridPos = HexHelper.WorldToHex(hit.point, WorldManager.Self.World.Size);

					//Vector3 pos = MeshHelper.HexToWorld(2 * SIZE + 1, WorldManager.Self.World.Size, gridPos.x, gridPos.y);
					//pos.y = WorldManager.Self.World.HeightGrid[gridPos];

					//testCursor.transform.position = pos;


					if (WorldManager.Self.CanPlace(Hive.HexSize, gridPos)) {
						if (Core.Withdraw(10)) {
							WorldManager.Self.PlaceEnt(Resources.Load<GameObject>("Prefabs/Collector"), gridPos);
						}
					}


				}

			}

			if (Input.GetMouseButtonDown(1)) {
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit)) {
					Vector2Int gridPos = HexHelper.WorldToHex(hit.point, WorldManager.Self.World.Size);

					if (WorldManager.Self.CanPlace(Hive.HexSize, gridPos)) {
						if (Core.Withdraw(20)) {
							WorldManager.Self.PlaceEnt(Resources.Load<GameObject>("Prefabs/Turret"), gridPos);
						}
					}
				}
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out RaycastHit hit)) {

					Vector2Int gridPos = HexHelper.WorldToHex(hit.point, WorldManager.Self.World.Size);

					if (WorldManager.Self.CanPlace(Hive.HexSize, gridPos)) {
						print("making core");
						Core = WorldManager.Self.PlaceEnt(Resources.Load<GameObject>("Prefabs/Core"), gridPos, 2).GetComponent<Core>();
						UnitMenu.SetActive(true);
					}


				}

			}
		}
	}
}
