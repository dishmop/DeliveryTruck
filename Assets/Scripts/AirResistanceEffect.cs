using UnityEngine;
using System.Collections;

public class AirResistanceEffect : MonoBehaviour {
	
	public float dragCoefficient = 1f;
	public GameObject arrow;
	bool isInside;
	Rigidbody2D playerRigidbody;
	// Use this for initialization
	void Start () {
		playerRigidbody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
		arrow.SetActive (false);
	}


	void FixedUpdate(){
		if (isInside) {
			addDragForce(playerRigidbody);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		isInside = true;
		arrow.SetActive (true);
	}

//	void OnTriggerStay2D(Collider2D coll){
//		addDragForce (coll.gameObject.GetComponent<Rigidbody2D> ());
//		
//	}

	void OnTriggerExit2D(Collider2D coll){
		isInside = false;
		arrow.SetActive (false);
	}
	
	void addDragForce(Rigidbody2D body){
		if (body.velocity.x > 0) {
			if(!arrow.activeInHierarchy){
				arrow.SetActive (true);
			}
			//Debug.Log ("Air resistance " + (-dragCoefficient * body.velocity.x));
			body.AddForce (-dragCoefficient * body.velocity);
		} else {
			arrow.SetActive (false);
		}
	}
}
