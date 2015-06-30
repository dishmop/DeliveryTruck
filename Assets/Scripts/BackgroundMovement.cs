using UnityEngine;
using System.Collections;

public class BackgroundMovement : MonoBehaviour {
	
	public GameObject background;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (background.transform.position.x < gameObject.transform.position.x) {
			float movingDistance = 2 * (Vector2.Distance(gameObject.transform.position, background.transform.position));
			background.transform.Translate(new Vector2(movingDistance,0f));
		}
	}
}
