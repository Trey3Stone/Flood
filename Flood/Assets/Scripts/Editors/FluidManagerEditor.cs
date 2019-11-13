using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FluidManager))]
public class FluidManagerEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("Get Total Volume")) {
			FluidManager.Self.GetTotalVolume();
		}
	}
}
