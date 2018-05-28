using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerGeneral : MonoBehaviour {

	public Rigidbody2D rb;
	
	private bool reversed;
	private float reverseTimer;

	public bool speedIncreased;
	private float speedTimer;

	public bool slowedDown;
	private float slowTimer;

	public void slowDown() {
		slowedDown = true;
		slowTimer = 6.5f;
	}

	public void reverse() {
		reversed = true;
		reverseTimer = 10.0f;
	}

	public void speedUp() {
		speedIncreased = true;
		speedTimer = 7.0f;
	}

	public virtual void takeInputReversed() {

	}

	public virtual void takeInput() {
		
	}

	void updateReverseTimer() {
		reverseTimer -= Time.deltaTime;

		if (reverseTimer < 0.01f) {
			reversed = false;
		}
	}

	void updateSpeedTimer() {
		speedTimer -= Time.deltaTime;

		if (speedTimer < 0.01f) {
			speedIncreased = false;
		}
	}

	void updateSlowTimer() {
		slowTimer -= Time.deltaTime;

		if (slowTimer < 0.01f) {
			slowedDown = false;
		}
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		reversed = false;
		if (rb == null) {
			Debug.Log("NULL");
		}
	}
	
	void Update () {
		if (reversed) {
			takeInputReversed();
		}
		else {
			takeInput();
		}
		if (reverseTimer > 0.0f) {
			updateReverseTimer();
		}
		if (speedTimer > 0.0f) {
			updateSpeedTimer();
		}
		if (slowTimer > 0.0f) {
			updateSlowTimer();
		}
	}
}