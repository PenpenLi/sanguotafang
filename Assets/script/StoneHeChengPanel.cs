using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class StoneHeChengPanel : Observer {

	public string type;
	public List<Button> stones;
	public int stoneId = 0;
	public int stoneItemId = 0;
	public Button stoneNew;
	public Button stone0;
	public Button stone1;
	public Button stone2;
	public Button stone3;
	public Button stone4;
	public List<int> stoneInUseId;
	public List<IconBase> IconBaseArr;
	void Awake () {
		type = PoolManager.STONE_HECHENG_PANEL;
		PoolManager.getInstance ().initPoolByType (type,this,1);
		messageArr.Add (Message.HECHENG_ADD_STONE);
		stones = new List<Button> ();
		stones.Add (stone0);
		stones.Add (stone1);
		stones.Add (stone2);
		stones.Add (stone3);
		stones.Add (stone4);
		IconBaseArr = new List<IconBase> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.HECHENG_ADD_STONE:
				{
					//if (data != null) {
					//	int id = int.Parse (data ["id"].ToString ());
					JsonObject data = nt.data as JsonObject;
					int pos = int.Parse(data ["pos"].ToString ());
					JsonObject stoneData = data ["stone"] as JsonObject;
					addStone (stoneData,pos);
					JsonObject jo = BagManager.getInstance ().getItemStaticData (stoneData);
					setHeChengStone (int.Parse(jo ["heroId"].ToString ()));
					int sameid = int.Parse(stoneData ["itemId"].ToString ());
					if (stoneItemId == 0) {
						List<JsonObject> list = BagManager.getInstance ().getItemsByType("2",sameid,stoneInUseId);
						for (int i = 0; i < list.Count; i++)
						{
							if (i > 3) {
								break;
							}
							addStone (list[i],i + 1);
						}
					}
					stoneItemId = int.Parse(stoneData ["itemId"].ToString ());
					//IconBaseArr.Add (icon);
				}
				break;
			}
		}
	}
	public void init(){
		
	}
	public void setHeChengStone(int stoneId){
		if (stoneId > 0) {
			if (stoneItemId == 0) {
				JsonObject stoneData = DataManager.getInstance ().itemDicJson [stoneId];
				//JsonObject jo = BagManager.getInstance ().getItemStaticData (stoneData);
				//	data = BagManager.getInstance ().getEquipById (id);
				//}

				//Button stoneKuang = stones[pos];
				//equip.sprite = 
				IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (stoneData ["color"].ToString ());
				icon.init (stoneData).Func = new callBackFunc<JsonObject> (onClickStone);
				//icon.Func = new callBackFunc<JsonObject> (onClickStone);
				icon.transform.SetParent (stoneNew.transform);
				icon.transform.localPosition = Vector3.zero;
				icon.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				IconBaseArr.Add (icon);
			}
		} else {
			TipManager.instance.showTip (1003);
			//onClose();
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
	public void onClose(){
		stoneItemId = 0;
		stoneInUseId.Clear ();
		for(int i=0;i < IconBaseArr.Count;i++){
			IconBase icon = IconBaseArr[i];
			if (icon != null) {
				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}


		IconBaseArr.Clear ();
		PoolManager.getInstance ().addToPool (type,this);
	}
	public void onHeCheng(){
		if (stoneInUseId.Count < 5) {
			TipManager.instance.showTip (1002);
		} else {
			JsonObject userMessage = new JsonObject();
			userMessage.Add ("stones", stoneInUseId);
			ServerManager.getInstance ().request("area.equipHandler.makeStone", userMessage, (data)=>{
				Debug.Log(data.ToString());
				AudioManager.instance.playEquip();
				onClose();

			});
		}
	}
	public void addStone(JsonObject stoneData,int pos){
		JsonObject jo = BagManager.getInstance ().getItemStaticData (stoneData);
		//	data = BagManager.getInstance ().getEquipById (id);
		//}

		stoneInUseId.Add(int.Parse(stoneData ["id"].ToString ()));
		Button stoneKuang = stones[pos];
		//equip.sprite = 
		IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo ["color"].ToString ());
		icon.init (stoneData).Func = new callBackFunc<JsonObject> (onClickStone);
		//icon.Func = new callBackFunc<JsonObject> (onClickStone);
		icon.transform.SetParent (stoneKuang.transform);
		icon.transform.localPosition = Vector3.zero;
		icon.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		IconBaseArr.Add (icon);
	}
	public void selectStone(int pos){
		List<JsonObject> list = BagManager.getInstance ().getItemsByType("2",stoneItemId,stoneInUseId);
		ListPanel _listPanel= (ListPanel)PoolManager.getInstance ().getGameObject (PoolManager.LIST_PANEL);
		_listPanel.transform.SetParent (this.transform.parent.transform);
		_listPanel.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		_listPanel.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		_listPanel.init (list,null,4,0,pos);
	}

}
