using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishRace : MonoBehaviour {

	public GameObject arrow;
	public GameObject airResistance;
	public GameObject nextRestartButton;
	public Text NRbuttonText;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public AudioClip gameOverSound;
	public AudioClip winingSound;
	public AudioClip brakingSound;
	public AudioClip WindSound;
	public int timer = 12;
	public GameObject minutes;
	public GameObject seconds;
	public float slowingRate = 5f;
	public bool level3 = false;
	public int windStartingTime = 34;
	public int windDuration = 12;

	Animator anim1;
	Animator anim2;
	AudioSource soundPlayer;
	bool hasWon;
	bool setScore;
	int currentScore=0;


	void Awake(){
		gameOverPanel.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		soundPlayer = GetComponent<AudioSource> ();

		anim2 = minutes.GetComponent<Animator> ();
		anim1 = seconds.GetComponent<Animator> ();
		anim1.enabled = true;
		anim2.enabled = true;



		StartCoroutine (setTimerCountdown());
	}
	


	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {

			StartCoroutine(stop(coll.gameObject));
			anim1.enabled = false;
			anim2.enabled = false;

			soundPlayer.PlayOneShot(winingSound);

			hasWon = true;
	
		}

	}

	IEnumerator stop(GameObject obj){
		Rigidbody2D ObjRigidbody = obj.GetComponent<Rigidbody2D> ();
		PlayerController playerController = obj.GetComponent<PlayerController> ();
		playerController.gameOver = true;
		ObjRigidbody.drag = slowingRate;
		soundPlayer.time = 2f;

		if (ObjRigidbody.velocity.x > 0) {
			soundPlayer.PlayOneShot(brakingSound);
		}

		yield return new WaitForSeconds (2f);
		gameOverPanel.SetActive (true);
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
			gameOverText.text = "You ran out of time :(\n" +
				"Try again";
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
			if(level3){
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
			
			}
			//*********************************************************
			yield return new WaitForSeconds(1f);
		}
		if (!hasWon) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			StartCoroutine(stop(player));
			anim1.enabled = false;
			anim2.enabled = false;
			yield return new WaitForSeconds (2f);
			if(!hasWon){
				soundPlayer.PlayOneShot(gameOverSound);
			}

		}

	}


	public void gameOver(){
		if (hasWon) {
			Application.LoadLevel(Application.loadedLevel + 1);
		} else {
			Application.LoadLevel(Application.loadedLevel);
		}

		
	}

	
}
