using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// TODO: Support vector sizes?
	private Vector2 size;
	public Vector2 Size { get { return size; } }

	private RenderTexture heightMap;
	public RenderTexture HeightMap { get { return heightMap; } }

	private float[,] heightData;

	public void Load(Vector2 sizeIn, float[,] heightDataIn, Mesh meshIn, RenderTexture heightMapIn) {
		print("World Load");
		size = sizeIn;
		heightData = heightDataIn;
		this.GetComponent<MeshFilter>().mesh = meshIn;

		heightMap = heightMapIn;
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
