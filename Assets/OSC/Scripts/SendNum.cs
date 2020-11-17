using UnityEngine;
using System.Collections;

public class SendNum : MonoBehaviour {

	public OSC osc;

	public string name = "/Num";
	public int num;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Send()
	{
		OscMessage message = new OscMessage();

		message.address = name;
		message.values.Add(num);
		osc.Send(message);
	}


}
