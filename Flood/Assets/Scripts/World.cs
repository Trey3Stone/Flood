using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// TODO: Support vector sizes?
	public Vector2 Size { get; private set; }

	public RenderTexture HeightMap { get; private set; }

	private float[,] heightData;

	public void Load(Vector2 sizeIn, float[,] heightDataIn, Mesh meshIn, RenderTexture heightMapIn) {
		print("World Load");
		Size = sizeIn;
		heightData = heightDataIn;
		this.GetComponent<MeshFilter>().mesh = meshIn;

		HeightMap = heightMapIn;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
