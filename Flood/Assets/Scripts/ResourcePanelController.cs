using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelController : MonoBehaviour
{

	[SerializeField]
	private Slider meter;

	[SerializeField]
	private Text text;

    // Start is called before the first frame update
    void Start()
    {
		meter.value = 0;
		text.text = "<size=46>SELECT CORE POSITION</size>";
    }

    // Update is called once per frame
    void Update()
    {
		Core core = GameManager.Self.Core;
        if (core != null) {

			meter.value = core.Contents / Core.Capacity;
			text.text = string.Format("{0,-4} / {1,4}", (int)core.Contents, (int)Core.Capacity);
		}
    }
}
