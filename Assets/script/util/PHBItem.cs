using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PHBItem : MonoBehaviour {

	// Use this for initialization
	public Text number;
	public Text name;
	public Text fight;
	public IconBase icon;
	public JsonObject data;
	public string type;
	public int phb_type;
	void Awake () {
		if (type == "phb_4") {
			PoolManager.getInstance ().initPoolByType (type,this,5);
		}
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDisable(){
		
	}
	public void intiData(int _number,JsonObject jo,int _phb_type){
		data = jo;
		phb_type = _phb_type;
		number.text = _number.ToString ();
		if (jo.ContainsKey ("name")) {
			name.text = jo ["name"].ToString ();
		} else {
			name.text = jo ["id"].ToString ();
		}

		//icon.gameObject.SetActive (true);
		//icon.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		if (icon != null) {
			PoolManager.getInstance ().addToPool (icon.type,icon);
			icon = null;
		}
		JsonObject sjo = null;
		if (jo.ContainsKey ("itemId")) {
			sjo = BagManager.getInstance ().getItemStaticData (jo);
		} else if (jo.ContainsKey ("heroId")) {
			sjo = HeroManager.getInstance ().getHeroStaticData (jo);
		}

		if (sjo != null) {
			//if (icon == null) {
				icon = (IconBase)PoolManager.getInstance ().getGameObject (sjo["color"].ToString());
			//}
			icon.transform.SetParent (this.transform);

			icon.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			icon.transform.localPosition = new Vector3 (25.0f, 0.0f, 0.0f);

			icon.init (sjo);
		}

		fight.text = jo ["fightPoint"].ToString ();
	}
	public void onClick(){
		JsonObject userMessage = new JsonObject();
		if (phb_type == 1) {
			userMessage.Add ("playerId", data ["id"]);
			userMessage.Add ("phb_type", 4);
		} else if (phb_type == 2) {
			userMessage.Add ("playerId", data ["playerId"]);
			userMessage.Add ("heroId", data ["heroId"]);
		} else if (phb_type == 3) {
			userMessage.Add ("playerId", data ["playerId"]);
			userMessage.Add ("equipId", data ["id"]);
		}
		//userMessage.Add ("limit", 10);
		ServerManager.getInstance ().request("connector.roleHandler.getPaiHangBang", userMessage, (databack)=>{
			Debug.Log(databack.ToString());
			JsonObject playerData = databack["data"] as JsonObject;
			HeroManager.getInstance().initData(playerData);
			BagManager.getInstance().initData(playerData);
			OtherInfoPanel._Current.initData(playerData);
			OtherInfoPanel._Current.gameObject.SetActive(true);
			OtherInfoPanel._Current.transform.SetParent(Loom.Current.transform);
			OtherInfoPanel._Current.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			OtherInfoPanel._Current.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			//List<object> joList = databack["data"] as List<object>;

			//AudioManager.instance.playEquip();
			//onClose();

		});
	}
}
