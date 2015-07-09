using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//public float timeBlocks = 5f;
	public Text lowFuelText;
	public Image runOutOfFuelImage;
	public AudioClip acceleratingSound;
	public AudioClip brakingSound;
	public AudioClip movingSound;
	public float volumeFactor = 0.4f;
	public float pitchFactor = 0.4f;
	public AudioSource camereaAudioSource;
	public float force = 10f;
	public float fuelTime = 10.0f;
	public float initialSpeed = 0.5f;
	public float flashSpeed = 2f;
	public Slider slider;
	public bool gameOver;
	public GameObject arrow;
	public Color flashColour = new Color(1f,0f,0f,0.5f);


	private Rigidbody2D objRigidbody;
	private AudioSource playerAudioSource;
	private float initialVolume;
	private float initialPitch;
	private bool outOfFuel;

//-------------------------------------------------------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start () {
		objRigidbody = GetComponent<Rigidbody2D> ();
		objRigidbody.velocity = new Vector2 (initialSpeed, 0f);

		slider.maxValue = fuelTime;
		slider.value = fuelTime;

		arrow.SetActive (false);

		playerAudioSource = GetComponent<AudioSource> ();
		initialVolume = playerAudioSource.volume;
		initialPitch = playerAudioSource.pitch;


		//timer = (fuelTime / timeBlocks);
	}
	
	// Update is called once per frame

//-------------------------------------------------------------------------------------------------------------------------------------------------
	void Update () {

		if ( (Mathf.Round(slider.value * 10)/10f ) == (Mathf.Round((slider.maxValue / 4f)* 10)/10f)) {
			StartCoroutine(showText());
		}

		if (!outOfFuel && fuelTime <= 0) {
			runOutOfFuelImage.color = flashColour;
			outOfFuel = true;
		} else {
			runOutOfFuelImage.color = Color.Lerp(runOutOfFuelImage.color, Color.clear,flashSpeed * Time.deltaTime); 
		}



		animateWheels();

//		if (Input.GetKey (KeyCode.Space) && fuelTime >= 0 && timer > (fuelTime/timeBlocks) && !gameOver ) {
//			timer = 0f;
//		} 
	}

	void accelerate(float value){
		objRigidbody.AddForce (new Vector2 (value, 0f));
		camereaAudioSource.PlayOneShot (acceleratingSound);
		animateWheels ();
	}

//-------------------------------------------------------------------------------------------------------------------------------------------------
	void FixedUpdate(){

		if (Input.GetKey (KeyCode.Space) && fuelTime >= 0 && !gameOver /* timer <(fuelTime / timeBlocks) */) {
			fuelTime -= Time.fixedDeltaTime;
			slider.value = fuelTime;
			arrow.SetActive (true);
			accelerate (force);
		} else {
			arrow.SetActive (false);
		}
	}

//-------------------------------------------------------------------------------------------------------------------------------------------------
	IEnumerator showText(){
		lowFuelText.text = "Low fuel!";
		yield return new WaitForSeconds (2f);
		lowFuelText.text = "";
	}

//-------------------------------------------------------------------------------------------------------------------------------------------------
	void animateWheels(){
		if (objRigidbody.velocity.x > 0) {
		
			playerAudioSource.volume = initialVolume + objRigidbody.velocity.x * volumeFactor;
			playerAudioSource.pitch = initialPitch + objRigidbody.velocity.x * pitchFactor;
			playerAudioSource.PlayOneShot (movingSound);

		} else {
			playerAudioSource.Stop();
		}

		GameObject[] wheels = GameObject.FindGameObjectsWithTag ("Wheels");
		for(int i = 0; i<wheels.Length; i++){
			Animator anim = wheels[i].GetComponent<Animator>();
			if(!gameOver && objRigidbody.velocity.x > 0){
				anim.enabled = true;
				anim.SetFloat("Speed",(objRigidbody.velocity.x / 1.595f));
				//Debug.Log((objRigidbody.velocity.x));                             //debuging...
			}else{
				anim.enabled = false;
			}
		}
	}

//-------------------------------------------------------------------------------------------------------------------------------------------------
	IEnumerator arrowEffect(){
			arrow.SetActive (true);
			yield return new WaitForSeconds(1f);
		arrow.SetActive (false);
	}
	//-------------------------------------------------------------------------------------------------------------------------------------------------




}

	
