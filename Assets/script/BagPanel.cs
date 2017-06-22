﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using TFSG;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BagPanel : Observer {
    public Text info;
    public Text count;
    public Text speed;
    public Text itemname;
    public Text defence;
    public Text magic_defence;
    public Image icon;
    public Button useBtn;
	public Button hechengBtn;
	public JsonObject data;
	private IconBase ico;
	public string poolType;
	public static BagPanel _demoPanel;
	public bool isUpdate = false;
	public int openType;
	public int itemType;
	void Awake(){
		messageArr.Add (Message.BAG_UPDATE);
		PoolManager.getInstance ().initPoolByType (PoolManager.BAG_ITEM + poolType,this,3);
	}
    // Use this for initialization
    void Start () {
		UGUIEventTrigger.Get (icon.gameObject).AddEventListener (EventTriggerType.PointerClick,OnClick);
		//BagManager.getInstance ().showAll ();
	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.BAG_UPDATE:
				{
					JsonObject _data = (JsonObject)nt.data;
					int updateheroId = int.Parse (_data ["id"].ToString ());
					int curheroId = int.Parse (data ["id"].ToString ());
					int count = int.Parse (_data ["count"].ToString ());
					if (updateheroId == curheroId) {
						if (count > 0) {
							data = _data;
							init (data,0);
						} else {
							data = _data;
							PoolManager.getInstance ().addToPool(PoolManager.BAG_ITEM + this.poolType,this);
						}

					}
				}
				break;
			}
		}
	}
	public void init(JsonObject item,int _openType = 0)
    {
		
		useBtn.gameObject.SetActive (false);
		hechengBtn.gameObject.SetActive (false);
		data = item;
		//if (ico != null) {
		//	PoolManager.getInstance ().addToPool (ico.type,ico);
		//}
		JsonObject staticData;// = BagManager.getInstance().getItemStaticData(item);
		if (item.ContainsKey ("itemId")) {
			staticData = BagManager.getInstance().getItemStaticData(item);
		} else {
			staticData = item;
		}
		itemType = int.Parse(staticData["itemType"].ToString());
		//ico = (IconBase)PoolManager.getInstance ().getGameObject (staticdata["color"].ToString());
		//ico.init (staticdata);
		//ico.transform.SetParent (this.transform);
		//ico.transform.localScale = new Vector3 (0.6f,0.6f,0.6f);
		//ico.transform.localPosition = new Vector3 (100,-this.GetComponent<Image>().rectTransform.rect.height/2,0);
		if (staticData.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + staticData["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else if(data.ContainsKey ("id")){
			icon.sprite = (Resources.Load("icon/" + staticData["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		openType = _openType;
		//icon.sprite = ("icon/" + staticdata["icon"].ToString(), typeof(Sprite)) as Sprite);
		icon.SetNativeSize();
		itemname.text = staticData["name"].ToString();
		info.text = staticData ["desc"].ToString ();
		if (itemType == 5 || itemType == 2) {
			
			string shuxing = "";
			int attack = int.Parse (data.ContainsKey("attackValue") ? data ["attackValue"].ToString () :staticData ["attackValue"].ToString ());
			int hp = int.Parse (data.ContainsKey("hpValue") ? data ["hpValue"].ToString () :staticData ["hpValue"].ToString ());
			int defence = int.Parse (data.ContainsKey("defenceValue") ? data ["defenceValue"].ToString () :staticData ["defenceValue"].ToString ());
			if (attack > 0) {
				shuxing += DataManager.getInstance ().itemDicJson [0] ["attackValue"].ToString () + "+" + attack.ToString ();
			}
			if (hp > 0) {
				shuxing += "  " + DataManager.getInstance ().itemDicJson [0] ["hpValue"].ToString () + "+" + hp.ToString ();
			}
			if (defence > 0) {
				shuxing += "  " + DataManager.getInstance ().itemDicJson [0] ["defenceValue"].ToString () + "+" + defence.ToString ();
			}
			info.text = shuxing;
			//}
		}
		if (data.ContainsKey ("heroId")) {
			int heroId = int.Parse (data ["heroId"].ToString ());
			//if (heroId == 0) {//被穿戴的装备不会在背包里面显示
			if (heroId > 0) {
				JsonObject herodata = DataManager.getInstance ().heroDicJson [heroId];
				itemname.text = staticData ["name"].ToString () + "(" + herodata ["name"].ToString () + ")";
			}
		}
		if (data.ContainsKey ("owerId")) {
			JsonObject jo = BagManager.getInstance().getEquipById (int.Parse(data["owerId"].ToString()));
			if (jo != null) {
				jo = BagManager.getInstance ().getItemStaticData (jo);
				itemname.text = staticData ["name"].ToString () + "(" + jo ["name"].ToString () + ")";
			}
		}
		if (data.ContainsKey ("level") && itemType == 5) {
			itemname.text = "Lv." + data ["level"].ToString () + " " + itemname.text;

		}
		//if (item.ContainsKey("color")) {
		//name.color = DataManager.getInstance().getColor(staticdata["color"].ToString());
		//} else {
		//	name.color = DataManager.getInstance().getColor("");
		//}
		if (item.ContainsKey ("count")) {
			count.text = item["count"].ToString();
		} else {
			count.text = "1";
		}

		//info.text = staticData["desc"].ToString();
		if (staticData.ContainsKey ("itemType")) {
			


			if (staticData ["itemType"].ToString () == "8") {
				int heroid = int.Parse(staticData ["heroId"].ToString ());
				JsonObject hd = HeroManager.getInstance().getHeroById (heroid);
				if (hd == null) {//如果没有这个英雄
					int count = int.Parse (item ["count"].ToString ());
					int needcount = int.Parse (staticData ["addExp"].ToString ());
					if (count >= needcount) {
						hechengBtn.gameObject.SetActive (true);
					}
				}

			}
			if (staticData ["itemType"].ToString () == "9" || staticData ["itemType"].ToString () == "2") {
					int heroid = int.Parse(staticData ["heroId"].ToString ());
				//JsonObject hd = HeroManager.getInstance().getHeroById (heroid);
				//if (hd == null) {//如果没有这个英雄
					int count = int.Parse (item ["count"].ToString ());
					int needcount = int.Parse (staticData ["addExp"].ToString ());
				if (count >= needcount && heroid > 0) {
						hechengBtn.gameObject.SetActive (true);
					}
				//}

			}
		}


		if (openType >= 2 || itemType == 2) {//穿戴显示穿戴按钮
			useBtn.gameObject.SetActive (true);
		}

    }
	public void OnClick(BaseEventData eventData){
		JsonObject staticData = BagManager.getInstance().getItemStaticData(data);
		if (itemType != 5) {
			ItemInfo _equipInfo = (ItemInfo)PoolManager.getInstance().getGameObject(PoolManager.ITEM_INFO);
			_equipInfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			_equipInfo.init (data);
		} else {
			EquipInfo _equipInfo = (EquipInfo)PoolManager.getInstance().getGameObject(PoolManager.EQUIP_INFO);
			_equipInfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_equipInfo.init (data,0);
			_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		}


	}
    public void onUse()
    {
        //HeroManager.getInstance().heroscene.selectKind.image.sprite = icon.sprite;
		if (openType == 2) {//穿装备
			HeroManager.getInstance ().heroscene.onEquip (this);
		} else if (openType == 3) {//宝石镶嵌
			
			if (ListPanel._currentListPanel != null) {
				JsonObject userMessage = new JsonObject();
				userMessage.Add ("stoneId", data ["id"]);
				userMessage.Add ("stonePos", ListPanel._currentListPanel.stonePos);
				userMessage.Add ("equipId", ListPanel._currentListPanel.openitemId);
				ServerManager.getInstance ().request("area.equipHandler.addStone", userMessage, (databack)=>{
					Debug.Log(databack.ToString());
					AudioManager.instance.playEquip();

				});
				ListPanel._currentListPanel.onClickCloseBtn ();
			}
		}else if (openType == 4) {//宝石合成选择
			JsonObject sendMessage = new JsonObject();
			sendMessage.Add ("stone", data);
			sendMessage.Add ("pos", ListPanel._currentListPanel.stonePos);
			NotificationManager.getInstance ().PostNotification (null,Message.HECHENG_ADD_STONE,sendMessage);
			ListPanel._currentListPanel.onClickCloseBtn ();
		}else if(itemType == 2){
			StoneHeChengPanel _stoneHeChengPanel = (StoneHeChengPanel)PoolManager.getInstance().getGameObject(PoolManager.STONE_HECHENG_PANEL);
			_stoneHeChengPanel.transform.SetParent (BagManager.getInstance().getGameScene().transform);
			_stoneHeChengPanel.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_stoneHeChengPanel.init ();
			_stoneHeChengPanel.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);

			JsonObject sendMessage = new JsonObject();
			sendMessage.Add ("stone", data);
			sendMessage.Add ("pos", 0);
			NotificationManager.getInstance ().PostNotification (null,Message.HECHENG_ADD_STONE,sendMessage);
		}


        //this.transform.SetParent(HeroManager.getInstance().heroscene.selectKind.transform);
    }
	public void onHeroShardHeCheng(){
		JsonObject userMessage = new JsonObject();
		userMessage.Add ("id",data["id"]);
		//userMessage.Add ("heroId", data.heroId);
		ServerManager.getInstance ().request("area.playerHandler.useItem", userMessage, (databack)=>{
			Debug.Log(databack.ToString());


		});
	}
}
