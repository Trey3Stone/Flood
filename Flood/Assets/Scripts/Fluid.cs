using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	// TODO: Support vector sizes?
	private int size;
	public int Size { get { return size; } }

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

	public void Load(int sizeIn, float[,] heightDataIn, Mesh meshIn) {
		print("Fluid Load");
		size = sizeIn;
		heightData = heightDataIn;
		this.GetComponent<MeshFilter>().mesh = meshIn;
		renderer.material.SetInt("_Res", size + 1);
	}

	public void SetHeight(ComputeBuffer buffer) {
		renderer.material.SetBuffer("_HeightData", buffer);
	}
}
