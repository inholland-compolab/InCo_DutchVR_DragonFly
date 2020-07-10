using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVR_Graph : MonoBehaviour {
	// public LineRenderer graphLine;
	// public Transform dotParent;
	// public GameObject dotPrefab;
	// private GameObject[] spawnedDots;

	// public float marginPercentage = 0.05f;
	public float marginX = 0;
	public float marginY = 0;

	protected float maxY = 100f;
	protected float maxX = 100f;
	protected float xStep = 100f;
	protected int currentPositionA = 0;
	protected int currentPositionB = 0;
	public int maxPositions = 10;

	protected float timer = 0f;
	protected float timerMax = 1f;
	protected float currentYPercentage = 0f;

	public bool demoMode = false;

	// Use this for initialization
	protected virtual void InitializeGraph () {
		Vector2 sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
		maxX = sizeDelta.x;
		maxY = sizeDelta.y;
		// marginX = marginPercentage*maxX;
		// marginY = marginPercentage*maxY;
		maxX -= marginX *2;
		maxY -= marginY *2;
		xStep = maxX/maxPositions;
		// StartGraph();
	}

	// Update is called once per frame
	protected virtual void UpdateGraph () {
		if(demoMode && Input.GetKeyDown(KeyCode.R)) {
			StartGraph();
		}

		if(demoMode) {
			if(currentPositionA < maxPositions) {
				timer += Time.deltaTime;
				if(timer >= timerMax) {
					timer = 0f;
					currentYPercentage = Random.Range(0f, 1f);
					AddData(currentYPercentage);
					Debug.Log("Updating graph at position: " + currentPositionA + ", max Positions: " + maxPositions);
					// currentYPercentage += 0.1f;
				}
			}
		}
	}

	protected virtual void StartGraph() {
		//Initialize graph
		timer = 0;
		currentPositionA = 0;
		currentPositionB = 0;
		currentYPercentage = 0f;

		AddData(currentYPercentage);
	}

	public virtual void AddData(float yPercentage) {
		//Add data to graph
	}

	public virtual void AddData(float yPercentage, int dataID) {
		//Add data to graph
	}
}
