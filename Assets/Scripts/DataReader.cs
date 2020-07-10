using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataReader : MonoBehaviour
{
    public string fileName1;
    public string fileName2;

    private string fileLocation;
    private string seperator = ";";

    public List<RangePayload> rangePayloadList = new List<RangePayload>();
    public List<AircraftAerodynamics> aircraftAerodynamicsList = new List<AircraftAerodynamics>();

    public DVR_Graph_Line lineGraph;

    // Start is called before the first frame update
    void Start()
    {
        TestCSV();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestCSV() {
        fileLocation = Application.streamingAssetsPath + "//" + fileName1;
        aircraftAerodynamicsList = new List<AircraftAerodynamics>();
        StartCoroutine(ReadAircraftAerodynamics());

        fileLocation = Application.streamingAssetsPath + "//" + fileName2;
        rangePayloadList = new List<RangePayload>();
        StartCoroutine(ReadPayloadCSV());
        
        StartCoroutine(CreatePayloadGraph());
    }

    IEnumerator CreatePayloadGraph() {
        float ymax = 120000f;
        for(int i = 0; i < rangePayloadList.Count; i++) {
            if(lineGraph != null) {
                lineGraph.AddData(rangePayloadList[i].fuel/ymax, 0);
                lineGraph.AddData(rangePayloadList[i].payload/ymax, 1);
            }
            yield return null;
        }
    }

    IEnumerator ReadPayloadCSV() {
        if(File.Exists(fileLocation)) {
            bool skippedFirst = false;
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(fileLocation)) 
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null) 
                {
                    if(!skippedFirst) {
                        skippedFirst = true;
                    } else {
                        // Debug.Log(line);
                        string[] csvArray = line.Split(new string[] {seperator}, System.StringSplitOptions.None);

                        RangePayload payloadData = new RangePayload();
                        int.TryParse(csvArray[0], out payloadData.range);
                        int.TryParse(csvArray[1], out payloadData.fuel);
                        int.TryParse(csvArray[2], out payloadData.payload);
                        int.TryParse(csvArray[4], out payloadData.epow);
                        rangePayloadList.Add(payloadData);
                        yield return null;
                    }
                }
            }
        } else {
            Debug.Log("File not available");
        }
    }

    IEnumerator ReadAircraftAerodynamics() {
        if(BetterStreamingAssets.FileExists(fileLocation)) {
            bool skippedFirst = false;
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            // using (StreamReader sr = new StreamReader(fileLocation)) 
            // {
            //     string line;
            //     // Read and display lines from the file until the end of 
            //     // the file is reached.
            //     while ((line = sr.ReadLine()) != null) 
            //     {
            //         if(!skippedFirst) {
            //             skippedFirst = true;
            //         } else {
            //             // Debug.Log(line);
            //             string[] csvArray = line.Split(new string[] {seperator}, System.StringSplitOptions.None);

            //             // / alpha;CL;CD;CDp;CM;Top_Xtr;Bot_Xtr;;;;;;;;;;;;;;;
            //             AircraftAerodynamics aircraftData = new AircraftAerodynamics();
            //             float.TryParse(csvArray[0], out aircraftData.alpha);
            //             float.TryParse(csvArray[1], out aircraftData.CL);
            //             float.TryParse(csvArray[2], out aircraftData.CD);
            //             float.TryParse(csvArray[3], out aircraftData.CDp);
            //             float.TryParse(csvArray[4], out aircraftData.CM);
            //             float.TryParse(csvArray[5], out aircraftData.Top_Xtr);
            //             float.TryParse(csvArray[6], out aircraftData.Bot_Xtr);
            //             aircraftAerodynamicsList.Add(aircraftData);
            //             yield return null;
            //         }
            //     }
            // }
            string[] lines = BetterStreamingAssets.ReadAllLines(fileLocation);
            foreach (string line in lines){
                if(!skippedFirst) {
                    skippedFirst = true;
                } else {
                    // Debug.Log(line);
                    string[] csvArray = line.Split(new string[] {seperator}, System.StringSplitOptions.None);

                    // / alpha;CL;CD;CDp;CM;Top_Xtr;Bot_Xtr;;;;;;;;;;;;;;;
                    AircraftAerodynamics aircraftData = new AircraftAerodynamics();
                    float.TryParse(csvArray[0], out aircraftData.alpha);
                    float.TryParse(csvArray[1], out aircraftData.CL);
                    float.TryParse(csvArray[2], out aircraftData.CD);
                    float.TryParse(csvArray[3], out aircraftData.CDp);
                    float.TryParse(csvArray[4], out aircraftData.CM);
                    float.TryParse(csvArray[5], out aircraftData.Top_Xtr);
                    float.TryParse(csvArray[6], out aircraftData.Bot_Xtr);
                    aircraftAerodynamicsList.Add(aircraftData);
                    yield return null;
                }
            }
        } else {
            Debug.Log("File not available");
        }
    }
}

// alpha;CL;CD;CDp;CM;Top_Xtr;Bot_Xtr;;;;;;;;;;;;;;;

[System.Serializable]
public class RangePayload {
    //0
    public int range;
    //1
    public int fuel;
    //2
    public int payload;
    //4
    public int epow;
}

// alpha;CL;CD;CDp;CM;Top_Xtr;Bot_Xtr;;;;;;;;;;;;;;;
[System.Serializable]
public class AircraftAerodynamics {
    //0
    public float alpha;
    //1
    public float CL;
    //2
    public float CD;
    //3
    public float CDp;
    //4
    public float CM;
    //5
    public float Top_Xtr;
    //6
    public float Bot_Xtr;
}
