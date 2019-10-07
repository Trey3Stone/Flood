using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public const int SIZE = 255; // Must be odd. Ideally power of 2 - 1

	public bool Paused { get; set; } = false;
	protected override void DerivedAwake() {
		DontDestroyOnLoad(gameObject);

		Debug.Assert(SIZE % 2 == 1, "Even-row offset hex arrays work best with odd size-length hexagons");
	}

	// Start is called before the first frame update
	void Start()
    {
		WorldManager.Self.Create(SIZE);
		FluidManager.Self.Start(SIZE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
