using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedBoost : MonoBehaviour {


	public Image turpoImg; 
	public float speedJump = 5f;
	bool isActive;
	Rigidbody2D playerRigidbody;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.C) && isActive) {
			playerRigidbody.velocity = new Vector2 ((playerRigidbody.velocity.x + speedJump), playerRigidbody.velocity.y);
			turpoImg.enabled = false;
			Destroy (gameObject); 
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		playerRigidbody = coll.gameObject.GetComponent<Rigidbody2D> ();
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		isActive = true;
		turpoImg.enabled = true;
	}
}
