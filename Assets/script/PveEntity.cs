using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public enum Property
{ 
	AP = 0,
	SP,
	HP,
	MP,
	MAXHP,
	MAXMP,
	AD,
	SD,
	CRIT,
	CRITPOINT,
	LUCK,
	SPEED,
	MAXINDEX
}
public class PveEntity : MonoBehaviour {
	public string type = "PveEntity";
	public Image style;
	public Image select;
	public RawImage HP;
	public RawImage MP;
	public Text HPTxt;
	public Text MPTxt;
	public Text entityName;
	protected int maxHP;//最大血量
	protected int maxMP;//最大血量
	protected PveScene pvescene;
	public JsonObject entityData;
	public bool isCanHit = false;//是否可以被攻击
	public int speed = 0;//出手速度
	public IconBase iconBase;
	protected int currentHP;//当前血量
	protected int currentMP;//当前血量
	protected int AP =0;//物理伤害
	protected int SP =0;//法术伤害
	protected int AD = 0;//物理防御
	protected int SD = 0;//法术防御
	protected int crit = 0;//暴击值
	protected int critPoint = 0;//暴击伤害百分比
	protected int luck = 0;//幸运值
	public Dictionary<Property,int> PropertyDic;
	public Dictionary<Property,int> PropertyAddByBuffDic;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void resetPropert(){
		PropertyDic = new Dictionary<Property, int> ();
		PropertyAddByBuffDic = new Dictionary<Property, int> ();
		for (Property i = 0; i <Property.MAXINDEX; i++) {
			PropertyDic [i] = 0;
			PropertyAddByBuffDic [i] = 0;
		}
	}
	public void changeMP(int _mp){
		int mp = PropertyDic [Property.MP];
		int maxmp = PropertyDic [Property.MAXMP];
		mp -= _mp;
		mp = mp >= 0 ? mp : 0;
		mp = mp >= maxmp ? maxmp : mp;
		PropertyDic [Property.MP] = mp;
		PropertyDic [Property.MAXMP] = maxmp;
		float xscale = (float)mp / (float)maxmp;
		MP.transform.localScale = new Vector3 (xscale,1,1);
		MPTxt.text = (Math.Ceiling(xscale * 100)).ToString () + "%";
	}
	public void onHit(int _demage){
		Bleed bleed = (Bleed)PoolManager.getInstance ().getGameObject ("Bleed");
		if (_demage > 0) {
			iTween.ShakePosition (style.gameObject, new Vector3 (5.0f, 5.0f, 0.0f), 0.2f);
			bleed.blood.color = DataManager.getInstance ().getColor ("red");
		} else {
			//加血
			bleed.blood.color = DataManager.getInstance ().getColor ("green");
		}
		int hp = PropertyDic [Property.HP];
		int maxhp = PropertyDic [Property.MAXHP];
		hp = hp - _demage;
		hp = hp >= 0 ? hp : 0;
		hp = hp >= maxhp ? maxhp : hp;
		float xscale = (float)hp / (float)maxhp;
		HP.transform.localScale = new Vector3 (xscale,1,1);
		HPTxt.text = (Math.Ceiling(xscale * 100)).ToString () + "%";
		PropertyDic [Property.HP] = hp;
		PropertyDic [Property.MAXHP] = maxhp;
		bleed.transform.SetParent (this.transform);
		bleed.show (-_demage,() => {
			if (PropertyDic [Property.HP] == 0) {
				onDead ();
			}
			pvescene.checkBout ();
		});

	}
	public void onClick(){
		if (isCanHit) {
			isCanHit = false;
			pvescene.attackEntity (this);
		}

	}
	public void onDead(){
		if (iconBase != null) {
			PoolManager.getInstance ().addToPool (iconBase.type,iconBase);
			iconBase = null;
		}
		PoolManager.getInstance ().addToPool (this.type,this);
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
