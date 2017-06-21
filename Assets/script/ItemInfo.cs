﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class ItemInfo : MonoBehaviour {
	public Text panelName;
	public Text itemName;
	public Text itemShuLiang;
	public Text itemInfo;
	public JsonObject data;
	public Image icon;
	public Image zhandouliPanel;
	public Image bg;
	public Image doPanel;
	public string type;
	public int itemType;
	public int owerId;
	public int pos;
	// Use this for initialization
	void Awake () {
		zhandouliPanel.gameObject.SetActive (false);
		PoolManager.getInstance ().initPoolByType (type,this,1);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void onChange(){
		if (itemType == 2) {
			List<JsonObject> list = BagManager.getInstance ().getItemsByType("2");
			ListPanel _listPanel= (ListPanel)PoolManager.getInstance ().getGameObject (PoolManager.LIST_PANEL);
			_listPanel.transform.SetParent (this.transform.parent.transform);
			_listPanel.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_listPanel.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			_listPanel.init (list,this,3,owerId,pos);
		}
	}
	public void init(JsonObject jo){
		data = jo;
		//if (jo.ContainsKey ("staticdata")) {
		jo = BagManager.getInstance ().getItemStaticData (jo);
		itemType = int.Parse (jo["itemType"].ToString());
		owerId = int.Parse (data["owerId"].ToString());
		pos = int.Parse (data["pos"].ToString());
		doPanel.gameObject.SetActive (false);
		if (itemType == 2) {
			if (owerId > 0) {
				doPanel.gameObject.SetActive (true);
			}
		} 
		//}
		/// <summary>
		/// 初始化武器信息
		/// </summary>
		if (jo.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + jo["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else {
			icon.sprite = (Resources.Load("icon/" + jo["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		icon.SetNativeSize();
		itemName.text = jo ["name"].ToString ();
		itemInfo.text = jo ["desc"].ToString ();

		bg.sprite = (Resources.Load("all/hero_bg_" + jo["color"].ToString(), typeof(Sprite)) as Sprite);
		itemShuLiang.text = "x" + data ["count"].ToString ();
	}
	public void onClose(){
		PoolManager.getInstance ().addToPool (type,this);
	}
}
