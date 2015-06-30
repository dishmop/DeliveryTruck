using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float force = 10f;
	public float fuelTime = 10.0f;
	public float initialSpeed = 0.5f;
	public Slider slider;
	private Rigidbody2D objRigidbody;
	private bool HasStopped;

	// Use this for initialization
	void Start () {
		objRigidbody = GetComponent<Rigidbody2D> ();
		objRigidbody.velocity = new Vector2 (initialSpeed, 0f);
		slider.maxValue = fuelTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space) && fuelTime >= 0) {

			fuelTime -= Time.deltaTime;
			slider.value = fuelTime;
			accelerate (force);
		} else if (!HasStopped){
			GameObject[] wheels = GameObject.FindGameObjectsWithTag ("Wheels");
			for(int i = 0; i<wheels.Length; i++){
				Animator anim = wheels[i].GetComponent<Animator>();
				anim.enabled = true;
				anim.SetFloat("Speed",(objRigidbody.velocity.x / 1.595f));
				//Debug.Log(wheels[i].name);
				//Debug.Log((objRigidbody.velocity.x / 1.595f));
				if(objRigidbody.velocity.x<=0){
					anim.enabled = false;
					HasStopped = true;
				}
			}
		}
	}

	void accelerate(float value){
		objRigidbody.AddForce (new Vector2 (value, 0f));
		GameObject[] wheels = GameObject.FindGameObjectsWithTag ("Wheels");
		for(int i = 0; i<wheels.Length; i++){
			Animator anim = wheels[i].GetComponent<Animator>();
			anim.enabled = true;
			anim.SetFloat("Speed",(objRigidbody.velocity.x / 1.595f));
			//Debug.Log(wheels[i].name);
			//Debug.Log((objRigidbody.velocity.x / 1.595f));
		}
	}
}
