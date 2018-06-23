using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowPickupScript : MonoBehaviour {

	private gameManager gameManager;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Player1(Clone)") {
			gameManager.slow(2);
			Destroy(this.gameObject);
		}
		else if (other.gameObject.name == "Player2(Clone)") {
			gameManager.slow(1);
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<gameManager>();
	}
}
