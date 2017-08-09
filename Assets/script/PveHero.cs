using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveHero : PveEntity {

	// Use this for initialization
	public Dictionary<int,int> skillTurnDic;
	public JsonObject selectSkill;
	void Start () {
		skillTurnDic = new Dictionary<int, int> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject jo,PveScene _pvescene){
		pvescene = _pvescene;
		entityData = jo;
		currentHP = maxHP = DataManager.getInstance().getHeroHp(jo);
		jo = HeroManager.getInstance ().getHeroStaticData (jo);
		style.sprite = Resources.Load("heroHanf/" + jo["style"].ToString(),typeof(Sprite)) as Sprite;
		hideSelect ();
		speed = int.Parse (jo ["speed"].ToString ());
		initSkillTurn (jo);
		//select.gameObject.SetActive (false);
		//style.SetNativeSize ();
	}
	public void initSkillTurn(JsonObject staticdata){
		skillTurnDic.Clear ();
		for (int i = 1; i <= 4; i++) {
			int skillid = int.Parse (staticdata ["skill" + i.ToString ()].ToString ());
			JsonObject skilldata = DataManager.getInstance().skillDicJson[skillid];
			skillTurnDic [skillid] = 0;//int.Parse (staticdata ["turn"].ToString ());
		}
	}
	public void resetSkillTurn(){
		if (selectSkill.ContainsKey ("skillType")) {
			int skillid = int.Parse (selectSkill ["id"].ToString ());
			skillTurnDic [skillid] = int.Parse (selectSkill ["turn"].ToString ());
		}
	}
	public void updateSkillTurn(){
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (entityData);
		for (int i = 1; i <= 4; i++) {
			int skillid = int.Parse (staticdata ["skill" + i.ToString ()].ToString ());
			JsonObject skilldata = DataManager.getInstance().skillDicJson[skillid];
			if (skillTurnDic [skillid] > 0) {
				skillTurnDic [skillid]--;
			}
		}
	}
	public void onClick(){
		if (isCanHit) {
			pvescene.ischeckBout = false;
			isCanHit = false;
			pvescene.hideAllHeroSelect ();
			/////////////////////////////////////////////////////////////////
			JsonObject jo = pvescene.pveHero.selectSkill;
			if (jo != null) {//选择的是技能
				int skillType = int.Parse (jo ["skillType"].ToString ());
				//pvescene.pveHero.updateSkillTurn ();
				switch (skillType) {
				case 201:
					onHit (-pvescene.skillDamage);
					break;
				case 202:
					pvescene.attackAllHero (-pvescene.skillDamage);
					break;
				default:
					onHit (-100);
					break;
				}
				//pvescene.checkBout ();
			} 

			/////////////////////////////////////////////////////////////////////

		}
	}
	public override void active(){
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		pvescene.setSkillsAndEquip (this);
		pvescene.showAllMonsterSelect ();
	}
	public override void disActive(){
		iTween.Stop (style.gameObject);
		style.transform.localPosition = Vector3.zero;

	}
}
