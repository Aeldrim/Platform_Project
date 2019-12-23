using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	private float [] parallaxScales; 	//Proporcion del movimiento de la camara para mover los backgrounds
	private Transform cam; 				//Referencia al transform de la camara
	private Vector3 previousCamPos;		//Almacena la posicion de la camara en el anterior frame

	[Header("Parallax Elements")]
	public Transform[] backgrounds;		//Array de backgrounds y foreground para Parallaxing
	public float smoothing = 1f; 		//Que tanto smooth tendra el parallax

	void Awake (){
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		// Asigna la escala de Parallax correspondiente
		parallaxScales = new float [backgrounds.Length];
		for (int i = 0; i <backgrounds.Length; i++){
			parallaxScales[i] = backgrounds[i].position.z *-1;
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i <backgrounds.Length; i++){
			// El parallax es opuesto al movimiento de camara
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//Settea una posicion x, que es la posicion actual del parallax
			float backgroundsTargetPosX = backgrounds[i].position.x + parallax;

			//Crea la posicion actual del background
			Vector3 backgroundsTargetPos = new Vector3 (backgroundsTargetPosX,backgrounds[i].position.y, backgrounds[i].position.z);

			//
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundsTargetPos, smoothing * Time.deltaTime);
		}

		previousCamPos = cam.position;
	}
}
