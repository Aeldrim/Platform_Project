using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	private Vector3 posA;
	private Vector3 posB;
	private Vector3 nextPos;

	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform transformA;
	[SerializeField]
	private Transform transformB;

	// Use this for initialization
	void Start () {
		posB = transformB.localPosition;
		posA = transformA.localPosition;
		nextPos = posB;
	}

	// Update is called once per frame
	void Update () {
			Move();
	}

	private void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			col.collider.transform.SetParent(transform);
		}
	}

	private void OnCollisionExit2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			col.collider.transform.SetParent(null);
		}
	}

	public void Move(){
		this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, nextPos,speed * Time.deltaTime);
		if (Vector3.Distance(this.transform.localPosition,nextPos) <= 0.1){
			ChangeDestination();
		}
	}

	private void ChangeDestination (){
		nextPos = nextPos != posA ? posA:posB;
	}
}
