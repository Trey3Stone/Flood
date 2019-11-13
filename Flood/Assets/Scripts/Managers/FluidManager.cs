using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidManager : MonoSingleton<FluidManager>
{
	private const int THREAD_GROUP_SIZE = 8;
	private const float UPDATE_RATE = 1 / 60f;

	private ComputeShader fluidSim;
	private SwapBuffer fluidBuffer;

	//private RenderTexture deltaRT;

	private int updateKernel;
	private int applyKernel;

	private Fluid fluid;

	enum FluidStage
	{
		Idle,
		Update,
		Apply
	};

	public struct Diff
	{
		// NOTE: Shaders swap X and Y coordinates.

		public float posX;
		public float posY;

		public float radius;
		public float weight;

		public Diff(Vector2 pos, float radiusIn, float weightIn) {
			this = new Diff(pos.x, pos.y, radiusIn, weightIn);
		}

		public Diff(float posXIn, float posYIn, float radiusIn, float weightIn) {
			posX = posXIn;
			posY = posYIn;

			radius = radiusIn;
			weight = weightIn;
		}
	}

	private FluidStage curStage = FluidStage.Idle;
	private float lastUpdateTime;

	private List<Diff> Diffs = new List<Diff>();
	private ComputeBuffer diffBuffer;


	protected override void DerivedAwake() {
		fluidSim = Resources.Load("Shaders/FluidSim") as ComputeShader;
		updateKernel = fluidSim.FindKernel("Update");
		applyKernel = fluidSim.FindKernel("Apply");
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (GameManager.Self.Paused) return; // TODO: Make the update timer stop on pause;

		if (Time.time - lastUpdateTime >= UPDATE_RATE) {
			if (curStage == FluidStage.Idle) {
				lastUpdateTime = Time.time;
				fluidBuffer.Swap();

				curStage = FluidStage.Update;
			} else {
				Debug.LogWarning("Fluid update out of sync!");
			}
		}


		switch (curStage) {
			case FluidStage.Idle:
				break;
			case FluidStage.Update:

				//Diffs.Add(new Diff(fluid.GridSize / 2.0f, 0, 4, 0.02f));
				//Diffs.Add(new Diff(fluid.GridSize / 2.0f, fluid.GridSize - 1, 4, -2f));

				//Debug.Log(Diffs[0].radius);

				fluidSim.SetTexture(updateKernel, "Front", fluidBuffer.Front);
				fluidSim.SetTexture(updateKernel, "Back", fluidBuffer.Back);

				fluidSim.SetInt("DiffCount", Diffs.Count);
				//fluidSim.SetInt("DiffCount", 1);

				if (Diffs.Count > 0) {
					diffBuffer.Release();
					diffBuffer = new ComputeBuffer(Diffs.Count, 4 * sizeof(float), ComputeBufferType.Structured, ComputeBufferMode.Immutable);
					diffBuffer.SetData<Diff>(Diffs);
					fluidSim.SetBuffer(updateKernel, "Diffs", diffBuffer);
					Diffs.Clear();
				}

				fluidSim.Dispatch(updateKernel, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1);
				curStage = FluidStage.Apply;
				break;
			case FluidStage.Apply:
				fluidSim.SetTexture(applyKernel, "Front", fluidBuffer.Front);
				fluidSim.SetTexture(applyKernel, "Back", fluidBuffer.Back);
				fluidSim.Dispatch(applyKernel, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1);
				fluid.SetFluidMap(fluidBuffer.Front);
				curStage = FluidStage.Idle;
				break;
		}

		//if (!fluidBuffer.Front.IsCreated()) fluidBuffer.Front.Create();
		//if (!fluidBuffer.Back.IsCreated()) fluidBuffer.Back.Create();

		//fluidBuffer.Swap();

		//fluidSim.Dispatch(updateKernel, 1 + res / THREAD_GROUP_SIZE, 1 + res / THREAD_GROUP_SIZE, 1);
		//fluidSim.Dispatch(applyKernel, 1 + res / THREAD_GROUP_SIZE, 1 + res / THREAD_GROUP_SIZE, 1);

		//Time.time

		//fluidSim.

		//print(fluidBuffer.Front.IsValid());

		//fluid.SetFluidMap(fluidBuffer.Front);
	}

	public void Start(int sideLength) {
		print("START!");

		fluid = FluidGenerator.Create(GameManager.SIZE);

		fluidBuffer = new SwapBuffer(fluid.HeightMap);
		fluid.SetFluidMap(fluidBuffer.Front);

		var deltaRT = new RenderTexture(fluid.HeightMap.descriptor);
		deltaRT.format = RenderTextureFormat.RGFloat;
		deltaRT.Create();

		// Default diffBuffer
		diffBuffer = new ComputeBuffer(1, 4 * sizeof(float), ComputeBufferType.Structured, ComputeBufferMode.Immutable);
		diffBuffer.SetData(new Diff[] { new Diff() });

		fluidSim.SetTexture(updateKernel, "Delta", deltaRT);
		fluidSim.SetTexture(updateKernel, "Mask", WorldManager.Self.World.MaskMap);
		fluidSim.SetTexture(updateKernel, "Terrain", WorldManager.Self.World.HeightMap);
		fluidSim.SetBuffer(updateKernel, "Diffs", diffBuffer);



		fluidSim.SetTexture(applyKernel, "Delta", deltaRT);
		fluidSim.SetTexture(applyKernel, "Mask", WorldManager.Self.World.MaskMap);
		fluidSim.SetTexture(applyKernel, "Terrain", WorldManager.Self.World.HeightMap);

		fluid.SetMask(WorldManager.Self.World.MaskMap);
		fluid.SetTerrainMap(WorldManager.Self.World.HeightMap);

		//var test = new RenderTexture(2 * size + 1, 2 * size + 1, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);

		lastUpdateTime = Time.time;


		//Test(size);
	}

	private void Test(int size) {
		float[] data = new float[fluid.GridSize * fluid.GridSize];

		float total = 0;

		for (int i = 0; i < fluid.GridSize * fluid.GridSize; i++) {
			if ((new Vector2(fluid.GridSize / 2f, fluid.GridSize / 2f) - MeshHelper.UnflattenCoords(fluid.GridSize, i)).magnitude < fluid.GridSize / 3f)
				data[i] = 60;
			total += data[i];
		}

		//fluidBuffer.Front.SetData(data);
		//fluidBuffer.Back.SetData(data);
		/*
		float height = total / (res * res);
		Vector3 pos;
		GameObject testWall;

		testWall = GameObject.CreatePrimitive(PrimitiveType.Cube); // North
		testWall.name = "North Wall";
		pos = MeshHelper.ArrToWorld(size + 1, 0, size + 1);
		pos.Scale(new Vector3(0, 0, 1));
		pos += new Vector3(0, height / 2, 0);
		testWall.transform.position = pos;
		testWall.transform.localScale = new Vector3(size + 2, height, 1);

		testWall = GameObject.CreatePrimitive(PrimitiveType.Cube); // East
		testWall.name = "East Wall";
		pos = MeshHelper.ArrToWorld(size + 1, size + 1, 0);
		pos.Scale(new Vector3(1, 0, 0));
		pos += new Vector3(0, height / 2, 0);
		testWall.transform.position = pos;
		testWall.transform.localScale = new Vector3(1, height, size + 2);

		testWall = GameObject.CreatePrimitive(PrimitiveType.Cube); // South
		testWall.name = "South Wall";
		pos = MeshHelper.ArrToWorld(size + 1, 0, 0);
		pos.Scale(new Vector3(0, 0, 1));
		pos += new Vector3(0, height / 2, 0);
		testWall.transform.position = pos;
		testWall.transform.localScale = new Vector3(size + 2, height, 1);

		testWall = GameObject.CreatePrimitive(PrimitiveType.Cube); // West
		testWall.name = "West Wall";
		pos = MeshHelper.ArrToWorld(size + 1, 0, 0);
		pos.Scale(new Vector3(1, 0, 0));
		pos += new Vector3(0, height / 2, 0);
		testWall.transform.position = pos;
		testWall.transform.localScale = new Vector3(1, height, size + 2);
		*/
	}

	public void Save() {

	}

	public void Load() {

	}


	class SwapBuffer // Use customrendertexture?
	{
		private bool isSwapped = false;

		private RenderTexture buffer1;
		private RenderTexture buffer2;

		public RenderTexture Front { get { return isSwapped ? buffer2 : buffer1; } }
		public RenderTexture Back { get { return isSwapped ? buffer1 : buffer2; } }

		public SwapBuffer(RenderTexture texture) {

			buffer1 = texture;

			buffer2 = new RenderTexture(texture);

			buffer2.filterMode = FilterMode.Trilinear;
			buffer2.wrapMode = TextureWrapMode.Mirror;
			Graphics.Blit(buffer1, buffer2);

		}

		public void Swap() {
			isSwapped = !isSwapped;
		}

		public void Destroy() {
			buffer1.Release();
			buffer2.Release();
		}
	}

	public void AddDiff(Vector3 worldPos, float radius, float weight) {
		AddDiff(worldPos.x + fluid.Size.x / 2, worldPos.z + fluid.Size.y / 2, radius, weight);
	}

	public void AddDiff(float posX, float posY, float radius, float weight) {
		Diffs.Add(new Diff(posX, posY, radius, weight));
	}

	/*
	public void GetTotalVolume2() {
		int vKernel = fluidSim.FindKernel("GetTotalVolume");

		int[] vData = new int[] { 0, 0 };
		ComputeBuffer vBuffer = new ComputeBuffer(2, sizeof(int));
		vBuffer.SetData(vData);

		fluidSim.SetBuffer(vKernel, "vBuffer", vBuffer);
		fluidSim.SetTexture(vKernel, "Front", fluidBuffer.Front);
		fluidSim.SetTexture(vKernel, "Back", fluidBuffer.Back);

		fluidSim.Dispatch(vKernel, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1);

		vBuffer.GetData(vData);
		Debug.Log("TOTAL Volume: " + vData[0] / 1000f);
		Debug.Log("TOTAL Velocity: " + (curStage == FluidStage.Idle ? 1 : -1) * vData[1] / 1000f);
		vBuffer.Release();


	}
	*/

	public void GetTotalVolume() {
		int vKernel = fluidSim.FindKernel("GetTotalVolume");

		int count = fluid.GridSize * fluid.GridSize;

		float[] vData = new float[count];
		ComputeBuffer vBuffer = new ComputeBuffer(count, sizeof(float));
		vBuffer.SetData(vData);

		fluidSim.SetBuffer(vKernel, "vBuffer", vBuffer);
		fluidSim.SetTexture(vKernel, "Front", fluidBuffer.Front);
		fluidSim.SetTexture(vKernel, "Back", fluidBuffer.Back);
		fluidSim.SetTexture(vKernel, "Mask", WorldManager.Self.World.MaskMap);

		fluidSim.Dispatch(vKernel, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1 + fluid.GridSize / THREAD_GROUP_SIZE, 1);

		vBuffer.GetData(vData);

		double volTotal = 0;
		for (int i = 0; i < count; i++) {
			volTotal += vData[i];
		}


		Debug.Log("TOTAL Volume: " + volTotal);
		//Debug.Log("TOTAL Velocity: " + (curStage == FluidStage.Idle ? 1 : -1) * vData[1] / 1000f);
		vBuffer.Release();
	}
}
