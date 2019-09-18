using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T self = null;
	public static T Self{ get { return self; } }
	private void Awake() {
		Debug.Assert(self == null);
		self = this as T;

		DerivedAwake();
	}

	protected virtual void DerivedAwake() { }
}
