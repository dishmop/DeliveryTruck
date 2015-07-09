using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CanvasController5 : MonoBehaviour {
	
	public SaveGraphData loadGraphData;
	public GameObject overallSceneView;
	//public GameObject additionBackground;
	public GameObject cameraObj;
	public GameObject player;
	public float mouseSensitivity = 1f;
	public GameObject finishLine;
	public GameObject instructionsPanel;
	public Text startRace;
	public Text button;
	public Text instructionText;
	//public float startingPos = 0f;
	public float endPos = 70f ;
	public float movingTime = 10f;
	public float startRaceFadeOutTime = 2f;
	
	PlayerController playerController;
	Plotting plotting;
	FinishLineLevel4 finishRace;
	UnityStandardAssets._2D.CameraFollow cameraFollow;
	int instruction;
	//Image panel;
	//bool animatingPanel;
	bool reviewingScene;
	float timer;
	float timer2;
	float timer3;
	Vector3 startingPosition;
	Vector3 endingPosition;
	bool navigate;
	bool startedRace;
	bool movingBack;
	bool showedGraph;
	void Awake(){
		//panel = GetComponent<Image> ();
		cameraFollow = cameraObj.GetComponent<UnityStandardAssets._2D.CameraFollow> ();
		playerController = player.GetComponent<PlayerController> ();
		plotting = cameraObj.GetComponent<Plotting> ();
		finishRace = finishLine.GetComponent<FinishLineLevel4> ();
		cameraFollow.enabled = false;
		playerController.enabled = false;
		plotting.enabled = false;
		finishRace.enabled = false;
	}
	
	// Use this for initialization
	void Start () {
		playInstructions ();
		startingPosition = cameraObj.transform.position;
		endingPosition = new Vector3 (endPos, cameraObj.transform.position.y,cameraObj.transform.position.z);
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
		
		if (navigate) {
			navigateScene();
		}
		
		
		if (reviewingScene) {
			reviewScene();
		}
	}
	
	public void playInstructions(){
		Vector2[] data;
		switch (instruction) {
		case 0:
			instructionText.text ="In this level you are required to reach the finish line with a certain velocity and before" +
				" time coutdown reaches zero.";
			reviewingScene = true;
			break;
		case 1:
			//movingBack = true;
			instructionText.text = "The best way to achieve that is to use the velocity/time graph above to control the truck" +
				"movement.";
			showGraph();
			break;
		case 2:
			instructionText.text = "The road a couple of obstacles ranging from a combination of air resistance in addition to friction" +
				"to friction alone.\n" +
				"There is also a turpo to boost your speed up.\n" +
				"Use your mouse to navigate through";
			reviewingScene = false;
			navigate = true;
			break;
		case 3:
			instructionText.text = "Now that you have seen the road, try to think of a strategy to achieve your objective," +
				"using the knowledge you learnt from the previous levels.";
			break;
		case 4:
			instructionText.text = "When you are ready press the start button to start.";
			button.text = "Start";
			break;
		case 5:
			
			skipInstructions();
			break;
		}
		instruction++;
	}
	
	void showGraph(){
		if (!showedGraph) {
			loadGraphData.load ("graphData3.dat");
			showedGraph = true;
		}
		
	}
	
	void navigateScene(){
		cameraObj.transform.Translate (new Vector3(mouseSensitivity * Input.GetAxis("Mouse X"),0f,0f));
		Vector3 cameraPosition = cameraObj.transform.position;
		cameraPosition.x = Mathf.Clamp (cameraPosition.x, startingPosition.x,endPos);
		cameraObj.transform.position = cameraPosition;
	}
	
	void reviewScene(){
		//Destroy (instructionsPanel);
		if (timer2 >= (movingTime+1)) {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, endingPosition, (2 - ((timer2-1) / movingTime)));
		} else {
			cameraObj.transform.position = Vector3.Lerp (startingPosition, endingPosition, (timer2 / movingTime));
		}
		timer2 += Time.deltaTime;
		//Debug.Log((timer2/movingTime));
		
		if (timer2 >= ((2 * movingTime)+1)) {
			//			Debug.Log ("we are here");
			reviewingScene = false;
			cameraObj.transform.position = startingPosition;
			//			startRace.text = "Ready...";
			//			gameObject.GetComponent<TimerCountdown4> ().enabled = true;
			timer3 = 0f;
		}
		
		
	}
	public void startLevel(){
		//Destroy (additionBackground);
		gameObject.GetComponent<TimerCountdown5>().enabled = false;
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
		showGraph ();
		navigate = false;
		reviewingScene = false;
		cameraObj.transform.position = startingPosition;
		Destroy (instructionsPanel);
		overallSceneView.SetActive (true);
		gameObject.GetComponent<TimerCountdown5> ().enabled = true;
	}
	
	//------------------------------------------------------
	
	
}
