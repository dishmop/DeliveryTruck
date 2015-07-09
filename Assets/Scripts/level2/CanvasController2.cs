using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasController2 : MonoBehaviour {

	public GameObject additionBackground;
	public GameObject cameraObj;
	public GameObject player;
	//public GameObject plotter;
	public GameObject finishLine;
	public GameObject graphPanel;
	public GameObject instructionsPanel;
	public Text startRace;
	public Text button;
	public Text instructionText;
	//public float startingPos = 0f;
	public float frictionPos = 50f;
	public float endPos = 70f ;
	public float movingTime = 10f;
	public float startRaceFadeOutTime = 2f;

	PlayerController playerController;
	Plotting plotting;
	FinishRace finishRace;
	int instruction;
	//Image panel;
	//bool animatingPanel;
	bool reviewingScene;
	float timer;
	float timer2;
	float timer3;
	Vector3 startingPosition;
	Vector3 frictionPosition;
	Vector3 endingPosition;
	bool showingFriction;
	bool startedRace;
	bool movingBack;

	void Awake(){
		//panel = GetComponent<Image> ();
		playerController = player.GetComponent<PlayerController> ();
		plotting = cameraObj.GetComponent<Plotting> ();
		finishRace = finishLine.GetComponent<FinishRace> ();
		playerController.enabled = false;
		plotting.enabled = false;
		finishRace.enabled = false;
	}

	// Use this for initialization
	void Start () {
		playInstructions ();
		startingPosition = cameraObj.transform.position;
		endingPosition = new Vector3 (endPos, cameraObj.transform.position.y,cameraObj.transform.position.z);
		frictionPosition = new Vector3 (frictionPos, cameraObj.transform.position.y,cameraObj.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (startedRace) {
			timer3 += Time.deltaTime;
			if(timer3 >= startRaceFadeOutTime){
				startRace.text = "";
				startedRace = false;
			}
		}

		if (showingFriction) {
			showFriction();
		}


		if (reviewingScene) {
			reviewScene();
		}
	}

	public void playInstructions(){
		Vector2[] data;
		switch (instruction) {
		case 0:
			instructionText.text ="When the truck moves over a rough surface it experiences friction force " +
				"in the opposite direction of the motion.";
			showingFriction = true;
			break;
		case 1:
			movingBack = true;
			instructionText.text = "The friction force causes the truck to decelerate uniformily if there is no " +
				"other force acting on the truck, as shown on the graph above.";
			data = new Vector2[]{
				new Vector2(0.01f,3f), new Vector2(30f,3f), new Vector2(60f,1.5f), new Vector2(90f,1.5f) 
			};
			showGraph(data);
			break;
		case 2:
			instructionText.text = "If the fuel is pressed while moving over a rough surface the forward force balances" +
				" the backward friction force and the truck moves with steady speed, as shown on the graph above.";
			data = new Vector2[]{
				new Vector2(0.01f,0.01f), new Vector2(30f,3f), new Vector2(90f,3f) 
			};
			showGraph(data);
			break;
		case 3:
			instructionText.text = "The faster the truck moves over a rough surface the less decrease in speed it experiences.";
			break;
		case 4:
			instructionText.text = "Your objective is again to reach the finish line as fast as possible" +
				" To achieve this maxismised the area under the graph.";
			button.text = "Start";
			break;
		case 5:

			reviewingScene = true;
			break;
		}
		instruction++;
	}

	void showGraph(Vector2[] data){
		UIGraph graph = graphPanel.GetComponent<UIGraph> ();

		graph.SetAxesRanges (0f,100f,0f,4f);
		graph.UploadData (data);
	}

	void showFriction(){
		if (timer2 >= (movingTime)) {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, frictionPosition, (2 - ((timer2) / movingTime)));
		} else {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, frictionPosition, (timer2 / movingTime));
			timer2 += Time.deltaTime;
		}
		if (movingBack) {
			timer2 += Time.deltaTime;
		}

		if(timer2 >= (2* movingTime)){
			timer2 = 0f;
			showingFriction = false;
		}
	}

	void reviewScene(){
		Destroy (instructionsPanel);
		if (timer2 >= (movingTime+1)) {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, endingPosition, (2 - ((timer2-1) / movingTime)));
		} else {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, endingPosition, (timer2 / movingTime));
		}
		timer2 += Time.deltaTime;
		//Debug.Log((timer2/movingTime));
		
		if (timer2 >= ((2 * movingTime)+1)) {
			Debug.Log ("we are here");
			reviewingScene = false;
			cameraObj.transform.position = startingPosition;
			startRace.text = "Ready...";
			gameObject.GetComponent<TimerCountdown2> ().enabled = true;
			timer3 = 0f;
		}


	}
	public void startLevel(){
		Destroy (additionBackground);
		gameObject.GetComponent<TimerCountdown2>().enabled = false;
		startRace.text = "Go!";
		startedRace = true;
		//cameraFollow.enabled = true;
		playerController.enabled = true;
		plotting.enabled = true;
		finishRace.enabled = true;

	}

	// To used by all levels to skip the instructions
	//------------------------------------------------------
	
	public void skipInstructions(){
		showingFriction = false;
		cameraObj.transform.position = startingPosition;
		Destroy (instructionsPanel);
		gameObject.GetComponent<TimerCountdown2> ().enabled = true;
	}
	
	//------------------------------------------------------


}
