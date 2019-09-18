using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public const int SIZE = 200;

	public bool Paused { get; set; } = false;
	protected override void DerivedAwake() {
		DontDestroyOnLoad(gameObject);
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
