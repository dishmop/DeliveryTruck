using UnityEngine;
using System.Collections;

public class FrictionEffect : MonoBehaviour {

	public GameObject arrow;
	public float frictionCoefficient = 0.1f;
	
	// Use this for initialization
	

	void Start () {
		arrow.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){

		arrow.SetActive (true);
	}

	void OnTriggerStay2D(Collider2D coll){
		addFricition (coll.gameObject.GetComponent<Rigidbody2D> ());
		
	}
	void OnTriggerExit2D(Collider2D coll){
		
		arrow.SetActive (false);
	}

	void addFricition(Rigidbody2D body){

		if(!arrow.activeInHierarchy){
			arrow.SetActive (true);
		}
		if (body.velocity.x > 0) {
			Debug.Log("About to give a force " + (-frictionCoefficient * body.mass * Mathf.Abs(Physics2D.gravity.y)) );
			body.AddForce (-frictionCoefficient * body.mass * Mathf.Abs(Physics2D.gravity.y) * body.velocity.normalized);
		}else {
			arrow.SetActive (false);
		}
	}
}
