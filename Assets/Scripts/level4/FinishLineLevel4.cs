
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishLineLevel4 : MonoBehaviour {

	public GameObject arrow;
	public GameObject airResistance;
	public GameObject nextRestartButton;
	public Text NRbuttonText;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public AudioClip gameOverSound;
	public AudioClip winingSound;
	public AudioClip WindSound;
	public AudioClip brakingSound;
	public int timer = 12;
	public GameObject minutes;
	public GameObject seconds;
	public float slowingRate = 5f;
	public float winningSpeed = 3.813607f;
	public float allowedError = 0.15f;
	public float stoppingDistance = 6.94f;
	public int windStartingTime = 34;
	public int windDuration = 12;

	Animator anim1;
	Animator anim2;
	AudioSource soundPlayer;
	bool hasWon;
	bool overSpeeding;
	bool isInside;
	bool setScore;
	float finialSpeed;
	float dragForce;
	Rigidbody2D ObjRigidbody;
	int currentScore=0;
	// Use this for initialization
	void Start () {

		ObjRigidbody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();

		soundPlayer = GetComponent<AudioSource> ();

		dragForce = Mathf.Pow ((winningSpeed + allowedError), 2f) / (2 * stoppingDistance);

		anim2 = minutes.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim1.enabled = true;
		anim2.enabled = true;
		
		gameOverPanel.SetActive (false);
		
		StartCoroutine (setTimerCountdown());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isInside) {
			StopCoroutine(setTimerCountdown());
			if(ObjRigidbody.velocity.x >0){
				Vector2 velocity = ObjRigidbody.velocity;
				velocity.x -= dragForce*Time.fixedDeltaTime;
				ObjRigidbody.velocity = velocity;
			}else{
				StartCoroutine(stop(GameObject.FindGameObjectWithTag ("Player")));
				if(!hasWon){
					soundPlayer.PlayOneShot(winingSound);
				}

				hasWon = true;
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			GameObject.FindGameObjectWithTag("Body").GetComponent<BoxCollider2D>().enabled = true;
			isInside = true;
			StopCoroutine(setTimerCountdown());

		}
	}
	
	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Body") {
			Debug.Log ("OverSpeeeeeeeeding.");
			isInside = false;
			overSpeeding = true;
			StartCoroutine (stop (GameObject.FindGameObjectWithTag ("Player")));
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		} 

	}
	
	IEnumerator stop(GameObject obj){
		Rigidbody2D ObjRigidbody = obj.GetComponent<Rigidbody2D> ();
		PlayerController playerController = obj.GetComponent<PlayerController> ();
		playerController.gameOver = true;

		soundPlayer.time = 2f;

		if (ObjRigidbody.velocity.x > 0) {
			ObjRigidbody.drag = slowingRate;
			soundPlayer.PlayOneShot(brakingSound);
			anim1.enabled = false;
			anim2.enabled = false;
		}

		yield return new WaitForSeconds (1f);
		//Debug.Log ("Before activating the panel.");
		gameOverPanel.SetActive (true);
		//Debug.Log ("After activating the panel.");
		if (hasWon) {
			//gameOverText.color = Color.blue;
			if(!setScore){
				currentScore = PlayerController.getScore();
				setScore = true;
			}
			int bonusScore = Mathf.RoundToInt(10f * GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().fuelTime);
			int totalScore = currentScore + 100 + bonusScore;
			gameOverText.text = "Congratulations!\nYou have completed the level successfully.\n" +
				"Previous Score: " + currentScore + "\n" +
					"This level Score: 100\n" +
					"Bonus Score: " + bonusScore +"\n" +
					"Total Score: " + totalScore;
			PlayerController.updateScore(totalScore);
			NRbuttonText.text = "Next Level";
		} else {
			//gameOverText.color = new Color((73f/255f), (21f/255f), (37f/255f));
			if(overSpeeding){
				gameOverText.text = "Your are moving too fast :(\n" +
					"Try again.";
			}else{
				gameOverText.text = "You ran out of time :(\n" +
					"Try again.";
			}

			NRbuttonText.text = "Restart";
		}
	}


	IEnumerator setTimerCountdown(){
		for (int i = timer-1; i >= 0; i--) {
			
			if(hasWon){
				break;
			}
			
			//Debug.Log("Animation1 is at " +((i % 10)/10f) );
			anim1.Play("font_number_sprite_",0,((i % 10)/10f)) ;
			anim1.speed = 0f;
			anim2.Play("TimerAnim",0,(Mathf.Floor (i / 10)/10f));
			anim2.speed = 0f;
			//*********************************************************
 			if(i == windStartingTime){
				airResistance.SetActive(true);
				soundPlayer.clip = WindSound;
				soundPlayer.loop = true;
				soundPlayer.Play();
			}
			if(i == (windStartingTime - windDuration)){
				airResistance.SetActive(false);
				soundPlayer.Stop();
				soundPlayer.loop = false;
				arrow.SetActive(false);
			}
			//*********************************************************

			yield return new WaitForSeconds(1f);
		}
		if (!hasWon) {

			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			StartCoroutine(stop(player));
			yield return new WaitForSeconds (1f);
			if(!hasWon){
				soundPlayer.PlayOneShot(gameOverSound);
			}

		}
		
	}
	
	
	public void gameOver(){
		if (hasWon) {
			if(Application.loadedLevel == 6){
				Application.LoadLevel(1);
			}else{
				Application.LoadLevel(Application.loadedLevel + 1);
			}

		} else {
			Application.LoadLevel(Application.loadedLevel);
		}
		
		
	}
	
	
}
