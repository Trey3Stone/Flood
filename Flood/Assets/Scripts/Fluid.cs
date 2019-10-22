using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	// TODO: Support vector sizes?
	//private Vector2 size;
	public Vector2 Size { get; private set; }

	public RenderTexture HeightMap { get; private set; }

	private new MeshRenderer renderer;

	private float[,] heightData;

	void Awake() {
		renderer = this.GetComponent<MeshRenderer>();
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}

	public void Load(Vector2 sizeIn, float[,] heightDataIn, Mesh meshIn, RenderTexture heightMapIn) {
		print("Fluid Load");
		Size = sizeIn;
		heightData = heightDataIn;
		this.GetComponent<MeshFilter>().mesh = meshIn;
		renderer.material.SetInt("_Res", heightDataIn.GetLength(0));

		HeightMap = heightMapIn;
		SetFluidMap(HeightMap);

		
	}

	public void SetFluidMap(RenderTexture texture) {
		//print(HeightMap.IsCreated());
		renderer.material.SetTexture("_FluidMap", texture);
	}

	public void SetTerrainMap(RenderTexture texture) {
		//print(HeightMap.IsCreated());
		renderer.material.SetTexture("_TerrainMap", texture);
	}

	public void SetMask(RenderTexture texture) {
		//print(HeightMap.IsCreated());
		renderer.material.SetTexture("_Mask", texture);
	}

}
