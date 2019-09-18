using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoSingleton<WorldManager>
{
	private World world;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Create(int size) {
		print("WorldManager Create");
		world = WorldGenerator.Create(size);
	}

	public void Save(string name) {

	}

	public void Load(string name) {

	}

}
