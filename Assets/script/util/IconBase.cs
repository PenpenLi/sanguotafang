using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class IconBase : MonoBehaviour {

	public static int ITEM_TYPE_YUANBAO = 0;//元宝
	public static int ITEM_TYPE_YINBI = 1;//银币
	public static int ITEM_TYPE_STONE = 2;//宝石
	public static int ITEM_TYPE_EQUIP = 5;//装备
	public static int ITEM_TYPE_EXP = 7;//经验丹
	public static int ITEM_TYPE_HEROSUB = 8;//英雄碎片
	public static int ITEM_TYPE_EQUIPSUB = 9;//装备碎片

	public Image icon;
	public Image sub;
	public string type;
	public Text count;
	private JsonObject data;
	public int itemType = 0;
	public callBackFunc<JsonObject> Func;

	void Awake () {
		//DontDestroyOnLoad (this);
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		//UGUIEventTrigger.Get (icon.gameObject).AddEventListener (EventTriggerType.PointerClick,OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public IconBase init(JsonObject jo){
		sub.gameObject.SetActive (false);
		count.gameObject.SetActive (false);
		itemType = 0;
		data = jo;
		jo = BagManager.getInstance ().getItemStaticData (jo);
		if (!jo.ContainsKey ("icon")) {
			jo = HeroManager.getInstance ().getHeroStaticData (jo);
		}
		if (jo.ContainsKey ("itemType")) {
			itemType = int.Parse (jo ["itemType"].ToString ());
		}

		Func = null;
		if (jo.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + jo["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else {
			icon.sprite = (Resources.Load("icon/" + jo["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		if (jo.ContainsKey ("heroId")) {//英雄碎片
			int heroId = int.Parse (jo ["heroId"].ToString ());
			if (int.Parse (jo ["heroId"].ToString ()) >= 6000) {
				sub.gameObject.SetActive (true);
			}
		}
		//icon.SetNativeSize();
		setCount ("count","x",1);
		if (itemType == 5 || itemType == 0) {
			setCount ("level", "Lv.");
		}
		return this;
	}
	public void setCount(string type,string fromat,int limit = 0){
		if (data.ContainsKey (type)) {
			int _count = DataManager.getInstance ().getJsonIntValue (data,type);
			if (_count > limit) {
				count.gameObject.SetActive (true);
				count.text = fromat + _count.ToString ();
			}
		}
	}
	public void onClick(){
		AudioManager.instance.playBtnClick ();
		if (Func != null) {
			Func (data);
		}
	}
}
