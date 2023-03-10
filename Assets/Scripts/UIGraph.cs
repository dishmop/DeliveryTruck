using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

public class UIGraph : MonoBehaviour {
	
	public GameObject canvasGO;
	public Color dataCol = Color.green;
	public Color axesCol = Color.clear;
	public Color borderCol = Color.white;
	public Color vCursorCol = Color.clear;
	
	float xAxisMin = -50;
	float xAxisMax = 50;
	float yAxisMin = -2;
	float yAxisMax = 2;
	
	
	//float vCursorSizeProp = 0.2f;
	
	Vector2[] data;
	
	float borderProp = 0.1f;
	Vector2[] borderCorners = new Vector2[4];
	
	Vector3 worldBottomLeft;
	Vector3 worldTopRight;
	
	List<Vector2>	vCursors = new List<Vector2>();
	
	List<VectorLine> lines = new List<VectorLine>();
	
	Vector2 screenRectMin;
	Vector2 screenRectMax;
	float screenRectWidth;
	float screenRectHeight;
	
	
	Vector2 borderRectMin;
	float borderRectWidth;
	float borderRectHeight;
	
	VectorLine axesLine;
	VectorLine borderLine;
	VectorLine dataLine;
	
	Material axesMaterial;
	Material borderMaterial;
	Material dataMaterial;
	
	
	Material vectrosityMaterialPrefab;

	int numElementsToDraw = 0;
	
	
	
	public void UploadData(Vector2[] newData){
		UploadData (newData, -1);
	}
	
	public void UploadData(Vector2[] newData, int numElementsToDraw){
		data = newData.ToArray();
		this.numElementsToDraw = numElementsToDraw;
	}
	
	public void SetAxesRanges(float xMin, float xMax, float yMin, float yMax){
		xAxisMin = xMin;
		xAxisMax = xMax;
		yAxisMin = yMin;
		yAxisMax = yMax;
	}
	
	
	
	public int AddVCursor(){
		vCursors.Add (Vector2.zero);
		return vCursors.Count()-1;
	}
	
	public void SetVCursor(int id, Vector2 pos){
		vCursors[id] = pos;
	}
	
	
	// Use this for initialization
	void Start () {
		
//		Vector2[] data = new Vector2[100];
//		for (int i = 0; i < 100; ++i){
//			data[i] = new Vector2(i-50, Mathf.Sin((float)i / 2));
//		}
		
		
		// Make a prefab from the material the SetLine uses so we can clone it and use it again,. 
		VectorLine tempLine = VectorLine.SetLine(Color.green, Vector2.zero,Vector2.zero);
		vectrosityMaterialPrefab = tempLine.material;
		VectorLine.Destroy(ref tempLine);
		
		
		
		//UploadData(data);
		
	}
	
	
	
	
	// Update is called once per frame
	void LateUpdate () {
		
		
		Rect rect = GetComponent<RectTransform>().rect;
		Vector3 screenRectMin3D = GetComponent<RectTransform>().TransformPoint(new Vector3(rect.min.x, rect.min.y, 0));
		Vector3 screenRectMax3D = GetComponent<RectTransform>().TransformPoint(new Vector3(rect.max.x, rect.max.y, 0));
		
		worldBottomLeft = Camera.main.ScreenToWorldPoint(screenRectMin3D);
		worldTopRight = Camera.main.ScreenToWorldPoint(screenRectMax3D);
		
		screenRectMin = new Vector2 (screenRectMin3D.x, screenRectMin3D.y);
		screenRectMax = new Vector2 (screenRectMax3D.x, screenRectMax3D.y);
		screenRectWidth = screenRectMax.x - screenRectMin.x;
		screenRectHeight = screenRectMax.y - screenRectMin.y;
		
		Vector2 screenCentre = 0.5f * (screenRectMin + screenRectMax);
		borderRectWidth = screenRectWidth * (1-borderProp);
		borderRectHeight =  screenRectHeight * (1-borderProp);
		borderRectMin = screenCentre - new Vector2(0.5f * borderRectWidth, 0.5f * borderRectHeight);
		
		
		worldBottomLeft.z = 0;
		worldTopRight.z = 0;
		
		GenerateBorder();
		
		// Remove all the lines in our list
		foreach (VectorLine line in lines){
			VectorLine thisLine = line;
			VectorLine.Destroy( ref thisLine);
		}
		lines.Clear();
		
		
		
		DrawGraphBorder ();
		DrawAxes();
		DrawData();
		DrawVCursors();
		
		
		
		//		foreach (VectorLine line in lines){
		//			line.Draw();
		//		}
	}
	
	void GenerateBorder(){
		Vector2 centrePos = 0.5f * (screenRectMin + screenRectMax);
		
		borderCorners[0] = screenRectMin;
		borderCorners[1] = new Vector3(screenRectMin.x, screenRectMax.y, 0);
		borderCorners[2] = screenRectMax;
		borderCorners[3] = new Vector3(screenRectMax.x, screenRectMin.y, 0);
		
		for (int i = 0; i < 4; ++i){
			borderCorners[i] -= centrePos;
		}
		for (int i = 0; i < 4; ++i){
			borderCorners[i] *= 1-borderProp;
		}
		
		
		for (int i = 0; i < 4; ++i){
			borderCorners[i] += centrePos;
		}
	}
	
	void DrawLine(Vector2 fromPoint, Vector2 toPoint, Color col){
		VectorLine newLine = VectorLine.SetLine(col, fromPoint, toPoint);
		lines.Add (newLine);
	}
	
	
	void DrawGraphBorder(){
		
		if (borderLine == null){
			Vector2[] points = new Vector2[5];
			for (int i = 0; i < 5; ++i){
				points[i] = borderCorners[i % 4];
			}
			
			borderMaterial = Material.Instantiate(vectrosityMaterialPrefab);
			borderMaterial.color = borderCol;
			borderLine = new VectorLine("Border", points, borderMaterial, 2.0f, LineType.Continuous);
		}
		else{
			for (int i = 0; i < 5; ++i){
				borderLine.points2[i] = borderCorners[i % 4];
			}
			borderMaterial.color = borderCol;
			
		}
		borderLine.Draw();
		
		
	}
	
	void DrawAxes(){
		
		Vector2[] points = new Vector2[4];
		
		// X-Axis
		points[0] = TransformGraphToGraphArea(new Vector2(xAxisMin, 0));
		points[1] = TransformGraphToGraphArea(new Vector2(xAxisMax, 0));
		
		// Y-Axis
		points[2] = TransformGraphToGraphArea(new Vector2(0, yAxisMin));
		points[3] = TransformGraphToGraphArea(new Vector2(0, yAxisMax));
		
		if (axesLine == null){
			axesMaterial = Material.Instantiate(vectrosityMaterialPrefab);
			axesMaterial.color = axesCol;
			axesLine = new VectorLine("Axes", points, axesMaterial, 2.0f, LineType.Discrete);
		}
		else{
			for (int i = 0; i < 4; ++i){
				axesLine.points2[i] = points[i];
			}
			axesMaterial.color = axesCol;
			
		}
		axesLine.Draw();
	}
	
	void DrawData(){
		
		
		
		if (data != null){
			Vector2[] points  = new Vector2[data.Count ()];
			for (int i = 0; i < points.Count (); ++i){
				points[i] = TransformGraphToGraphArea(data[i]);
			}
			
			if (dataLine != null && dataLine.points2.Count() != points.Count ()){
				VectorLine.Destroy(ref dataLine);
				dataLine = null;
				Destroy(dataMaterial);
				dataMaterial = null;
				
			}
			
			if (dataLine == null){
				dataMaterial = Material.Instantiate(vectrosityMaterialPrefab);
				dataMaterial.color = dataCol;
				dataLine = new VectorLine("Data", points, dataMaterial, 2.0f, LineType.Continuous);
			}
			else{
				for (int i = 0; i < data.Count (); ++i){
					dataLine.points2[i] = points[i];
				}
				dataMaterial.color = dataCol;
			}
			if (numElementsToDraw >= 0){
				dataLine.drawEnd = numElementsToDraw;
			}
			else{
				dataLine.drawEnd = dataLine.points2.Count();
			}
			dataLine.Draw ();
			
			
		}
	}
	
	void DrawVCursors(){
		foreach (Vector2 vCursorPos in vCursors){
			Vector2 axisPos = TransformGraphToGraphArea(new Vector2(vCursorPos.x, 0));
			Vector2 cursorPos = TransformGraphToGraphArea(vCursorPos);
			DrawLine(axisPos, cursorPos, vCursorCol);
		}
	}
	
	Vector3 TransformGraphToWorld(Vector2 dataIn){
		float xAxisRange = xAxisMax - xAxisMin;
		float yAxisRange = yAxisMax - yAxisMin;
		Vector3 unitScalePos =  new Vector2((dataIn.x - xAxisMin) / xAxisRange, (dataIn.y - yAxisMin) / yAxisRange);
		
		float worldXRange = borderCorners[2].x -  borderCorners[0].x;
		float worldYRange = borderCorners[2].y -  borderCorners[0].y;
		
		return new Vector3(borderCorners[0].x + unitScalePos.x * worldXRange, borderCorners[0].y + unitScalePos.y * worldYRange, 0);
		
	}
	
	Vector2	TransformGraphToScreen(Vector2 dataIn){
		
		
		float xAxisRange = xAxisMax - xAxisMin;
		float yAxisRange = yAxisMax - yAxisMin;
		Vector3 unitScalePos =  new Vector2((dataIn.x - xAxisMin) / xAxisRange, (dataIn.y - yAxisMin) / yAxisRange);
		
		return new Vector2(screenRectMin.x + unitScalePos.x * screenRectWidth, screenRectMin.y + unitScalePos.y * screenRectHeight);
		
	}
	
	Vector2	TransformGraphToGraphArea(Vector2 dataIn){
		
		
		float xAxisRange = xAxisMax - xAxisMin;
		float yAxisRange = yAxisMax - yAxisMin;
		Vector3 unitScalePos =  new Vector2((dataIn.x - xAxisMin) / xAxisRange, (dataIn.y - yAxisMin) / yAxisRange);
		
		return new Vector2(borderRectMin.x + unitScalePos.x * borderRectWidth, borderRectMin.y + unitScalePos.y * borderRectHeight);
		
	}
	
	public void setRed(bool isRed){
		if (isRed) {
			dataCol = Color.red;
		}
	}
}
