using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class ListPanel : MonoBehaviour {
	public string type;
	public Transform content;
	public List<MonoBehaviour> itemList;
	public string itemType;
	private MonoBehaviour openFrom;
	private int  openType;
	public int openitemId;
	public static ListPanel _currentListPanel;
	// Use this for initialization
	void Awake () {
		itemList = new List<MonoBehaviour> ();
		PoolManager.getInstance ().initPoolByType (type,this,1);
	}
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void onClickCloseBtn(){
		BagManager.getInstance ().Clear ();
		if (openFrom != null) {
			openFrom.gameObject.SetActive (true);
		}
		_currentListPanel = null;

		PoolManager.getInstance ().addToPool (type,this);
	}
	public void addItem(JsonObject data){
		JsonObject sd = BagManager.getInstance().getItemStaticData(data);
		BagPanel bagItem = (BagPanel)PoolManager.getInstance ().getGameObject (PoolManager.BAG_ITEM + sd["color"].ToString());

		bagItem.transform.SetParent(content);
		//bagItem.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		bagItem.init(data,openType);
		//itemList.Add (bagItem);

	}
	public void init(List<JsonObject> list,MonoBehaviour _openFrom = null,int _openType = 0,int _openitemId = 0){
		_currentListPanel = this;
		openFrom = _openFrom;
		openType = _openType;
		openitemId = _openitemId;
		if (openFrom != null) {
			openFrom.gameObject.SetActive (false);
		}
		for (int i = 0; i < list.Count; i++)
		{
			addItem (list[i]);
		}
	}
}
