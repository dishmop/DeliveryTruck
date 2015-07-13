using UnityEngine;
using System.Collections;

public class Plotting : MonoBehaviour {

	public GameObject graphObj;
	public GameObject playerObj;
	public float xScale = 1f;
	public float yScale = 1f;
	public float xAxisMin = -50;
	public float xAxisMax = 50;
	public float yAxisMin = -2;
	public float yAxisMax = 2; 
	public int maxNoPoints = 1000;
//	public float delayRatio = 5.71429f;
//	public float speedOffset = 0.3134796f;
//	public float calibrationFactor = 0.5f;

	Rigidbody2D playerRigidbody;
	float delayTime;
	float timer;
	float timeaxis;
	Vector2[] data;
	long count;
	// Use this for initialization
	void Start () {
		data = new Vector2[maxNoPoints];
		delayTime = Time.fixedDeltaTime * xScale;
		playerRigidbody =  playerObj.GetComponent<Rigidbody2D> ();
		//  Create a new graph named "MouseX", with a range of 0 to 2000, colour green at position 100,100
		//PlotManager.Instance.PlotCreate("Velocity", 0, 10, Color.green, new Vector2(100,100));

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timeaxis += Time.fixedDeltaTime;
		timer += Time.fixedDeltaTime;
		if (timer >= delayTime && playerRigidbody.velocity.x > 0) {
			timer = 0f;
			plotPoint(xScale * timeaxis,yScale * playerRigidbody.velocity.x );
			if(count<maxNoPoints-2){
				count++;
			}

			//PlotManager.Instance.PlotAdd("Velocity",  ((calibrationFactor * playerRigidbody.velocity.x) + speedOffset));
		}

	}

	void plotPoint(float xPoint, float yPoint){
		UIGraph graph = graphObj.GetComponent<UIGraph> ();
		graph.setRed (false);
		graph.SetAxesRanges (xAxisMin,xAxisMax,yAxisMin,yAxisMax);
		//Debug.Log ("Count is at " + count);
		data.SetValue( new Vector2 (xPoint, yPoint), count);
		graph.UploadData (data, (int)count);
	}

	public Vector2[] getData(){
		return data;
	}

//	void OnGUI(){
//		GUI.Label (new Rect (10, 30, 300, 30), ("Count = " + count + "delayTime = " + delayTime));
//		
//	}
}
