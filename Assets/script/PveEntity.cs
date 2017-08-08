using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveEntity : MonoBehaviour {
	public string type = "PveEntity";
	public Image style;
	public Image select;
	public RawImage HP;
	public RawImage MP;
	public Text HPTxt;
	public Text MPTxt;
	public Text entityName;
	protected int currentHP;//当前血量
	protected int maxHP;//最大血量
	protected PveScene pvescene;
	protected JsonObject entityData;
	public bool isCanHit = false;//是否可以被攻击
	public int speed = 0;//出手速度
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void onHit(int _demage){
		iTween.ShakePosition (style.gameObject, new Vector3 (5.0f, 5.0f, 0.0f), 0.2f);
		currentHP = currentHP - _demage;
		currentHP = currentHP >= 0 ? currentHP : 0;
		float xscale = (float)currentHP / (float)maxHP;
		HP.transform.localScale = new Vector3 (xscale,1,1);
		HPTxt.text = ((int)(xscale * 100)).ToString () + "%";
		Loom.QueueOnMainThread (() => {
			if (currentHP == 0) {
				onDead ();
			}
		},0.5f);

	}
	public void onDead(){
		PoolManager.getInstance ().addToPool (this.type,this);
		pvescene.checkBout ();
		
	}
	public void showSelect(){
		if (!select.isActiveAndEnabled) {
			iTween.ScaleBy (select.gameObject, iTween.Hash ("y", 1.2, "x", 1.2, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1, "time", 0.5));
			select.gameObject.SetActive (true);
			isCanHit = true;
		}
	}
	public void hideSelect(){
		if (select.isActiveAndEnabled) {
			iTween.Stop (select.gameObject);
			select.transform.localScale = Vector3.one;
			select.gameObject.SetActive (false);
			isCanHit = false;
		}

	}
	public virtual void click (){

	}
	public virtual void active(){
	}
	public virtual void disActive(){

	}
}
