using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class BagScene : Observer {
	public Transform content;
	public BagPanel _bagpanel;
	public Button equipTab;
	public Button bagTab;
	public Button heroShardTab;
	public Button equipShardTab;
	public Button stoneTab;
	public string curType;
	void Awake(){
		messageArr.Add (Message.BAG_ADD);
		BagManager.getInstance ()._bagScene = this;
	}
	// Use this for initialization
	void Start () {
		
		//BagManager.getInstance ().showAll ();

    }
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.BAG_ADD:
				{
					JsonObject _data = (JsonObject)nt.data;
					if (curType == _data ["itemType"].ToString ()) {
						add (_data);
					}
				}
				break;
			}
		}
	}
	public void reflesh(){
	}
	public void add(JsonObject cd,int openType = 0)
	{
		JsonObject sd = BagManager.getInstance().getItemStaticData(cd);;
		BagPanel bagItem = (BagPanel)PoolManager.getInstance ().getGameObject (PoolManager.BAG_ITEM + sd["color"].ToString());

		bagItem.transform.SetParent(content);
		bagItem.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		bagItem.init(cd,openType);
		/**
		//Button btn;
		if (content.childCount == 0)
		{
			BagPanel._demoPanel.transform.SetParent(content);
			BagPanel._demoPanel.init(cd);
			//OnChangeHero(cd,heroHeadDemo);
		}
		else
		{
			BagPanel panel = (BagPanel)GameObject.Instantiate(BagPanel._demoPanel, BagPanel._demoPanel.transform.position, BagPanel._demoPanel.transform.rotation, BagPanel._demoPanel.transform.parent);
			//btn.interactable = true;
			panel.init(cd);

			//btn.transform.SetParent (content.transform);
		}
		//heroHeadList.Add (btn);
		//btn.onClick.AddListener(delegate () {
		//    this.OnChangeHero(cd, btn);

		//});
		**/
	}
	void showPanel(string str,Button btn){
		//if (btn.interactable)
        {
			curType = str;
            if (btn != bagTab) {bagTab.interactable = true; }
			if (btn != equipTab) {equipTab.interactable = true; }
			if (btn != heroShardTab) {heroShardTab.interactable = true; }
			if (btn != equipShardTab) {equipShardTab.interactable = true; }
			if (btn != stoneTab) {stoneTab.interactable = true; }
            btn.interactable = false;
			BagManager.getInstance ().showItemByType (str);
        }
		
	}
	public void onclickBtn(int type){
		switch (type) {
		case 1:
			showPanel ("7",bagTab);
			break;
		case 2:
			showPanel ("equip",equipTab);
			break;
		case 3:
			showPanel ("8",heroShardTab);//英雄碎片
			break;
		case 4:
			showPanel ("9",equipShardTab);//碎片
			break;
		case 5:
			showPanel ("2",stoneTab);//宝石
			break;
		default:
			break;
		}
	}
}
