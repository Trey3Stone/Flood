using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public const int SIZE = 63; // Must be odd. Ideally power of 2 - 1

	public bool Paused { get; set; } = false;
	protected override void DerivedAwake() {
		DontDestroyOnLoad(gameObject);

		Debug.Assert(SIZE % 2 == 1, "Even-row offset hex arrays work best with odd size-length hexagons");
	}

	// Start is called before the first frame update
	void Start()
    {
		WorldManager.Self.Create(SIZE);
		FluidManager.Self.Start(SIZE); // Must occur after WorldManager start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) != Input.GetMouseButton(1)) {

			float weight = 0.1f * (Input.GetMouseButton(0) ? 1 : -1);

			//print("Mouse Down");
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Ray ray = new Ray(new Vector3(0, 100, 0), Vector3.down);

			//print(ray);

			if (Physics.Raycast(ray, out RaycastHit hit)) {
				//print("Mouse Hit: " + hit.point);
				FluidManager.Self.AddDiff(hit.point, 4, weight);
			}

		}
    }
}
