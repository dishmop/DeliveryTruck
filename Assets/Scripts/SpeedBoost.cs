using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour {

	public float speedJump = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		Rigidbody2D	collRigidbody = coll.gameObject.GetComponent<Rigidbody2D> ();
		collRigidbody.velocity = new Vector2 ((collRigidbody.velocity.x + speedJump), collRigidbody.velocity.y);
		Destroy (gameObject); 
	}
}
