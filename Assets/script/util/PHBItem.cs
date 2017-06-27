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
	public string type;
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
	public void intiData(int _number,JsonObject jo){
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
		
	}
}
