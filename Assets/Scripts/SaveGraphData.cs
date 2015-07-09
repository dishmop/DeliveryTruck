using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SaveGraphData : MonoBehaviour {

	public GameObject graphPanel;
	Plotting plotting;
	// Use this for initialization
	void Start () {
		plotting = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Plotting> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void save(String filename){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/" + filename, FileMode.Create);
		GraphData graphData = new GraphData ();
		graphData.setData(plotting.getData ());

		bf.Serialize (file, graphData);
		file.Close ();
	}

	public void load(String filename){
		if (File.Exists (Application.dataPath + "/GraphData/" + filename)) {
			BinaryFormatter bf = new BinaryFormatter ();
			//FileStream file = File.Open (Application.persistentDataPath + "/" + filename, FileMode.Open);
			FileStream file = File.Open (Application.dataPath + "/GraphData/" + filename, FileMode.Open);
			GraphData graphData = (GraphData)bf.Deserialize (file);

			UIGraph graph = graphPanel.GetComponent<UIGraph> ();
			graph.setRed (true);
			graph.SetAxesRanges (0f, 100f, 0f, 4f);
			graph.UploadData (graphData.getData ());
		} else {
			Debug.LogError("File not found "+ Application.dataPath + "/GraphData/"+ filename);
		}
	}
}
[Serializable]
class GraphData{
	private float[,] data;

	public void setData(Vector2[] vData){
		data = new float[vData.Length, 2];
		for (int i = 0; i<vData.Length; i++) {
			data[i,0] = vData[i].x;
			data[i,1] = vData[i].y;
		}

	}
	public Vector2[] getData(){
		Vector2[] vData = new Vector2[data.Length / 2];
		for (int i = 0; i<vData.Length; i++) {
			if(i != 0 && data[i,0] == 0.0f){
				vData[i].x = vData[i-1].x;
				vData[i].y = vData[i-1].y;
			}else{
				vData[i].x = data[i,0];
				vData[i].y = data[i,1];
			}

		}
		return vData;
	}
}
