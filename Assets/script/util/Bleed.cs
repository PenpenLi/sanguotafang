using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Bleed : MonoBehaviour {

	public Text blood;
	public string type = "Bleed";
	public int playType = 1;//播放的类型
	public bool isPlay = false;
	private Vector3 currentScale;
	private int step = 1;
	private Action action;
	void Awake () {
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		currentScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isPlay) {
			if (playType == 1) {
				playBlood ();
			} else if (playType == 2){
				playTurn ();
			}

		}
	}
	public void playTurn(){//播放回合
		if (step == 1) {//放大阶段
			if (currentScale.x < 2.0f) {
				currentScale.x = currentScale.y = currentScale.z += 0.25f;
				transform.localScale = currentScale;
			} else {
				step = 2;
			}
		}else if(step == 2){//回到正常大小阶段
			if (currentScale.x > 1.0f) {
				currentScale.x = currentScale.y = currentScale.z -= 0.1f;
				transform.localScale = currentScale;
			} else {
				step = 3;
				Loom.QueueOnMainThread (() => {
					isPlay = false;
					step = 1;
					currentScale = Vector3.zero;
					PoolManager.getInstance ().addToPool (type,this);
					if (action != null) {
						action ();
					}
				}, 1.0f);
			}
		}
	}
	public void playBlood(){
		if (step == 1) {//放大阶段
			if (currentScale.x < 3.0f) {
				currentScale.x = currentScale.y = currentScale.z += 1.0f;
				transform.localScale = currentScale;
			} else {
				step = 2;
			}
		}else if(step == 2){//回到正常大小阶段
			if (currentScale.x > 1.0f) {
				currentScale.x = currentScale.y = currentScale.z -= 0.5f;
				transform.localScale = currentScale;
			} else {
				step = 3;
				Loom.QueueOnMainThread (() => {
					if (action != null) {
						action ();
					}
				}, 0.5f);
			}
		}else if(step == 3){//向上移动阶段
			if (transform.localPosition.y < 30) {
				transform.Translate (new Vector3 (0, 0.5f, 0));
				Color color = blood.color;
				color.a -=0.01f;
				blood.color = color;
			} else {
				isPlay = false;
				step = 1;
				currentScale = Vector3.zero;
				Color color = blood.color;
				color.a =1.0f;
				blood.color = color;
				PoolManager.getInstance ().addToPool (type,this);


			}
		}
	}
	public void show(int damage,Action _action = null){
		playType = 1;
		action = _action;
		transform.localPosition = Vector3.zero;
		transform.localScale = currentScale;
		blood.text = damage.ToString ();
		isPlay = true;
	}
	public void show(string txt,Action _action = null){
		playType = 2;
		action = _action;
		transform.localPosition = Vector3.zero;
		transform.localScale = currentScale;
		blood.text = txt;
		isPlay = true;
	}
}
