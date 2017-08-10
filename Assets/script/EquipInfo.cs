using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using System.Text.RegularExpressions;
public class EquipInfo :Observer {
	public Text panelName;
	public Text itemName;
	public Text itemShuXing;
	public Text equipFightPointer;
	public Text itemInfo;
	public JsonObject data;
	public Image icon;
	public Image bg;
	public Image doPanel;//操作界面
	public Image FightPointerPanel;
	public string type;
	public string kind;
	public int equipId;
	public int Id;
	public Button changeBtn;
	public Button fumoBtn;
	public Button levelupBtn;
	public Text levelUpNeedInfo;
	public Text fumoNeedInfo;

	public Text weapon;
	public Text armor;
	public Text shoes;
	public Text amulet;
	public Text suitName;
	public Text suit2;
	public Text suit3;
	public Text suit4;
	public Dictionary<string,Text> suitArr;
	public Image suitPanel;
	public List<Button> stoneArr;
	public Dictionary<string,Button> equipArr;
	public List<IconBase> IconBaseArr;
	public Button stone0;
	public Button stone1;
	public Button stone2;
	public Button stone3;
	public Button stone4;

	public Button equip0;
	public Button equip1;
	public Button equip2;
	public Button equip3;
	public Image stonePanel;
	public int pinzhi = 1;
	public bool isFlesh = false;
	public int openType;
	// Use this for initialization
	void Awake () {
		suitArr = new Dictionary<string, Text> ();
		stoneArr = new List<Button> ();
		equipArr = new Dictionary<string, Button> ();
		IconBaseArr = new List<IconBase> ();
		suitArr ["weapon"] = weapon;
		suitArr ["armor"] = armor;
		suitArr ["shoes"] = shoes;
		suitArr ["amulet"] = amulet;
		suitArr ["name"] = suitName;
		suitArr ["suit2"] = suit2;
		suitArr ["suit3"] = suit3;
		suitArr ["suit4"] = suit4;
		stoneArr.Add (stone0);
		stoneArr.Add (stone1);
		stoneArr.Add (stone2);
		stoneArr.Add (stone3);
		stoneArr.Add (stone4);

		equipArr ["weapon"] = equip0;
		equipArr ["armor"] = equip1;
		equipArr ["shoes"] = equip2;
		equipArr ["amulet"] = equip3;
		messageArr.Add (Message.EQUIP_LEVELUP);
		messageArr.Add (Message.EQUIP_ADD_STONE);
		PoolManager.getInstance ().initPoolByType (type,this,1);
	}
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.EQUIP_LEVELUP:
				{
					//if (data != null) {
					//	int id = int.Parse (data ["id"].ToString ());
						data = nt.data as JsonObject;
					//	data = BagManager.getInstance ().getEquipById (id);
						init(data,openType);
					//}
				}
				break;
			case Message.EQUIP_ADD_STONE:
				{
					//data = nt.data as JsonObject;
					//updateShuXing ();
				}
				break;
			}
		}
	}
	public void initBase(JsonObject jo){
		/// <summary>
		/// 初始化武器信息
		/// </summary>
		kind = jo["kind"].ToString();
		equipId = int.Parse(jo["id"].ToString());
		//if (jo.ContainsKey ("icon")) {
		icon.sprite = (Resources.Load(jo["icon"].ToString(), typeof(Sprite)) as Sprite);
		//} else {
		//	icon.sprite = (Resources.Load("icon/" + jo["id"].ToString(), typeof(Sprite)) as Sprite);
		//}
		icon.SetNativeSize();
		pinzhi = DataManager.getInstance ().getPinZhi(jo ["color"].ToString ());
		if (data.ContainsKey ("level")) {
			itemName.text = "Lv." + data ["level"].ToString () + " " + jo ["name"].ToString ();
		} else {
			itemName.text = jo ["name"].ToString ();
		}
		itemName.color = DataManager.getInstance ().getColor (jo ["color"].ToString ());
		itemInfo.text = jo ["desc"].ToString ();

		bg.sprite = (Resources.Load("all/hero_bg_" + jo["color"].ToString(), typeof(Sprite)) as Sprite);
	

		itemShuXing.text = DataManager.getInstance().updateShuXing (data);
		equipFightPointer.text = DataManager.getInstance().updateShuXing (data,1);
		fumoBtn.gameObject.SetActive (true);
		levelupBtn.gameObject.SetActive (true);

			
	}
	public void init(JsonObject jo,int _openType){
		openType = _openType;
		//NotificationManager.getInstance ().AddObserver (this,"equip_levelup");
		data = jo;
		Id = int.Parse (data ["id"].ToString ());
		//if (jo.ContainsKey ("staticdata")) {
		jo = BagManager.getInstance ().getItemStaticData (jo);

		//}
		initBase(jo);
		for(int i=0;i < IconBaseArr.Count;i++){
			//Button btn = equips [kvp.Key];
			IconBase icon = IconBaseArr[i];
			if (icon != null) {

				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		IconBaseArr.Clear ();
		initSuit (jo);
		initStone ();
		if(_openType > 0){
			doPanel.gameObject.SetActive(true);
		}else{
			doPanel.gameObject.SetActive(false);
		}
		if(doPanel.isActiveAndEnabled){
			changeBtn.gameObject.SetActive (true);
			int heroId = int.Parse(data["owerId"].ToString());
			if (heroId == 0 || openType == 0) {
				changeBtn.gameObject.SetActive (false);
			}

			//升星更新
			updateBtn(levelupBtn,levelUpNeedInfo,101,"level","equipLevelUpNeed","levelUp");	

			//附魔更新
			updateBtn(fumoBtn,fumoNeedInfo,100,"level","equipFuMoNeed","levelUp");
		}
	}
	public void initSuit(JsonObject jo){//套装系统
		JsonObject suit = DataManager.getInstance ().getSuitByEquip (jo);

		int suitNum = 0;
		if (suit != null) {
			suitNum += 1;
			suitPanel.gameObject.SetActive (true);
			foreach (KeyValuePair<string,object> kvp in suit) {
				if (suitArr.ContainsKey(kvp.Key)) {
					string value = kvp.Value.ToString ();
					bool isInt = Regex.IsMatch (value, @"^[+-]?\d*$");

					if (isInt) {
						
						JsonObject equip = DataManager.getInstance ().itemDicJson [int.Parse(value)];
						suitArr [kvp.Key].text = equip ["name"].ToString ();
						///////////////////////////////////////////////////
						Button stoneKuang = equipArr [kvp.Key];
						//equip.sprite = 
						IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo ["color"].ToString ());
						if (openType > 0) {
							icon.init (equip).Func = new callBackFunc<JsonObject> (onClickStone);
						} else {
							icon.init (equip);
						}
						//icon.Func = new callBackFunc<JsonObject> (onClickStone);
						icon.transform.SetParent (stoneKuang.transform);
						icon.transform.localPosition = Vector3.zero;
						icon.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
						IconBaseArr.Add (icon);
						/////////////////////////////////////////
						int heroId = 0;
						if (data.ContainsKey ("owerId")) {
							heroId = int.Parse (data ["owerId"].ToString ());
						}
						if (heroId > 0) {
							if (kvp.Key == kind) {
								suitArr [kvp.Key].color = DataManager.getInstance ().getColor (equip ["color"].ToString ());
							} else {
								JsonObject otherEquip = BagManager.getInstance ().getEquipByHeroIdAndItemId (heroId, int.Parse (value));

								if (otherEquip != null) {
									suitNum += 1;
									JsonObject otherEquipStaticData = BagManager.getInstance ().getItemStaticData (otherEquip);
									suitArr [kvp.Key].color = DataManager.getInstance ().getColor (otherEquipStaticData ["color"].ToString ());
								} else {
									suitArr [kvp.Key].color = Color.gray;
								}

							}
						}
					} else {
						if (kvp.Key.IndexOf ("suit") >= 0) {
							string format = DataManager.getInstance ().suitJson [0] [kvp.Key].ToString ();
							string[] shuxingArr = value.Split('_');
							if (shuxingArr.Length == 2) {
								suitArr [kvp.Key].text = string.Format (format, shuxingArr [0], shuxingArr [1]);
							} else if (shuxingArr.Length == 3) {
								suitArr [kvp.Key].text = string.Format (format, shuxingArr [0], shuxingArr [1],shuxingArr [2]);
							} else {
								suitArr [kvp.Key].text = value;
							}

		
						} else {
							suitArr [kvp.Key].text = value;
						}

						suitArr [kvp.Key].color = Color.gray;
					}
				}
			}
			suitArr ["name"].text += "[" + suitNum.ToString () + "/4]";
			suitArr["name"].color =suitArr [kind].color;
			if (suitNum == 2) {
				suitArr ["suit2"].color = Color.green;
			} else if (suitNum == 3) {
				suitArr ["suit2"].color = Color.green;
				suitArr ["suit3"].color = Color.green;
			} else if (suitNum == 4) {
				suitArr ["suit2"].color = Color.green;
				suitArr ["suit3"].color = Color.green;
				suitArr ["suit4"].color = Color.green;
			}
		} else {
			suitPanel.gameObject.SetActive (false);
		}
	}
	public void onClickStoneBtn(int pos){
		//stoneArr [pos];
		if (openType > 0) {
			List<JsonObject> list = BagManager.getInstance ().getItemsByType ("2");
			ListPanel _listPanel = (ListPanel)PoolManager.getInstance ().getGameObject (PoolManager.LIST_PANEL);
			_listPanel.transform.SetParent (this.transform.parent.transform);
			_listPanel.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			_listPanel.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_listPanel.init (list, this, 3, Id, pos + 1);
		}

	}
	public void onClickStone(JsonObject _data){
		//stoneArr [pos];
		ItemInfo _equipInfo = (ItemInfo)PoolManager.getInstance().getGameObject(PoolManager.ITEM_INFO);
		_equipInfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
		_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		_equipInfo.init (_data);

	}
	public void initStone(){//宝石系统
		//List<object> stones = jo["stones"] as List<object>;


		for (int i = 0; i < stoneArr.Count; i++) {
			stoneArr [i].gameObject.SetActive (i < pinzhi?true:false);
		}
		//if (openType > 0) {
			List<JsonObject> theEquipHaveStones = BagManager.getInstance ().getStoneByEquipId (Id);
			for (int k = 0; k < theEquipHaveStones.Count; k++) {
				JsonObject stoneData = theEquipHaveStones [k];
				JsonObject jo = BagManager.getInstance ().getItemStaticData (stoneData);
				int pos = int.Parse (stoneData ["pos"].ToString ());
				if (pos > 0) {
				
					Button stoneKuang = stoneArr [pos - 1];
					//equip.sprite = 
					IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo ["color"].ToString ());
					if (openType > 0) {
						icon.init (stoneData).Func = new callBackFunc<JsonObject> (onClickStone);
					} else {
						icon.init (stoneData);
					}
					//icon.Func = new callBackFunc<JsonObject> (onClickStone);
					icon.transform.SetParent (stoneKuang.transform);
					icon.transform.localPosition = Vector3.zero;
					icon.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
					IconBaseArr.Add (icon);
				}
			}
		//}
	}
	public void onClose(){
		AudioManager.instance.playCloseClick ();
		for(int i=0;i < IconBaseArr.Count;i++){
			//Button btn = equips [kvp.Key];
			IconBase icon = IconBaseArr[i];
			if (icon != null) {

				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		IconBaseArr.Clear ();
		//NotificationManager.getInstance ().RemoveObserver (this,"equip_levelup");
		PoolManager.getInstance ().addToPool (type,this);
	}
	public void OnChange(){//更换武器
		List<JsonObject> list = BagManager.getInstance ().getEquipByType(kind);
		ListPanel _listPanel= (ListPanel)PoolManager.getInstance ().getGameObject (PoolManager.LIST_PANEL);
		_listPanel.transform.SetParent (BagManager.getInstance().getGameScene().transform);
		_listPanel.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		_listPanel.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		_listPanel.init (list,this,2);
		PoolManager.getInstance ().addToPool (type,this);
		//BagManager.getInstance ().showEquipByType (kind);
	}
	public void OnFuMo(){//武器附魔
	}
	public void OnLevelUp(){//武器升级
		JsonObject userMessage = new JsonObject();
		userMessage.Add ("equipId", data["id"]);
		//userMessage.Add ("heroId", heroId);

		ServerManager.getInstance ().request("area.equipHandler.equipLevelUp", userMessage, (databack)=>{
			Debug.Log(databack.ToString());
			//isFlesh = true;

		});
	}

	public void updateBtn(Button btn,Text txt,int itemid,string level,string need,string dataDicName){
		JsonObject item = BagManager.getInstance ().getItemByItemId (itemid);
		int nextLevel = int.Parse(data[level].ToString()) + 1;
		if (DataManager.getInstance ().dataDic[dataDicName].ContainsKey (nextLevel)) {
			btn.gameObject.SetActive (true);
			JsonObject jo8 = DataManager.getInstance ().dataDic[dataDicName][nextLevel];
			int haveNum = 0;
			int needNum = int.Parse (jo8 [need].ToString ());
			if (item != null) {
				haveNum = int.Parse (item ["count"].ToString ());
				txt.text = item["count"].ToString() + "/" + jo8[need].ToString();
			} else {
				haveNum = 0;
				txt.text =  "0/" + jo8[need].ToString();
			}
			if (haveNum < needNum) {
				//btn.interactable = false;
				txt.color = Color.red;
			} else {
				//btn.interactable = true;
				txt.color = Color.white;
			}

		} else {
			btn.gameObject.SetActive (false);
		}
	}
}
