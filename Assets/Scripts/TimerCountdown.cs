using UnityEngine;
using System.Collections;

public class TimerCountdown : MonoBehaviour {
	
	public GameObject minutes;
	public GameObject seconds;
	public int timer = 4;
	
	Animator anim1;
	Animator anim2;
	CanvasController cc;
	// Use this for initialization
	void Start () {
		cc = gameObject.GetComponent<CanvasController> ();
		anim2 = minutes.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim1.enabled = true;
		anim2.enabled = true;
		StartCoroutine (setTimerCountdown());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator setTimerCountdown(){
		for (int i = timer-1; i >= 0; i--) {
			//Debug.Log("Animation1 is at " +((i % 10)/10f) );
			anim1.Play ("font_number_sprite_", 0, ((i % 10) / 10f));
			anim1.speed = 0f;
			anim2.Play ("TimerAnim", 0, (Mathf.Floor (i / 10) / 10f));
			anim2.speed = 0f;
			yield return new WaitForSeconds (1f);
		}
		cc.startLevel ();
	}
}
