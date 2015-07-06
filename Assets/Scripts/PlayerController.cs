using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//public float timeBlocks = 5f;
	public float force = 10f;
	public float fuelTime = 10.0f;
	public float initialSpeed = 0.5f;
	public Slider slider;
	public bool gameOver;
	public GameObject arrow;
	private Rigidbody2D objRigidbody;
//	float timer;
	//GameObject arrow;
	// Use this for initialization
	void Start () {
		objRigidbody = GetComponent<Rigidbody2D> ();
		objRigidbody.velocity = new Vector2 (initialSpeed, 0f);
		slider.maxValue = fuelTime;
		//arrow = GameObject.FindGameObjectWithTag ("ForwardArrow");
		arrow.SetActive (false);
//		timer = (fuelTime / timeBlocks);
	}
	
	// Update is called once per frame
	void Update () {

		animateWheels();

//		if (Input.GetKey (KeyCode.Space) && fuelTime >= 0 && timer > (fuelTime/timeBlocks) && !gameOver ) {
//			timer = 0f;
//		} 
	}

	void accelerate(float value){
		objRigidbody.AddForce (new Vector2 (value, 0f));
		GameObject[] wheels = GameObject.FindGameObjectsWithTag ("Wheels");
		//StartCoroutine(arrowEffect());
		animateWheels ();
	}


	void FixedUpdate(){
//		timer += Time.fixedDeltaTime;

		if (Input.GetKey (KeyCode.Space) && fuelTime >= 0 && !gameOver /* timer <(fuelTime / timeBlocks) */) {
			fuelTime -= Time.fixedDeltaTime;
			slider.value = fuelTime;
			arrow.SetActive (true);
			accelerate (force);
		} else {
			arrow.SetActive (false);
		}
	}


	void animateWheels(){
		GameObject[] wheels = GameObject.FindGameObjectsWithTag ("Wheels");
		for(int i = 0; i<wheels.Length; i++){
			Animator anim = wheels[i].GetComponent<Animator>();
			if(!gameOver){
				anim.enabled = true;
				anim.SetFloat("Speed",(objRigidbody.velocity.x / 1.595f));
				Debug.Log((objRigidbody.velocity.x));                             //debuging...
			}else{
				anim.enabled = false;
			}
		}
	}

	IEnumerator arrowEffect(){
			arrow.SetActive (true);
			yield return new WaitForSeconds(1f);
		arrow.SetActive (false);
	}
}
