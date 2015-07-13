using UnityEngine;
using System.Collections;

public class TimerCountdown2 : MonoBehaviour {
	
	public GameObject signal;
	public GameObject minutes;
	public GameObject seconds;
	public GameObject finishLine;
	public float signalTimer = 3f;
	public CanvasController2 cc;
	public AudioClip countdownSound;
	
	Animator anim1;
	Animator anim2;
	Animator anim3;
	AudioSource soundPlayer;
	FinishRace finishRace;
	// Use this for initialization
	void Start () {
		soundPlayer = GetComponent<AudioSource> ();
		finishRace = finishLine.GetComponent<FinishRace> ();
		cc = gameObject.GetComponent<CanvasController2> ();
		anim2 = minutes.GetComponent<Animator> ();
		anim3 = signal.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim1.enabled = true;
		anim2.enabled = true;
		anim3.enabled = true;
		StartCoroutine (setTimerCountdown());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator setTimerCountdown(){
		anim1.Play ("font_number_sprite_", 0, (((finishRace.timer-1) % 10) / 10f));
		anim1.speed = 0f;
		anim2.Play ("TimerAnim", 0, (Mathf.Floor ((finishRace.timer-1) / 10) / 10f));
		anim2.speed = 0f;
		soundPlayer.clip = countdownSound;
		soundPlayer.Play ();
		for (int i = 0; i <= signalTimer; i++) {

			//Debug.Log("Animation is at " + (i/(signalTimer+1)));
			float fraction = Mathf.Clamp((i/(signalTimer+1)),0,1);
			anim3.Play("Signals_",0,fraction);
			anim3.speed = 0f;
			if(i==signalTimer){
				cc.startLevel ();
			}

			yield return new WaitForSeconds (1f);

		}
		Destroy (signal);

	}
}
