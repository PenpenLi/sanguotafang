using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster3D : MonoBehaviour {

	public Animator animator;
	// Use this for initialization
	void Start () {
		this.transform.localPosition = new Vector3 (300F,0F,800F);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p3 = this.transform.localPosition;
		p3.z -= 1;
		this.transform.localPosition = p3;
	}
}
