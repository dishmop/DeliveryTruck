using UnityEngine;
using System.Collections;

public class Plotting : MonoBehaviour {

	public GameObject playerObj;
	public float delayRatio = 5.71429f;
	public float speedOffset = 0.3134796f;
	public float calibrationFactor = 3.19f;

	Rigidbody2D playerRigidbody;
	float delayTime;
	float timer;
	//LineRenderer line;
	// Use this for initialization
	void Start () {
		delayTime = Time.deltaTime * delayRatio;
		playerRigidbody =  playerObj.GetComponent<Rigidbody2D> ();
		//line = GetComponent<LineRenderer> ();
		//line.sortingLayerName = "Graph";
		//  Create a new graph named "MouseX", with a range of 0 to 2000, colour green at position 100,100
		PlotManager.Instance.PlotCreate("Velocity", 0, 10, Color.green, new Vector2(100,100));

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= delayTime &&playerRigidbody.velocity.x > 0) {
			timer = 0f;
			PlotManager.Instance.PlotAdd("Velocity",  ((calibrationFactor * playerRigidbody.velocity.x) + speedOffset));
		}

	}
}
