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
	public int playerId;
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
		if (data.ContainsKey ("playerId")) {
			playerId = int.Parse(data ["playerId"].ToString ());
		} else {
			playerId = int.Parse(data ["id"].ToString ());
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
	public void showPanel(JsonObject playerData){
		HeroManager.getInstance ().initData (playerData);
		BagManager.getInstance ().initData (playerData);
		if (phb_type == 1) {

			OtherInfoPanel._Current.initData (playerData);
			OtherInfoPanel._Current.gameObject.SetActive (true);
			OtherInfoPanel._Current.transform.SetParent (Loom.Current.transform);
			OtherInfoPanel._Current.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			OtherInfoPanel._Current.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		} else if (phb_type == 2) {
			OtherInfoPanel._Current.initData (playerData, int.Parse (data ["heroId"].ToString ()));
			OtherInfoPanel._Current.gameObject.SetActive (true);
			OtherInfoPanel._Current.transform.SetParent (Loom.Current.transform);
			OtherInfoPanel._Current.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			OtherInfoPanel._Current.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		} else {
			EquipInfo _equipInfo = (EquipInfo)PoolManager.getInstance ().getGameObject (PoolManager.EQUIP_INFO);
			_equipInfo.transform.SetParent (Loom.Current.transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			_equipInfo.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_equipInfo.init (BagManager.getInstance ().getEquipById (int.Parse (data ["id"].ToString ())), 0);
		}

	}
	public void onClick(){
		
		if (OtherInfoPanel._Current.otherPlayerDataCacheArr.ContainsKey (playerId)) {
			showPanel (OtherInfoPanel._Current.otherPlayerDataCacheArr[playerId]);
		} else {
			JsonObject userMessage = new JsonObject ();
			if (phb_type == 1) {
				userMessage.Add ("playerId", playerId);
				userMessage.Add ("phb_type", 4);
			} else if (phb_type == 2) {
				userMessage.Add ("playerId", playerId);
				userMessage.Add ("heroId", data ["heroId"]);
				userMessage.Add ("phb_type", 5);
			} else if (phb_type == 3) {
				userMessage.Add ("playerId", playerId);
				userMessage.Add ("equipId", data ["id"]);
				userMessage.Add ("phb_type", 6);
			}
			//userMessage.Add ("limit", 10);
			ServerManager.getInstance ().request ("connector.roleHandler.getPaiHangBang", userMessage, (databack) => {
				Debug.Log (databack.ToString ());
				int _type = int.Parse (databack ["type"].ToString ());
				JsonObject playerData = databack ["data"] as JsonObject;
				OtherInfoPanel._Current.otherPlayerDataCacheArr [playerId] = playerData;
				showPanel(playerData);
				//List<object> joList = databack["data"] as List<object>;

				//AudioManager.instance.playEquip();
				//onClose();

			});
		}
	}
}
