﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Controller : playerControllerGeneral {

	public override void takeInputReversed() {
		float speed;
		if (slowedDown) {
			speed = 3.0f;
		}
		else if (speedIncreased) {
			speed = 6.5f;
		}
		else {
			speed = 5f;
		}
		Vector2 movementVec = new Vector2(0, 0);
		if (Input.GetKey(KeyCode.A)) {
			movementVec += new Vector2(1 * speed, 0);
		}
		if (Input.GetKey(KeyCode.W)) {
			movementVec += new Vector2(0, -1 * speed);
		}
		if (Input.GetKey(KeyCode.S)) {
			movementVec += new Vector2(0, 1 * speed);
		}
		if (Input.GetKey(KeyCode.D)) {
			movementVec += new Vector2(-1 * speed, 0);
		}

		rb.velocity = movementVec;
	}

	public override void takeInput() {
		float speed;
		if (slowedDown) {
			speed = 3.0f;
		}
		else if (speedIncreased) {
			speed = 6.5f;
		}
		else {
			speed = 5f;
		}
		Vector2 movementVec = new Vector2(0, 0);
		if (Input.GetKey(KeyCode.A)) {
			movementVec += new Vector2(-1 * speed, 0);
		}
		if (Input.GetKey(KeyCode.W)) {
			movementVec += new Vector2(0, 1 * speed);
		}
		if (Input.GetKey(KeyCode.S)) {
			movementVec += new Vector2(0, -1 * speed);
		}
		if (Input.GetKey(KeyCode.D)) {
			movementVec += new Vector2(1 * speed, 0);
		}

		rb.velocity = movementVec;
	}
}