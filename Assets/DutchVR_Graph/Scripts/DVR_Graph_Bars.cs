using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DVR_Graph_Bars : DVR_Graph {
	// public LineRenderer graphLine;
	public Transform barParent;
	public GameObject barPrefab;
	private GameObject[] spawnedBars;
	public float barWidth = 10f;

	// public float marginPercentage = 0.05f;
	// public float marginX = 0;
	// public float marginY = 0;

	// private float maxY = 1000f;
	// private float maxX = 1000f;
	// private float xStep = 100f;
	// private int currentPosition = 0;
	// public int maxPositions = 10;

	// private float timer = 0f;
	// private float timerMax = 1f;
	// private float currentYPercentage = 0f;
	
	void Start () {
		base.InitializeGraph();
		StartGraph();
	}
	
	// Update is called once per frame
	void Update () {
		base.UpdateGraph();
	}

	protected override void StartGraph() {
		if(spawnedBars != null) {
			for(int i = 0; i < spawnedBars.Length; i++) {
				Destroy(spawnedBars[i].gameObject);
			}
		}

		spawnedBars = new GameObject[maxPositions];

		base.StartGraph();
		
		if(demoMode)
			currentYPercentage += 0.1f;
	}

	public override void AddData(float yPercentage) {
		base.AddData(yPercentage);
		Debug.Log("Adding new bar");
		float newY = maxY * yPercentage;
		float newX = marginX + currentPositionA * (xStep + 0.5f);
		Vector3 localSpawnPos = new Vector3(newX, marginY, 0f);
		spawnedBars[currentPositionA] = (GameObject)Instantiate(barPrefab, localSpawnPos, Quaternion.identity, barParent);
		spawnedBars[currentPositionA].transform.localPosition = localSpawnPos;
		RectTransform currentRect = spawnedBars[currentPositionA].GetComponent<RectTransform>();
		currentRect.sizeDelta = new Vector2(barWidth, 0);
		currentRect.DOSizeDelta(new Vector2(barWidth, newY), 0.5f);
		currentPositionA ++;
	}
}
