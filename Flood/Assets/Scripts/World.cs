using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// TODO: Support vector sizes?
	private int size;
	public int Size { get { return size; } }

	private float[,] heightData;

	public void Load(int sizeIn, float[,] heightDataIn, Mesh meshIn) {
		print("World Load");
		size = sizeIn;
		heightData = heightDataIn;
		this.GetComponent<MeshFilter>().mesh = meshIn;
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
