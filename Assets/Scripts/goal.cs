using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour {

	private gameManager gameManager;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			if (other.transform.position.x > 0) {
				gameManager.player1Goal();
			}
			else {
				gameManager.player2Goal();
			}
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<gameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
