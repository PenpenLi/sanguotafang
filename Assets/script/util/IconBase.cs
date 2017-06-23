using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class IconBase : MonoBehaviour {

	public Image icon;
	public Image sub;
	public string type;
	private JsonObject data;
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
		data = jo;
		jo = BagManager.getInstance ().getItemStaticData (jo);


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
		icon.SetNativeSize();
		return this;
	}
	public void onClick(){
		AudioManager.instance.playBtnClick ();
		if (Func != null) {
			Func (data);
		}
	}
}
