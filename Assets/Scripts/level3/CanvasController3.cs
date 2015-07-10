using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasController3 : MonoBehaviour {

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
			instructionText.text ="When the truck moves through air it experiences a drag force " +
				"in the opposite direction of the motion due to air resistance.";
			showingFriction = true;
			break;
		case 1:
			movingBack = true;
			instructionText.text = "The drag force or air resistance is proprotional to the speed of the truck, i.e" +
				" the faster it moves the greater drag force it experiences, as shown on the graph above.";
			data = new Vector2[90];
			for(int i =0; i<90; i++){
				data[i] = new Vector2(i, 3 * (Mathf.Exp(-i/10f)));
			}
			showGraph(data);
			break;
		case 2:
			instructionText.text = "If the truck is accelerating while moving through air, the drag force increases" +
				" until it balances the forward force and the truck moves with steady speed, as shown on the graph above.";
			data = new Vector2[90];
			for(int i =0; i<90; i++){
				data[i] = new Vector2(i, 3 * (1- Mathf.Exp(-i/10f)));
			}
			showGraph(data);
			break;
		case 3:
			instructionText.text = "The faster the truck moves through air resistance the less decrease in speed it experiences.";
			break;
		case 4:
			instructionText.text = "Your objective is to reach the finish line as fast as possible with the minimum use of fuel.";
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
			//Debug.Log ("we are here");
			reviewingScene = false;
			cameraObj.transform.position = startingPosition;
			startRace.text = "Ready...";
			gameObject.GetComponent<TimerCountdown3> ().enabled = true;
			timer3 = 0f;
		}


	}
	public void startLevel(){
		Destroy (additionBackground);
		gameObject.GetComponent<TimerCountdown3>().enabled = false;
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
		gameObject.GetComponent<TimerCountdown3> ().enabled = true;
	}
	
	//------------------------------------------------------


}
