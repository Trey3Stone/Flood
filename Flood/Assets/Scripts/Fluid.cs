using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	// TODO: Support vector sizes?
	//private Vector2 size;
	public Vector2 Size { get; private set; }
	public int GridSize { get; private set; }

	private new MeshRenderer renderer;

	public HexGrid<float> HeightGrid { get; private set; }
	public RenderTexture HeightMap { get; private set; }

	void Awake() {
		renderer = this.GetComponent<MeshRenderer>();
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}

	public void Load(Vector2 sizeIn, HexGrid<float> heightGridIn) {
		print("Fluid Load");
		Size = sizeIn;

		GridSize = heightGridIn.Size;

		HeightGrid = heightGridIn;
		HeightMap = HexHelper.HexTexture(HeightGrid);
		SetFluidMap(HeightMap);

		HexGrid<float> baseGrid = new HexGrid<float>(GridSize);
		HexHelper.FillHexGrid(baseGrid, (GridSize - 1) / 2);

		Mesh mesh = MeshHelper.HexGridToMesh(Size, baseGrid);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();

		mesh.bounds = new Bounds(Vector3.zero, mesh.bounds.extents + new Vector3(0, 1000, 0));
		print("Fluid Mesh Bounds: " + mesh.bounds);

		this.GetComponent<MeshFilter>().mesh = mesh;
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
