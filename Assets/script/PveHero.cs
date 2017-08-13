using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveHero : PveEntity {

	// Use this for initialization
	public Dictionary<int,int> skillTurnDic;
	public JsonObject selectSkill;

	public int threat = 0;
	void Awake () {
		type = "PveHero";
		skillTurnDic = new Dictionary<int, int> ();

		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject jo,PveScene _pvescene){
		resetPropert ();
		pvescene = _pvescene;
		entityData = jo;
		threat = PropertyDic[Property.AP] = DataManager.getInstance().getHeroDamage(jo);
		PropertyDic[Property.HP] = PropertyDic[Property.MAXHP] = DataManager.getInstance().getHeroHp(jo);
		jo = HeroManager.getInstance ().getHeroStaticData (jo);
		style.sprite = Resources.Load("heroHanf/" + jo["style"].ToString(),typeof(Sprite)) as Sprite;
		hideSelect ();
		PropertyDic[Property.SPEED] = int.Parse (jo ["speed"].ToString ());
		initSkillTurn (jo);
		PropertyDic[Property.MP] = PropertyDic[Property.MAXMP] = 100;//默认全为100
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
			changeMP (20);
		}
	}
	/// <summary>
	/// /////////////
	/// </summary>
	/// <returns>The equip damage.</returns>
	/// <param name="isAddthreat">If set to <c>true</c> is addthreat.</param>
	public int getEquipDamage(bool isAddthreat = false){
		if (isAddthreat)
			threat += PropertyDic[Property.AP];
		return PropertyDic[Property.AP];
	}
	public bool isCanUseSkill(JsonObject skilldata){
		if (skilldata.ContainsKey ("skillType")) {
			int skillid = int.Parse (skilldata ["id"].ToString ());
			int needMP = skilldata.ContainsKey ("needMP") ? int.Parse (skilldata ["needMP"].ToString ()) : 20;
			int turn = skillTurnDic [skillid];
			if (turn == 0 && PropertyDic[Property.MP] >= needMP) {
				//selectSkill = skilldata;
				return true;
			}
			return false;
		}
		return true;
	}
	/// <summary>
	/// /////////
	/// </summary>
	/// <returns>The skill damage by skill identifier.</returns>
	/// <param name="skillid">Skillid.</param>
	/// <param name="isAddthreat">If set to <c>true</c> is addthreat.</param>
	public int getSkillDamageBySkillId(int skillid,bool isAddthreat = false){
		JsonObject jo = DataManager.getInstance ().skillDicJson [skillid];
		int _demage = int.Parse (jo ["attackDamage"].ToString ());
		float _add = float.Parse (jo ["attackAdd"].ToString ());
		PropertyDic[Property.SP] = _demage + (int)(_add * PropertyDic[Property.AP]);
		if (isAddthreat) {
			threat += PropertyDic[Property.SP];
		}
		return PropertyDic[Property.SP];
	}
	/// <summary>
	/// /////////
	/// </summary>
	/// <returns>The skill damage by skill identifier.</returns>
	/// <param name="isAddthreat">If set to <c>true</c> is addthreat.</param>
	public int getSelectedSkillDamage(bool isAddthreat = false){
		//int skillid = selectSkill
		//JsonObject jo = DataManager.getInstance ().skillDicJson [skillid];
		int _demage = int.Parse (selectSkill ["attackDamage"].ToString ());
		float _add = float.Parse (selectSkill["attackAdd"].ToString ());
		PropertyDic[Property.SP] = _demage + (int)(_add * PropertyDic[Property.AP]);
		if (isAddthreat) {
			threat += PropertyDic[Property.SP];
		}
		return PropertyDic[Property.SP];
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
		base.onClick ();
		/**if (isCanHit) {
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
					onHit (-pvescene.pveHero.getSelectedSkillDamage(true));
					break;
				case 202:
					pvescene.attackAllHero (-pvescene.pveHero.getSelectedSkillDamage(true));
					break;
				default:
					//onHit (-100);
					break;
				}
				//pvescene.checkBout ();
			} 

			/////////////////////////////////////////////////////////////////////

		}**/
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
