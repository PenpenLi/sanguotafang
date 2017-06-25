using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class BagManager{

	private static BagManager _bagManager;
	//private DictionaryEntry<int,Hero> _heroArr;
	//private Dictionary<int,JsonObject> itemArr;
	private Dictionary<int,JsonObject> equipArr;
	private Dictionary<int,JsonObject> stoneArr;
	private Dictionary<int,Dictionary<int,JsonObject>> allItemDic;
	private ArrayList bagItemArr;
	public BagScene _bagScene;
    private GameScene gamescene;
	public static BagManager getInstance(){//获取单例
		if(_bagManager == null){
			_bagManager = new BagManager();
		}
		return _bagManager;
	}
	public BagManager(){
		bagItemArr = new ArrayList ();
		allItemDic = new Dictionary<int, Dictionary<int, JsonObject>> ();
		//itemArr = new Dictionary<int, JsonObject> ();
		equipArr = new Dictionary<int, JsonObject> ();
		stoneArr = new Dictionary<int, JsonObject> ();
		if(DataManager.playerData.ContainsKey("bag")){
			JsonObject heroArr = (DataManager.playerData ["bag"]) as JsonObject;
			if (heroArr != null) {
				initData (heroArr);
			}


		}
	}
	public void initData(JsonObject heroArr){
		//itemArr.Clear ();
		equipArr.Clear ();
		List<object> objs;
		//JsonObject objss = (heroArr ["items"]) as JsonObject;
		if(heroArr.ContainsKey ("items")){
			objs = (heroArr ["items"]) as List<object>;
			for(int i = 0; i < objs.Count; i++) {
				JsonObject jo = objs [i] as JsonObject;
				//int itemid = int.Parse(jo["itemId"].ToString());
				int id = int.Parse (jo ["id"].ToString ());
				int count = int.Parse (jo ["count"].ToString ());
				int itemType = int.Parse (jo ["itemType"].ToString ());
				//JsonObject item = DataManager.getInstance ().itemDicJson [itemid] as JsonObject;
				//jo ["staticdata"] = item;
				if (count > 0) {
					if (!allItemDic.ContainsKey (itemType)) {
						allItemDic [itemType] = new Dictionary<int, JsonObject> ();
					}
					allItemDic [itemType] [id] = jo;
				}

				//item ["count"] = jo ["count"];
				//item ["type"] = jo ["type"];
				//Debug.Log (item.ToString());

			}
		}
		//equipArr = itemArr;
		//if (heroArr.ContainsKey ("equips")) {
			//objs = (heroArr ["equips"]) as List<object>;
		/**foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
			
			int count = int.Parse (kvp.Value ["count"].ToString ());
			if (count > 0) {
				int itemtype = int.Parse(kvp.Value ["itemType"].ToString ());
				int id = int.Parse (kvp.Value ["id"].ToString ());
				if( itemtype == 5){
					equipArr [id] = kvp.Value;
				}else if(itemtype == 2){
					stoneArr [id] = kvp.Value;
				}
			}

		}**/

	}
    public void setGameScene(GameScene _gamescene)
    {
        gamescene = _gamescene;
    }
	public GameScene getGameScene()
	{
		return gamescene;
	}
    public void Clear()
    {
        for (int i = 0; i < bagItemArr.Count; i++)
        {
			BagPanel go = (BagPanel)bagItemArr[i];
			if (go != null) {
				//go.transform.SetParent(null);
				PoolManager.getInstance ().addToPool (PoolManager.BAG_ITEM + go.poolType,go);
			}
            
        }
		bagItemArr.Clear ();
    }
	public List<JsonObject> getEquipByType(string type)
    {
        //gamescene.onclickBtn(5);
        //_bagScene.onclickBtn(2);
        //Clear();
		List<JsonObject> list = new List<JsonObject> ();
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_EQUIP)) {
			List<JsonObject> listNotEquip = new List<JsonObject> ();
			foreach (KeyValuePair<int,JsonObject> kvp in allItemDic[IconBase.ITEM_TYPE_EQUIP]) {
				JsonObject jo = kvp.Value;
				JsonObject staticData = getItemStaticData (jo);
				if (staticData ["kind"].ToString () == type) {
					int heroId = int.Parse (jo ["owerId"].ToString ());
					if (heroId == 0) {//被穿戴的装备不会在背包里面显示
						list.Add (jo);
					} else {
						listNotEquip.Add (jo);
					}
				}
			}
			/**foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			JsonObject jo = kvp.Value;

			if(jo["kind"].ToString() == type)
			{
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId == 0) {//被穿戴的装备不会在背包里面显示
					list.Add (jo);
				}
			}
		}**/
			list.AddRange (listNotEquip);
		}
		return list;

    }
	public List<JsonObject> getItemsByType(string type,int _itemId = 0,List<int> inUseList = null){
		List<JsonObject> list = new List<JsonObject> ();

		/**if(type == "equip")
		{
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId > 0) {//被穿戴的装备不会在背包里面显示
					list.Add (jo);
				}
			}
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId == 0) {//被穿戴的装备不会在背包里面显示
					list.Add (jo);
				}
			}
		}
		else
		{**/
		int key = int.Parse (type);
		if (allItemDic.ContainsKey (key)) {
			List<JsonObject> listNotEquip = new List<JsonObject> ();
			Dictionary<int,JsonObject> items = allItemDic [int.Parse (type)];
			foreach (KeyValuePair<int,JsonObject> kvp in items) {
				//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
				//if (type == kvp.Value["itemType"].ToString()) {
				int owerId = int.Parse (kvp.Value ["owerId"].ToString ());
				int itemId = int.Parse (kvp.Value ["itemId"].ToString ());
				int id = int.Parse (kvp.Value ["id"].ToString ());
				if (_itemId > 0) {
					if (_itemId != itemId) {
						continue;
					}
				}
				if (inUseList != null) {
					bool isInUse = false;
					for (int i = 0; i < inUseList.Count; i++) {
						if (inUseList [i] == id) {
							isInUse = true;
							break;
						}
					}
					if (isInUse)
						continue;
				}
				if (owerId == 0) {
					list.Add (kvp.Value);
				} else {
					listNotEquip.Add (kvp.Value);
				}

				//}
			}
			/**}**/
			list.AddRange (listNotEquip);
		}
		return list;
	}
	public List<JsonObject> getStoneByEquipId(int equipId){
		List<JsonObject> list = new List<JsonObject> ();
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_STONE)) {
			Dictionary<int,JsonObject> items = allItemDic [IconBase.ITEM_TYPE_STONE];
			foreach (KeyValuePair<int,JsonObject> kvp in items) {
				//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
				if (equipId == int.Parse (kvp.Value ["owerId"].ToString ())) {
					list.Add (kvp.Value);
				}
			}
		}
		return list;
	}
    public void showItemByType(string type,int openType = 0){
        Clear();
		List<JsonObject> list = getItemsByType(type);
		for (int i = 0; i < list.Count; i++) {
			_bagScene.add (list[i],openType);
		}
		/**if(type == "equip")
        {
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId > 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo);
				}
			}
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId == 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo);
				}
			}
        }
        else
        {
			foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
				//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
				if (type == kvp.Value["itemType"].ToString()) {
					_bagScene.add (kvp.Value);
				}
			}
        }**/

    }
	public void showAll(){
       // Clear();
        //if (bagItemArr.Count == 0){//
		//foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
		//	_bagScene.add(kvp.Value);
		//}
		//foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
		//	_bagScene.add(kvp.Value);
		//}
		//}
	}
	public JsonObject getEquipById(int id){
		//heroData hd2;
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_EQUIP)) {
			if (allItemDic [IconBase.ITEM_TYPE_EQUIP].ContainsKey (id)) {
				return allItemDic [IconBase.ITEM_TYPE_EQUIP] [id];
			}
		}
		return null;
	}
	public ArrayList getEquipByHeroId(int heroid){
		ArrayList arr = new ArrayList ();
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_EQUIP)) {
			foreach (KeyValuePair<int,JsonObject> kvp in allItemDic[IconBase.ITEM_TYPE_EQUIP]) {
				if (int.Parse (kvp.Value ["owerId"].ToString ()) == heroid) {
					arr.Add (kvp.Value);
				}
			}
		}
		return arr;
	}
	public JsonObject getItemByItemId(int itemid){
		ArrayList arr = new ArrayList ();
		foreach(KeyValuePair<int,Dictionary<int,JsonObject>> kv in allItemDic){
			foreach(KeyValuePair<int,JsonObject> kvp in kv.Value){
				if (int.Parse (kvp.Value ["itemId"].ToString ()) == itemid) {
					return kvp.Value;
				}
			}
		}
		return null;
	}
	public JsonObject getEquipByHeroIdAndKind(int heroid,string kind){
		JsonObject data = null;
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_EQUIP)) {
			foreach (KeyValuePair<int,JsonObject> kvp in allItemDic[IconBase.ITEM_TYPE_EQUIP]) {
				JsonObject staticdata = getItemStaticData (kvp.Value);
				if (int.Parse (kvp.Value ["owerId"].ToString ()) == heroid && staticdata ["kind"].ToString () == kind) {
					data = kvp.Value;
					break;
				}
			}
		}
		return data;
	}
	public JsonObject getEquipByHeroIdAndItemId(int heroid,int itemid){
		JsonObject data = null;
		if (allItemDic.ContainsKey (IconBase.ITEM_TYPE_EQUIP)) {
			foreach (KeyValuePair<int,JsonObject> kvp in allItemDic[IconBase.ITEM_TYPE_EQUIP]) {
				if (int.Parse (kvp.Value ["owerId"].ToString ()) == heroid && int.Parse (kvp.Value ["itemId"].ToString ()) == itemid) {
					data = kvp.Value;
					break;
				}
			}
		}
		return data;
	}
	public void addItem(BagPanel item)
	{
		bagItemArr.Add(item);
	}
	public void showItemsByItemType(int type)
	{
		Clear();
		if (allItemDic.ContainsKey (type)) {
			Dictionary<int,JsonObject> itemArr = allItemDic [type];
			foreach (KeyValuePair<int,JsonObject> kvp in itemArr) {
				//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
				if (type == int.Parse (kvp.Value ["itemType"].ToString ())) {
					_bagScene.add (kvp.Value);
				}
			}
		}
	}
	public void updateItemByServer(JsonObject data){
		//DataManager.playerData ["bag"] = data;
		int itemId = int.Parse(data["itemId"].ToString());
		int id = int.Parse(data["id"].ToString());
		int count = int.Parse(data["count"].ToString());
		int itemType = int.Parse(data["itemType"].ToString());
		if (count <= 0) {
			allItemDic[itemType].Remove(id);

			NotificationManager.getInstance ().PostNotification (null,Message.BAG_UPDATE,data);
		} else {
			if(!allItemDic.ContainsKey(itemType))
				allItemDic [itemType] = new Dictionary<int, JsonObject> ();
			if (!allItemDic[itemType].ContainsKey (id)) {
				
				NotificationManager.getInstance ().PostNotification (null, Message.BAG_ADD, data);
			} else {
				NotificationManager.getInstance ().PostNotification (null,Message.BAG_UPDATE,data);
			}
			allItemDic[itemType] [id] = data;
			if (itemType == IconBase.ITEM_TYPE_STONE) {
				//NotificationManager.getInstance ().PostNotification (null, Message.EQUIP_ADD_STONE, data);
			}else if(itemType == IconBase.ITEM_TYPE_EQUIP){
				NotificationManager.getInstance ().PostNotification (null,Message.EQUIP_LEVELUP,data);
			}

		}

		if (itemType == IconBase.ITEM_TYPE_YUANBAO || itemType == IconBase.ITEM_TYPE_YINBI) {
			NotificationManager.getInstance ().PostNotification (null,Message.MONEY_GOLD_UPDATE,null);
		}

		//NotificationCenter.DefaultCenter ().PostNotification (null,"initBase",data);
		//initData (data);
	}
	public JsonObject getItemStaticData(JsonObject item){
		if (item.ContainsKey ("itemId")) {
			int itemid = int.Parse (item ["itemId"].ToString ());
			//if (itemid > 8000) {//装备
			//	if (DataManager.getInstance ().equipDicJson.ContainsKey (itemid))
			//		return DataManager.getInstance ().equipDicJson [itemid];
			//} else {
				if (DataManager.getInstance ().itemDicJson.ContainsKey (itemid))
					return DataManager.getInstance ().itemDicJson [itemid];
			//}
			//return item;
		}
		return item;
	}
}
