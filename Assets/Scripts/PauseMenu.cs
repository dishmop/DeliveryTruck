using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	public GameObject PauseBlocker;
	public AudioClip WhenPaused;
	public AudioClip WhenResumed;
	public FMG.Music music ;


	GameObject vectorCanvas;
	Animator viewAnim;
	bool isPaused;
	bool isShown;
	// Use this for initialization
	void Start () {
		viewAnim = GetComponent<Animator> ();
		viewAnim.enabled = true;
		viewAnim.SetBool ("SlideOut", false);
		hideMenu ();
		vectorCanvas = GameObject.Find ("VectorCanvas");
		//viewAnim.speed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !isPaused) {
			isPaused = true;
			StartCoroutine(viewPauseMenu());
		}
	}


	IEnumerator viewPauseMenu(){

		if (!isShown) {
			showMenu();
			isShown = true;
		}
		vectorCanvas = GameObject.Find ("VectorCanvas");
		if (vectorCanvas != null) {
			vectorCanvas.GetComponent<Canvas>().enabled = false;
		}
		PauseBlocker.SetActive (true);

		viewAnim.SetBool ("SlideOut", true);
		music.musicClip = WhenPaused;
		music.Awake ();

		yield return new WaitForSeconds (1f);
		AudioListener.pause = true;
		Time.timeScale = 0f;
	}

	public void resume(){
		Time.timeScale = 1.0f;
		music.musicClip = WhenResumed;
		music.Awake ();
		viewAnim.SetBool ("SlideOut", false);
		isPaused = false;
		AudioListener.pause = false;
		if (vectorCanvas != null) {
			vectorCanvas.GetComponent<Canvas>().enabled = true;
		}
		PauseBlocker.SetActive (false);

	}

	public void returnToMainMenu(){
		AudioListener.pause = false;
		viewAnim.SetBool ("SlideOut", false);
		Time.timeScale = 1.0f;
		//Application.Quit ();
		Application.LoadLevel (1);
	}

	void hideMenu(){
		GetComponent<Image> ().enabled = false;
		foreach (Image image in  GetComponentsInChildren<Image>()) {
			image.enabled = false;
		}
		foreach (Text text in  GetComponentsInChildren<Text>()) {
			text.enabled = false;
		}
	}

	void showMenu(){
		GetComponent<Image> ().enabled = true;
		foreach (Image image in  GetComponentsInChildren<Image>()) {
			image.enabled = true;
		}
		foreach (Text text in  GetComponentsInChildren<Text>()) {
			text.enabled = true;
		}
	}
	
}

