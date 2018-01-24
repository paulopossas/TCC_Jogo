using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

	public GameObject up_grey;
	public GameObject up_red;
	public GameObject down_grey;
	public GameObject down_red;
	public GameObject left_grey;
	public GameObject left_red;
	public GameObject right_grey;
	public GameObject right_red;
	public GameObject selecionar_grey;
	public GameObject selecionar_red;
	public GameObject pular_grey;
	public GameObject pular_red;
	public GameObject pegar_grey;
	public GameObject pegar_red;

	// Use this for initialization
	void Start () {

		clearImages ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clearImages(){
		up_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		down_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		left_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		right_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		selecionar_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		pegar_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		pular_red.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
	}
}
