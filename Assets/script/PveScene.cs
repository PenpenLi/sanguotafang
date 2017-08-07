using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveScene : MonoBehaviour {

	public PveMonster monster1;
	public PveMonster monster2;
	public PveMonster monster3;

	public PveHero pvehero1;
	public PveHero pvehero2;
	public PveHero pvehero3;
	public PveHero pvehero4;
	public Dictionary<int,PveHero> PveHeroList;
	public Dictionary<int,Button> skillList;
	public ArrayList IconBaseList;

	public Button equip;
	public Button skill1;
	public Button skill2;
	public Button skill3;
	public Button skill4;

	public Text skillInfo;
	// Use this for initialization
	void Start () {
		PveHeroList = new Dictionary<int, PveHero> ();
		skillList = new Dictionary<int, Button> ();
		IconBaseList = new ArrayList ();
		PveHeroList [0] = pvehero1;
		PveHeroList [1] = pvehero2;
		PveHeroList [2] = pvehero3;
		PveHeroList [3] = pvehero4;

		skillList [0] = skill1;
		skillList [1] = skill2;
		skillList [2] = skill3;
		skillList [3] = skill4;
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			kvp.Value.gameObject.SetActive (false);
		}

		Dictionary<int,JsonObject> heroarr = HeroManager.getInstance().getHeros();
		int index = 0;
		foreach(KeyValuePair<int,JsonObject> kvp in heroarr)
		{

			PveHeroList[index].init (kvp.Value,this);
			PveHeroList[index].gameObject.SetActive (true);
			if(index == 0){
				setSkillsAndEquip (kvp.Value);
			}
			index++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setSkillsAndEquip(JsonObject herodata){
		///////////////////////清理图标///////////////////////////////
		for(int i=0;i < IconBaseList.Count;i++){
			IconBase icon = (IconBase)IconBaseList[i];
			if (icon != null) {
				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		IconBaseList.Clear ();
		/////////////////////////////////////////////////////////////////
		int heroId = int.Parse (herodata ["heroId"].ToString ());
		ArrayList equipArr = BagManager.getInstance ().getEquipByHeroId (heroId);
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		for(int i=0;i < equipArr.Count;i++){	
			JsonObject jo = equipArr [i] as JsonObject;
			JsonObject sdjo =  BagManager.getInstance().getItemStaticData (jo);
			string key = sdjo ["kind"].ToString ();
			if (key == "weapon") {
				IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (sdjo["color"].ToString());
				icon.init (sdjo).Func = new callBackFunc<JsonObject> (onClickEquip);
				icon.transform.SetParent (equip.transform);
				icon.transform.localPosition = Vector3.zero;
				icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
				//equip.gameObject.SetActive (true);
				IconBaseList.Add(icon);
			}
		}
		for (int i = 1; i <= 4; i++) {
			JsonObject skilldata = DataManager.getInstance().skillDicJson[int.Parse(staticdata["skill" + i.ToString()].ToString())];
			//skillName.text = skilldata["name"].ToString();
			Button skill = skillList[i-1];
			skill.image.sprite = Resources.Load(skilldata["icon"].ToString(),typeof(Sprite)) as Sprite;
			skill.image.SetNativeSize ();
			IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (skilldata["color"].ToString());
			icon.init (skilldata).Func = new callBackFunc<JsonObject> (onClickSkill);
			//icon.Func = new callBackFunc<JsonObject> (onCallBack);
			icon.transform.SetParent (skill.transform);
			icon.transform.localPosition = Vector3.zero;
			icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			IconBaseList.Add(icon);
			//iTween.ScaleTo(icon.gameObject, iTween.Hash("y", 0.5f,"x", 0.5f,"z", 0.5f ,"delay", 0.0f,"time",0.5f));
			//skill.gameObject.SetActive (true);
		}
	}
	public void onClickEquip(JsonObject jo){
		//Debug.Log (jo.ToString());
		skillInfo.text = jo["desc"].ToString();
	}
	public void onClickSkill(JsonObject jo){
		//Debug.Log (jo.ToString());
		skillInfo.text = jo["desc"].ToString();
	}
	public void quitScene(){
		PoolManager.getInstance ().clearPool ();
		SceneManager.LoadScene ("GameScene");
	}
}
