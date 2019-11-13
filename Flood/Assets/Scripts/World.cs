using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// TODO: Support vector sizes?
	public Vector2 Size { get; private set; }
	public int GridSize { get; private set; }

	public HexGrid<float> HeightGrid { get; private set; }
	public RenderTexture HeightMap { get; private set; }

	public HexGrid<byte> MaskGrid { get; private set; }
	public RenderTexture MaskMap { get; private set; }

	public void Load(Vector2 sizeIn, HexGrid<float> heightGridIn) {
		print("World Load");
		Size = sizeIn;

		GridSize = heightGridIn.Size;

		HeightGrid = heightGridIn;
		HeightMap = HexHelper.HexTexture(HeightGrid);

		MaskGrid = HexHelper.HexMask(HeightGrid);

		//print(HeightGrid[64, 0]);
		//print(MaskGrid[64, 0]);

		//HeightGrid[64, 0] = 5;

		MaskMap = HexHelper.HexMaskTexture(MaskGrid);

		Mesh mesh = MeshHelper.HexGridToMesh(Size, HeightGrid);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();

		this.GetComponent<MeshFilter>().mesh = mesh;

		var collider = this.GetComponent<MeshCollider>();
		collider.sharedMesh = mesh;

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
