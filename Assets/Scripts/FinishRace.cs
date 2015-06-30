using UnityEngine;
using System.Collections;

public class FinishRace : MonoBehaviour {

	public int timer = 12;
	public GameObject minutes;
	public GameObject seconds;
	public float slowingRate = 5f;
	//bool crossTheFinishLine;
	Animator anim1;
	Animator anim2;
	// Use this for initialization
	void Start () {
		anim2 = minutes.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim2.StartPlayback ();
		anim1.StartPlayback ();
		Debug.Log("About to enter a coroutine");
		StartCoroutine (setTimerCountdown());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			//crossTheFinishLine = true;
			stop(coll.gameObject);
		}
	}

	void stop(GameObject obj){
		Rigidbody2D ObjRigidbody = obj.GetComponent<Rigidbody2D> ();

		ObjRigidbody.drag = slowingRate;


	}
	IEnumerator setTimerCountdown(){
		for (int i = timer; i > 0; i--) {
			Debug.Log("Playback mode " +anim1.recorderMode.ToString() );
			anim1.playbackTime = (timer % 10);
			anim2.playbackTime = Mathf.Floor (timer / 10);
			yield return new WaitForSeconds(1f);
		}
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		stop (player);

	}

}
