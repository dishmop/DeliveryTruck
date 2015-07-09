using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	Animator viewAnim;

	// Use this for initialization
	void Start () {
		viewAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			viewPauseMenu();
		}
	}


	void viewPauseMenu(){
		viewAnim.enabled = true;
		viewAnim.SetBool ("SlideOut", true);
		Time.timeScale = 0f;
	}

	public void resume(){

		viewAnim.SetBool ("SlideOut", false);
		viewAnim.enabled = false;
		Time.timeScale = 1.0f;
	}

	public void returnToMainMenu(){
		Application.LoadLevel (1);
	}
}

