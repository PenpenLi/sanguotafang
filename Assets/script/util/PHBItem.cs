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
	public void intiData(int _number,JsonObject jo){
		number.text = _number.ToString ();
		if (jo.ContainsKey("name")) {
			name.text = jo["name"].ToString ();
		} else {
			name.text = jo["id"].ToString ();
		}
		icon.gameObject.SetActive (true);
		//icon.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		if (jo.ContainsKey ("itemId")) {
			icon.init (BagManager.getInstance ().getItemStaticData (jo));
		} else if (jo.ContainsKey ("heroId")) {
			icon.init (HeroManager.getInstance ().getHeroStaticData (jo));
		} else {
			icon.gameObject.SetActive (false);
		}
		fight.text = jo ["fightPoint"].ToString ();
	}
	public void onClick(){
		
	}
}
