using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DVR_Graph_Line : DVR_Graph {
	public LineRenderer graphLine;
	public UILineRenderer uiGraphLine;
	public UILineRenderer uiGraphLine2;

	[Header ("Spawn Dots")]
	public Transform dotParent;
	public GameObject dotPrefab;
	private GameObject[] spawnedDotsA;
	private GameObject[] spawnedDotsB;

	private int positionCount = 0;

	public bool _spawnDots = true;

	// public UnityEngine.UI.Extensions.UILineRenderer uiGraphLine; // Assign Line Renderer in editor
    // public UnityEngine.UI.Text XValue; // Test Input field to supply new X Value
    // public UnityEngine.UI.Text YValue; // Test Input field to supply new Y Value

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

	// Use this for initialization
	void Start () {
		base.InitializeGraph();

		StartGraph();
	}
	
	// Update is called once per frame
	void Update () {
		base.UpdateGraph();
	}

	protected override void StartGraph() {
		if(spawnedDotsA != null) {
			for(int i = 0; i < spawnedDotsA.Length; i++) {
				Destroy(spawnedDotsA[i].gameObject);
			}
		}

		if(spawnedDotsB != null) {
			for(int i = 0; i < spawnedDotsB.Length; i++) {
				Destroy(spawnedDotsB[i].gameObject);
			}
		}

		spawnedDotsA = new GameObject[maxPositions];
		spawnedDotsB = new GameObject[maxPositions];
		positionCount = 0;
		graphLine.positionCount = positionCount;
		// uigraphLine.positionCount = 0;

		base.StartGraph();
		
		if(demoMode)
			currentYPercentage += 0.1f;
	}

	public override void AddData(float yPercentage) {
		AddData(yPercentage, 0);
	}

	public override void AddData(float yPercentage, int dataID = 0) {
		base.AddData(yPercentage, dataID);
		positionCount++;
		graphLine.positionCount = positionCount;
		// uigraphLine.positionCount = uigraphLine.positionCount + 1;
		Debug.Log("Adding new point");
		float newY = marginY + maxY * yPercentage;
		float newX = 0f;
		Vector3 localSpawnPos;
		
		// graphLine.SetPosition(currentPosition, localSpawnPos);
		if(dataID == 0) {
			newX = marginX + currentPositionA * xStep;
			localSpawnPos = new Vector3(newX, newY, 0f);
			AddNewPoint(localSpawnPos, uiGraphLine);
			spawnedDotsA[currentPositionA] = AddDot(localSpawnPos);
			currentPositionA ++;
		} else {
			newX = marginX + currentPositionB * xStep;
			localSpawnPos = new Vector3(newX, newY, 0f);
			AddNewPoint(localSpawnPos, uiGraphLine2);
			spawnedDotsB[currentPositionB] = AddDot(localSpawnPos);
			currentPositionB ++;
		}
	}

	public GameObject AddDot(Vector3 localSpawnPos) {
		if(_spawnDots && dotPrefab != null) {
			GameObject tempObject = (GameObject)Instantiate(dotPrefab, localSpawnPos, Quaternion.identity, dotParent);
			if(tempObject != null) {
				// spawnedDotsA[currentPositionA] = tempObject;
				tempObject.transform.localPosition = localSpawnPos;
				return tempObject;
			}
		}
		return null;
	}

    // Use this for initialization
    public void AddNewPoint (Vector2 spawnPos, UILineRenderer uILineRenderer) {
        List<Vector2> pointlist = new List<Vector2>(uILineRenderer.Points);
        pointlist.Add(spawnPos);
        uILineRenderer.Points = pointlist.ToArray();
    }
}
