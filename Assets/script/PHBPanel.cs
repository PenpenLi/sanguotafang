using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PHBPanel : Observer {

	// Use this for initialization
	public Button fightBtn;
	public Button heroBtn;
	public Button equipBtn;
	public int curType;
	public string type;
	public PHBItem phb_1;
	public PHBItem phb_2;
	public PHBItem phb_3;
	public List<PHBItem> PHBItemList;
	public Transform content;
	void Awake () {
		messageArr.Add (Message.PAIHANGBANG_DATA_FROM_SERVER);
		PHBItemList = new List<PHBItem> ();
		PoolManager.getInstance ().initPoolByType (type,this,1);

	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.PAIHANGBANG_DATA_FROM_SERVER:
				{
					JsonObject _data = (JsonObject)nt.data;
				}
				break;
			}
		}
	}
	void showPanel(int _type,Button btn){
		//if (btn.interactable)
		{
			curType = _type;
			if (btn != fightBtn) {fightBtn.interactable = true; }
			if (btn != heroBtn) {heroBtn.interactable = true; }
			if (btn != equipBtn) {equipBtn.interactable = true; }
			btn.interactable = false;
			JsonObject userMessage = new JsonObject();
			userMessage.Add ("phb_type", _type);
			userMessage.Add ("limit", 10);
			ServerManager.getInstance ().request("connector.roleHandler.getPaiHangBang", userMessage, (databack)=>{
				Debug.Log(databack.ToString());
				List<object> joList = databack["data"] as List<object>;
				int phb_type = int.Parse(databack["type"].ToString());
				for (int i = 0; i < joList.Count; i++) {
					JsonObject d = joList[i] as JsonObject;
					if(i == 0){
						phb_1.intiData(i+1,d,phb_type);
					}else if(i == 1){
						phb_2.intiData(i+1,d,phb_type);
					}else if(i == 2){
						phb_3.intiData(i+1,d,phb_type);
					}else{
						PHBItem _phbitem = (PHBItem)PoolManager.getInstance().getGameObject(PoolManager.PHB_4);
						_phbitem.transform.SetParent(content);
						_phbitem.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
						_phbitem.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
						_phbitem.intiData(i+1,d,phb_type);
						PHBItemList.Add(_phbitem);
					}

				}

				//AudioManager.instance.playEquip();
				//onClose();

			});
			//BagManager.getInstance ().showItemByType (_type.ToString(),1);
		}

	}
	public void onClickBtn(int _type){
		clear ();
		switch (_type) {
		case 1:
			showPanel (1,fightBtn);
			break;
		case 2:
			showPanel (2,heroBtn);
			break;
		case 3:
			showPanel (3,equipBtn);
			break;
		default:
			break;
		}
	}
	public void clear(){
		for (int i = 0; i < PHBItemList.Count; i++)
		{
			PHBItem go = PHBItemList[i];
			if (go != null) {
				PoolManager.getInstance ().addToPool (go.type,go);
			}
		}
		PHBItemList.Clear ();
	}
	public void onClose(){
		clear ();
		PoolManager.getInstance ().addToPool (this.type,this);
	}
}
