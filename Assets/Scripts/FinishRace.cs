using UnityEngine;
using System.Collections;

public class FinishRace : MonoBehaviour {


	public AudioClip gameOverSound;
	public AudioClip winingSound;
	public AudioClip brakingSound;
	public int timer = 12;
	public GameObject minutes;
	public GameObject seconds;
	public float slowingRate = 5f;
	//bool crossTheFinishLine;
	Animator anim1;
	Animator anim2;
	AudioSource soundPlayer;
	// Use this for initialization
	void Start () {
		soundPlayer = GetComponent<AudioSource> ();
		anim2 = minutes.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim1.enabled = true;
		anim2.enabled = true;

		//anim2.speed = 0f;
		//anim1.speed = 0f;

		StartCoroutine (setTimerCountdown());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			//crossTheFinishLine = true;
			stop(coll.gameObject);
			anim1.enabled = false;
			anim2.enabled = false;
			soundPlayer.PlayOneShot(winingSound);
	
		}

	}

	void stop(GameObject obj){
		Rigidbody2D ObjRigidbody = obj.GetComponent<Rigidbody2D> ();
		PlayerController playerController = obj.GetComponent<PlayerController> ();
		playerController.gameOver = true;
		ObjRigidbody.drag = slowingRate;
		soundPlayer.PlayOneShot(brakingSound);


	}
	IEnumerator setTimerCountdown(){
		for (int i = timer-1; i >= 0; i--) {
			//Debug.Log("Animation1 is at " +((i % 10)/10f) );
			anim1.Play("font_number_sprite_",0,((i % 10)/10f)) ;
			anim1.speed = 0f;
			anim2.Play("TimerAnim",0,(Mathf.Floor (i / 10)/10f));
			anim2.speed = 0f;
			yield return new WaitForSeconds(1f);
		}
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		stop (player);
		soundPlayer.PlayOneShot(gameOverSound);
		anim1.enabled = false;
		anim2.enabled = false;

	}

	
}
