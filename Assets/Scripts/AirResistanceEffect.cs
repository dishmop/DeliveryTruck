using UnityEngine;
using System.Collections;

public class AirResistanceEffect : MonoBehaviour {
	
	public float dragCoefficient = 1f;
	public GameObject arrow;
	
	// Use this for initialization
	void Start () {

		arrow.SetActive (false);
	}


	void OnTriggerEnter2D(Collider2D coll){
		
		arrow.SetActive (true);
	}

	void OnTriggerStay2D(Collider2D coll){
		addDragForce (coll.gameObject.GetComponent<Rigidbody2D> ());
		
	}

	void OnTriggerExit2D(Collider2D coll){
		
		arrow.SetActive (false);
	}
	
	void addDragForce(Rigidbody2D body){
		Debug.Log("Air resistance " + ( -dragCoefficient * body.velocity.x ));
		body.AddForce (-dragCoefficient * body.velocity);
		
	}
}
