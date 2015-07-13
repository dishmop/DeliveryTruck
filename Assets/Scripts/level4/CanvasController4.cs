using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CanvasController4 : MonoBehaviour {

	//public SaveGraphData loadGraphData;
	public GameObject graphPanel;
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
		playerController = player.GetComponent<PlayerController> ();
		plotting = cameraObj.GetComponent<Plotting> ();
		finishRace = finishLine.GetComponent<FinishLineLevel4> ();
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
		switch (instruction) {
		case 0:
			instructionText.text ="In this level, you speed should not exceed a certain limit, indicated on the graph above, only when you cross the finish line.";
			showGraph();

			break;
		case 1:
			//movingBack = true;
			instructionText.text = "You have to be within a distance when you cross the finish line, so slow down if you are moving fast to minimise the stopping distance.";
			reviewingScene = true;
			break;
		case 2:
			instructionText.text = "The wind blows near the start at time 34 for a duration of 12 seconds as indicated on the graph.\n" +
				"There is also friction throughout the road.";
			reviewingScene = false;
			break;
		case 3:
			instructionText.text = "Use the arrows keys to navigate the road.";
			navigate = true;
			break;
		case 4:
			instructionText.text = "Now that you have seen the road, try to think of a strategy to achieve your objective," +
				"using what you learnt from the previous levels.";
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
			//loadGraphData.load ("graphData.dat");
			UIGraph graph = graphPanel.GetComponent<UIGraph> ();
			graph.setRed (true);
			graph.SetAxesRanges (0f, 100f, 0f, 4f);
			Vector2[] data = new Vector2[] {
				new Vector2(0.01f,finishRace.winningSpeed * plotting.yScale) , new Vector2(90f, finishRace.winningSpeed * plotting.yScale)
			};
			graph.UploadData (data);
			showedGraph = true;
		}

	}
	
	void navigateScene(){
		cameraObj.transform.Translate (new Vector3(mouseSensitivity * Input.GetAxis("Horizontal"),0f,0f));
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
		gameObject.GetComponent<TimerCountdown4>().enabled = false;
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
		gameObject.GetComponent<TimerCountdown4> ().enabled = true;
	}
	
	//------------------------------------------------------
	
	
}
