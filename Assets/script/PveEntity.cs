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
	public Dictionary<Property,int> PropertyAddByBuffDic;//Buff增加属性的具体数字
	public Dictionary<Property,int> PropertyAddByBuffPointDic;//Buff增加属性的百分比
	public Dictionary<int,Dictionary<Property,int>> PropertyAllDic;
	public List<Buff> BuffDic;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(){
		BuffDic = new List<Buff> ();
		resetPropert ();
		HP.transform.localScale =Vector3.one;
		hideSelect ();
	}
	public void resetPropert(){
		PropertyDic = new Dictionary<Property, int> ();
		PropertyAddByBuffDic = new Dictionary<Property, int> ();
		PropertyAddByBuffPointDic = new Dictionary<Property, int> ();
		PropertyAllDic = new Dictionary<int, Dictionary<Property, int>> ();
		PropertyAllDic [0] = PropertyDic;
		PropertyAllDic [1] = PropertyAddByBuffDic;
		PropertyAllDic [2] = PropertyAddByBuffPointDic;
		for (Property i = 0; i <Property.MAXINDEX; i++) {
			PropertyDic [i] = 0;
			PropertyAddByBuffDic [i] = 0;
			PropertyAddByBuffPointDic [i] = 100;
		}
	}
	public void addBuff(Buff buff){
		BuffDic.Add (buff);
		buff.init (this);
	}
	public void removeBuff(Buff buff){
		BuffDic.Remove (buff);
	}
	public void updateBuff(){
		for (int i = 0; i <BuffDic.Count; i++) {
			BuffDic [i].updateBuff ();
		}
		for (int i = 0; i <BuffDic.Count; i++) {
			if (BuffDic [i].turn <= 0) {
				BuffDic.RemoveAt (i);
				i--;
			}
		}
	}
	public int getPropert(Property _propert){
		return (int)((PropertyDic[_propert] + PropertyAddByBuffDic[_propert]) *  PropertyAddByBuffPointDic[_propert]/100);
		
	}
	public void showPropertyChange(Property changeEntityProperty,int _changeValue,int valueType){
		Bleed bleed = (Bleed)PoolManager.getInstance ().getGameObject ("Bleed");
		if (_changeValue > 0) {
			bleed.blood.color = DataManager.getInstance ().getColor ("green");
		} else {
			bleed.blood.color = DataManager.getInstance ().getColor ("red");
		}
		bleed.transform.SetParent (this.transform);
		string str = "";
		switch (valueType) {
		case 0:
			str = DataManager.getInstance ().languageJson [10011 + (int)changeEntityProperty] ["name"].ToString () + _changeValue.ToString ();
			if (changeEntityProperty == Property.HP) {
				if (PropertyDic [Property.HP] > PropertyDic [Property.MAXHP]) {
					PropertyDic [Property.HP] = PropertyDic [Property.MAXHP];
				}
				float xscale = (float)PropertyDic [Property.HP] / (float)PropertyDic [Property.MAXHP];
				HP.transform.localScale = new Vector3 (xscale, 1, 1);
				HPTxt.text = (Math.Ceiling (xscale * 100)).ToString () + "%";
			}
			break;
		case 1:
			str = DataManager.getInstance ().languageJson [10023 + (int)changeEntityProperty]["name"].ToString () + _changeValue.ToString ();
			break;
		case 2:
			str = DataManager.getInstance ().languageJson [10035 + (int)changeEntityProperty]["name"].ToString () + _changeValue.ToString ();
			break;
		default:
			break;
		}
		bleed.show (str, () => {
			if (PropertyDic [Property.HP] == 0) {
				onDead ();
			}
			pvescene.checkBout ();
		});
	}
	public void changeMP(int _mp){
		int mp = PropertyDic [Property.MP];
		int maxmp = PropertyDic [Property.MAXMP];
		mp -= _mp;
		mp = mp >= 0 ? mp : 0;
		mp = mp >= maxmp ? maxmp : mp;
		PropertyDic [Property.MP] = mp;
		float xscale = (float)mp / (float)maxmp;
		MP.transform.localScale = new Vector3 (xscale,1,1);
		MPTxt.text = (Math.Ceiling(xscale * 100)).ToString () + "%";
	}
	public void onHit(int _demage,int num = 1){
		float _delay = 0.0f;
		for (int i = 0; i < num; i++) {
			
			Loom.QueueOnMainThread (() => {
				if (PropertyDic [Property.HP] > 0) {
					Bleed bleed = (Bleed)PoolManager.getInstance ().getGameObject ("Bleed");
					if (_demage > 0) {
						iTween.ShakePosition (style.gameObject, new Vector3 (5.0f, 5.0f, 0.0f), 0.2f);
						bleed.blood.color = DataManager.getInstance ().getColor ("red");
					} else {
						//加血
						bleed.blood.color = DataManager.getInstance ().getColor ("green");
					}
					int buffhp = PropertyAddByBuffDic [Property.HP];//护盾
					buffhp = buffhp - _demage;
					PropertyAddByBuffDic [Property.HP] = buffhp <0 ? 0 : buffhp;
					if (buffhp < 0 || _demage < 0) {
						int hp = PropertyDic [Property.HP];
						int maxhp = PropertyDic [Property.MAXHP];
						hp = hp + buffhp;
						hp = hp >= 0 ? hp : 0;
						hp = hp >= maxhp ? maxhp : hp;
						float xscale = (float)hp / (float)maxhp;
						HP.transform.localScale = new Vector3 (xscale, 1, 1);
						HPTxt.text = (Math.Ceiling (xscale * 100)).ToString () + "%";
						PropertyDic [Property.HP] = hp;

					} else {
						buffhp = 0;
					}
					bleed.transform.SetParent (this.transform);
						bleed.show (buffhp, () => {
							if (PropertyDic [Property.HP] == 0) {
								onDead ();
							}
							pvescene.checkBout ();
						});
				}

			},_delay);
			_delay += 0.3f;


		}

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
