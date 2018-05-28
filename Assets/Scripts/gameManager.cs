using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject ballPrefab;
	public GameObject player1Prefab;
	public GameObject player2Prefab;
	public GameObject blockPickupPrefab;
	public GameObject reversePickupPrefab;
	public GameObject speedUpPickupPrefab;
	public GameObject magnetizePickupPrefab;
	public GameObject slowPickupPrefab;

	public GameObject rightBlockWall;
	public GameObject leftBlockWall;

	public float initFloorXPos = -15.5f;
	public float initFloorYPos = 15.5f;

	public float initWallXPos = -13.5f;
	public float initWallYPos = 5f;

	public int numX;
	public int numY;

	public Text player1ScoreText;
	public Text player2ScoreText;

	public AudioClip goal;
	public AudioClip powerup;

	private Transform wallHolder;
	private List<GameObject> walls = new List<GameObject>();

	private GameObject ballInstance;
	private GameObject player1Instance;
	private GameObject player2Instance;

	private float gameTimer = 0.0f;

	private int player1Score = 0;
	private int player2Score = 0;

	//private bool pickupSpawned = false;
	private float spawnPickupTimer;
	private GameObject prevPickup;

	private float wallShiftTimer = 0.0f;
	private bool wallsShifting = false;
	private int wallShiftCount = 0;

	private float wallBlockTimer = 0.0f;

	private bool player1Magnetized = false;
	private bool player2Magnetized = false;
	private float magnetizeTimer;

	private AudioSource efx;

	void shiftWallsRight() {
		Vector3 curPos;
		for (int i = 0; i < walls.Count; i++) {
			curPos = walls[i].transform.position;
			if (curPos.y == initWallYPos && curPos.x < (initWallXPos + numX)) {
				curPos += new Vector3(1f, 0, 0);
			}
			else if (curPos.x == initWallXPos + numX && curPos.y > -initWallYPos) {
				curPos += new Vector3(0, -1f, 0);
			}
			else if (curPos.y == -initWallYPos && curPos.x > initWallXPos) {
				curPos += new Vector3(-1f, 0, 0);
			}
			else if (curPos.x == initWallXPos && curPos.y < initWallYPos) {
				curPos += new Vector3(0, 1f, 0);
			}
			walls[i].transform.position = curPos;
		}
	}

	void shiftWallsLeft() {
		Vector3 curPos;
		for (int i = 0; i < walls.Count; i++) {
			curPos = walls[i].transform.position;
			if (curPos.y == initWallYPos && curPos.x > initWallXPos) {
				curPos += new Vector3(-1f, 0, 0);
			}
			else if (curPos.x == initWallXPos + numX && curPos.y < initWallYPos) {
				curPos += new Vector3(0, 1f, 0);
			}
			else if (curPos.y == -initWallYPos && curPos.x < initWallXPos + numX) {
				curPos += new Vector3(1f, 0, 0);
			}
			else if (curPos.x == initWallXPos && curPos.y > -initWallYPos) {
				curPos += new Vector3(0, -1f, 0);
			}
			walls[i].transform.position = curPos;
		}
	}

	void generateWalls() {
		for (int i = 0; i < numX; i++) {
			GameObject created = Instantiate(wallPrefab, new Vector3(initWallXPos + i, initWallYPos, 0), Quaternion.identity);
			walls.Add(created);
			created.transform.SetParent(wallHolder);
		}
		for (int i = numX; i < numY + numX; i++) {
			GameObject created = Instantiate(wallPrefab, new Vector3(initWallXPos + numX, initWallYPos - (i - numX), 0), Quaternion.identity);
			walls.Add(created);
			created.transform.SetParent(wallHolder);
		}
		for (int i = (numY + numX); i < (numY + (2 * numX) + 1); i++) {
			GameObject created = Instantiate(wallPrefab, new Vector3((initWallXPos + numX) - (i - (numY + numX)), -initWallYPos, 0), Quaternion.identity);
			walls.Add(created);
			created.transform.SetParent(wallHolder);
		}
		for (int i = (numY + (2 * numX) + 1); i < (2 * numX) + (2 * numY); i++) {
			GameObject created = Instantiate(wallPrefab, new Vector3(initWallXPos, -initWallYPos + (i - (numY + (2 * numX) + 1)), 0), Quaternion.identity);
			walls.Add(created);
			created.transform.SetParent(wallHolder);
		}
	}

	void updatePlayerScores() {
		player1ScoreText.text = "" + player1Score;
		player2ScoreText.text = "" + player2Score;
	}

	public void slow(int playerNum) {
		if (playerNum == 1) {
			player1Instance.GetComponent<player1Controller>().slowDown();
		}
		else {
			player2Instance.GetComponent<player2Controller>().slowDown();
		}
		efx.clip = powerup;
		efx.Play();
	}

	public void magnetize(int playerNum) {
		if (playerNum == 1) {
			player1Magnetized = true;
		}
		else {
			player2Magnetized = true;
		}
		magnetizeTimer = 5.0f;
		efx.clip = powerup;
		efx.Play();
	}

	public void speedUp(int playerNum) {
		if (playerNum == 1) {
			player1Instance.GetComponent<player1Controller>().speedUp();
		}
		else {
			player2Instance.GetComponent<player2Controller>().speedUp();
		}
		efx.clip = powerup;
		efx.Play();
	}

	public void reverse(int playerNum) {
		if (playerNum == 1) {
			player1Instance.GetComponent<player1Controller>().reverse();
		}
		else {
			player2Instance.GetComponent<player2Controller>().reverse();
		}
		efx.clip = powerup;
		efx.Play();
	}

	public void block(int wallNum) {
		if (wallNum == 1) {
			leftBlockWall.SetActive(true);
		}
		else {
			rightBlockWall.SetActive(true);
		}
		wallBlockTimer = 10.0f;
		efx.clip = powerup;
		efx.Play();
	}

	public void resetPositions() {
		ballInstance.transform.position = new Vector3(0, 0, 0);
		ballInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		ballInstance.GetComponent<Rigidbody2D>().angularVelocity = 0;

		player1Instance.transform.position = new Vector3(-5, 0, 0);
		player1Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		player1Instance.GetComponent<Rigidbody2D>().angularVelocity = 0;

		player2Instance.transform.position = new Vector3(5, 0, 0);
		player2Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		player2Instance.GetComponent<Rigidbody2D>().angularVelocity = 0;
	}

	public void player1Goal() {
		player1Score++;
		updatePlayerScores();
		resetPositions();
		efx.clip = goal;
		efx.Play();
	}

	public void player2Goal() {
		player2Score++;
		updatePlayerScores();
		resetPositions();
		efx.clip = goal;
		efx.Play();
	}

	// void generateLevel() {
	// 	GameObject created;
	// 	for (int i = 0; i < numX; i++) {
	// 		for (int j = 0; j < numY; j++) {
	// 			created = Instantiate(floorPrefab, new Vector3(initFloorXPos + i, initFloorYPos - j, 0), Quaternion.identity);
	// 			created.transform.SetParent(level);
	// 		}
	// 	}
	// }

	void openOriginalGoals() {
		walls[numX + (numY / 2) - 2].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
		walls[numX + (numY / 2)].SetActive(false);
		walls[numX + (numY / 2) - 1].SetActive(false);
		walls[numX + (numY / 2) + 1].SetActive(false);
		walls[numX + (numY / 2) + 2].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

		walls[(2 * numX) + (numY + 1) + (numY / 2) - 2].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
		walls[(2 * numX) + (numY + 1) + (numY / 2)].SetActive(false);
		walls[(2 * numX) + (numY + 1) + (numY / 2) - 1].SetActive(false);
		walls[(2 * numX) + (numY + 1) + (numY / 2) + 1].SetActive(false);
		walls[(2 * numX) + (numY + 1) + (numY / 2) + 2].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
	}

	void updateWallShiftTimer() {
		wallShiftTimer += Time.deltaTime;

		if (wallShiftTimer > 0.3 && wallsShifting) {
			if (wallShiftCount < 10) {
				wallShiftCount++;
				shiftWallsRight();
			}
			else if (wallShiftCount < 30) {
				wallShiftCount++;
				shiftWallsLeft();
			}
			else if (wallShiftCount < 40) {
				wallShiftCount++;
				shiftWallsRight();
			}
			else {
				//wallsShifting = false;
				wallShiftCount = 0;
			}
			wallShiftTimer = 0.0f;
		}
	}

	void updateWallBlockTimer() {
		wallBlockTimer -= Time.deltaTime;

		if (wallBlockTimer < 0.01f) {
			rightBlockWall.SetActive(false);
			leftBlockWall.SetActive(false);
			wallBlockTimer = 0.0f;
		}
	}

	void spawnPickup() {
		// if (prevPickup != null) {
		// 	Destroy(prevPickup);
		// }
		float randomNum = Random.Range(0.0f, 2.5f);
		GameObject pickupToSpawn;

		if (randomNum > 2.0f) {
			pickupToSpawn = slowPickupPrefab;
		}
		else if (randomNum > 1.5f) {
			pickupToSpawn = magnetizePickupPrefab;
		}
		else if (randomNum > 1.0f) {
			pickupToSpawn = speedUpPickupPrefab;
		}
		else if (randomNum > 0.5f) {
			pickupToSpawn = reversePickupPrefab;
		}
		else {
			pickupToSpawn = blockPickupPrefab;
		}
		prevPickup = Instantiate(pickupToSpawn, new Vector3(Random.Range(-11.5f, 11.5f), Random.Range(-3.5f, 3.5f), 0), Quaternion.identity);
	}

	void updatePickupTimer() {
		spawnPickupTimer -= Time.deltaTime;

		if (spawnPickupTimer < 0.01f) {
			spawnPickup();
			spawnPickupTimer = 10.0f;
		}
	}

	void updateMagnetizeTimer() {
		magnetizeTimer -= Time.deltaTime;

		if (magnetizeTimer < 0.01f) {
			player1Magnetized = false;
			player2Magnetized = false;
			magnetizeTimer = 0.0f;
		}
	}

	void updateGameTimer() {
		gameTimer += Time.deltaTime;

		// if ((int)gameTimer % 45 > -0.1f && (int)gameTimer % 45 < 0.1f) {
		// 	wallsShifting = true;
		// }
	}

	void initSpawn() {
		player1Instance = Instantiate(player1Prefab, new Vector3(-5, 0, 0), Quaternion.identity);
		player2Instance = Instantiate(player2Prefab, new Vector3(5, 0, 0), Quaternion.identity);
		ballInstance = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}

	void controlBall() {
		Vector3 magForce;
		float intensity = 3;
		if (player1Magnetized) {
			magForce = player1Instance.transform.position - ballInstance.transform.position;
			magForce *= intensity;
			ballInstance.GetComponent<Rigidbody2D>().AddForce(magForce);
		}
		else if (player2Magnetized) {
			magForce = player2Instance.transform.position - ballInstance.transform.position;
			magForce *= intensity;
			ballInstance.GetComponent<Rigidbody2D>().AddForce(magForce);
		}
	}

	// Use this for initialization
	void Start () {
		wallHolder = new GameObject ("Walls").transform;
		efx = GetComponent<AudioSource>();
		//generateLevel();
		generateWalls();

		initSpawn();

		openOriginalGoals();

		wallsShifting = true;

		spawnPickupTimer = 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		updateGameTimer();
		if (wallBlockTimer > 0.0f) {
			updateWallBlockTimer();
		}
		updatePickupTimer();
		updateWallShiftTimer();
		updateMagnetizeTimer();

		controlBall();
	}
}
