using UnityEngine;
using System.Collections;

public class FrictionEffect : MonoBehaviour {

	public GameObject arrow;
	public float frictionCoefficient = 0.1f;

	bool isInside;
	Rigidbody2D playerRigidbody;

	// Use this for initialization
	

	void Start () {
		arrow.SetActive (false);
		playerRigidbody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isInside) {
			addFriction(playerRigidbody);
		}
	}


	void OnTriggerEnter2D(Collider2D coll){
		isInside = true;
		if (!arrow.activeSelf) {
			arrow.SetActive (true);
		}

	}

//	void OnTriggerStay2D(Collider2D coll){
//		addFriction (coll.gameObject.GetComponent<Rigidbody2D> ());
//		
//	}


	void OnTriggerExit2D(Collider2D coll){
		isInside = false;
		arrow.SetActive (false);
	}

	void addFriction(Rigidbody2D body){

		if(!arrow.activeInHierarchy){
			arrow.SetActive (true);
		}
		if (body.velocity.x > 0) {
			//Debug.Log("About to give a force " + (-frictionCoefficient * body.mass * Mathf.Abs(Physics2D.gravity.y)) );
			body.AddForce (-frictionCoefficient * body.mass * Mathf.Abs(Physics2D.gravity.y) * body.velocity.normalized);
		}else {
			arrow.SetActive (false);
		}
	}
}
