using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasController : MonoBehaviour {

	public GameObject additionBackground;
	public GameObject cameraObj;
	public GameObject player;
	public GameObject areaUnderGraph;
	public GameObject finishLine;
	public GameObject graphPanel;
	public GameObject instructionsPanel;
	public Text startRace;
	public Text button;
	public Text instructionText;
	public Slider slider;
	//public float startingPos = 0f;
	public float endPos = 70f ;
	public float movingTime = 10f;
	public float startRaceFadeOutTime = 2f;
	public int numberOfLines = 9;

	PlayerController playerController;
	Plotting plotting;
	FinishRace finishRace;
	int instruction;
	//Image panel;
	//bool animatingPanel;
	bool animatingSlider;
	bool reviewingScene;
	float timer;
	float timer2;
	float timer3;
	Vector3 startingPosition;
	Vector3 endingPosition;
	bool startedRace;
	UIGraph graph;

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
		graph = graphPanel.GetComponent<UIGraph> ();
		graph.SetAxesRanges (0f,100f,0f,4f);

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

		if (animatingSlider) {
			timer += Time.deltaTime;
			animateSlider(timer/4f);
		}

		if (reviewingScene) {
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
				gameObject.GetComponent<TimerCountdown> ().enabled = true;
				timer3 = 0f;
			}
		}
	}

	public void playInstructions(){
		switch (instruction) {
		case 0:
			instructionText.text ="Welcome to Speed Truck.\n" +
				"In this game, you will use velocity against time graphs to help you drive the truck " +
					"to the finish line before you run out of time.";
			break;
		case 1:
			instructionText.text = "A velocity/time graph is used to analyse the motion" +
				" of moving objects. It also gives information about the distance travelled and the acceleration.";
			showGraph();
			break;
		case 2:
			instructionText.text = "The area under the graph represents the distance travelled and " +
				"the slope of the graph gives the acceleration.";
			StartCoroutine(animateArea());
			break;
		case 3:
			areaUnderGraph.SetActive (false);
			instructionText.text = "Press 'Space' to apply a forward force, i.e accelerate, and 'left shift' to apply a backward force, i.e. brake. " +
				"Be careful! you have a limited amount of fuel, the screen will flash red when you are run out of fuel.\n" +
					"The truck uses a battery to save the kinetic energy when you brake, so that will increase the amount of fuel you have.";
			animatingSlider = true;
			break;
		case 4:
			instructionText.text = "Use the 'Up' and 'Down' arrows to change the amount of force you want to apply on the truck.\n" +
				"The number on the side of the fuel bar indicates the amount of force you are currently selecting.\n" +
				"The greater the force you apply the more fuel it takes.";
			break;
		case 5:
			animatingSlider = false;
			animateSlider(0f);
			instructionText.text = "Your objective is to reach the finish line as fast as possible." +
				" To achieve this, maxismise the area under the graph to get maximum distance.\n" +
					"You will get extra points for any fuel remains.";
			button.text = "Start";
			break;
		case 6:

			reviewScene();
			break;
		}
		instruction++;
	}

	void showGraph(){

		Vector2[] data = new Vector2[]{
			new Vector2(0.01f,0.01f), new Vector2(30f,3f), new Vector2(60f,3f), new Vector2(90f,0.01f) 
		};
		graph.UploadData (data);
	}
	

	IEnumerator animateArea(){
		areaUnderGraph.SetActive (true);
		Image image = areaUnderGraph.GetComponent<Image> ();
		for (float t=0f; t<2f; t += Time.deltaTime) {
			image.color = Color.Lerp(Color.clear, Color.white, (t/2f));
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}

	void animateSlider(float weight){
		slider.gameObject.GetComponent<Animator> ().enabled = true;
		slider.maxValue = 10f;
		slider.minValue = 0f;
		slider.value = Mathf.Lerp (10f, 5f, weight);
		if (timer >= 4f) {
			slider.gameObject.GetComponent<Animator> ().enabled = false;
		}
	}

	void reviewScene(){
		Destroy (instructionsPanel);
		reviewingScene = true;
	}
	public void startLevel(){
		animatingSlider = false;
		slider.gameObject.GetComponent<Animator> ().enabled = false;
		Destroy (additionBackground);
		gameObject.GetComponent<TimerCountdown>().enabled = false;
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
		slider.gameObject.GetComponent<Animator> ().enabled = false;
		Destroy (instructionsPanel);
		gameObject.GetComponent<TimerCountdown> ().enabled = true;
	}

	//------------------------------------------------------

}
