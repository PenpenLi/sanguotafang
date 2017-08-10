using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveScene : Observer {

	//public PveMonster monster1;
	//public PveMonster monster2;
	//public PveMonster monster3;

	//public PveHero pvehero1;
	//public PveHero pvehero2;
	//public PveHero pvehero3;
	//public PveHero pvehero4;
	public Dictionary<int,PveHero> PveHeroList;
	public Dictionary<int,PveMonster> PveMonsterList;
	public Dictionary<int,Button> skillList;
	public List<PveEntity> PveEntityList;
	public ArrayList IconBaseList;

	public Button equip;
	public Button skill1;
	public Button skill2;
	public Button skill3;
	public Button skill4;

	public Text skillInfo;
	public RawImage selectKuang;
	public int actIndex = 0;

	public Image bg;
	public Image heroPanel;
	public Image skillPanel;
	public Transform attackList;
	public int monsterBoShu = 1;//怪物第几波
	public PveHero pveHero;//当前出手的英雄数据
	public bool ischeckBout = false;
	// Use this for initialization
	void Start () {
		PveHeroList = new Dictionary<int, PveHero> ();
		PveMonsterList = new Dictionary<int, PveMonster> ();
		PveEntityList = new List<PveEntity> ();
		skillList = new Dictionary<int, Button> ();
		IconBaseList = new ArrayList ();
		//PveHeroList [0] = pvehero1;
		//PveHeroList [1] = pvehero2;
		//PveHeroList [2] = pvehero3;
		//PveHeroList [3] = pvehero4;

		//PveMonsterList [0] = monster1;
		//PveMonsterList [1] = monster2;
		//PveMonsterList [2] = monster3;

		skillList [0] = skill1;
		skillList [1] = skill2;
		skillList [2] = skill3;
		skillList [3] = skill4;
		//foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
		//	kvp.Value.gameObject.SetActive (false);
		//}

		initBg ();
		initHero ();
		///////////////////////////////////////////////////////////
		checkBout();
		//initMonster();
		//sortEntityBySpeed ();//根据出手速度进行排序

	}
	public void initBg(){
		bg.sprite = (Resources.Load (ChapterManager2.getInstance ().mapPath,typeof(Sprite)) as Sprite);
		//bg.SetNativeSize ();

		//屏幕适配,按宽度缩放
		/**float retio = (float)(Screen.width) / (float)(bg.sprite.rect.width);
		float retioBg = (float)(Screen.height) / (float)(bg.sprite.rect.height);
		if (retio < retioBg)
		{
			bg.transform.localScale = new Vector3(retio, retio, 0);
		}
		else
		{
			bg.transform.localScale = new Vector3(retioBg, retioBg, 0);
		}
		**/
	}
	public void initHero(){
		Dictionary<int,JsonObject> heroarr = HeroManager.getInstance().getHeros();
		int index = 0;
		int pos = 7;
		foreach(KeyValuePair<int,JsonObject> kvp in heroarr)
		{
			//PveHeroList[index].gameObject.SetActive (true);
			//PveHeroList[index].init (kvp.Value,this);
			PveHero pvehero = (PveHero)PoolManager.getInstance ().getGameObject ("PveHero");
			//foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			pvehero.transform.SetParent (heroPanel.transform);
			pvehero.transform.localScale = Vector3.one;
			JsonObject _monsterpos = DataManager.getInstance ().pvePosJson [pos];
			pvehero.transform.localPosition = new Vector3 (float.Parse (_monsterpos ["x"].ToString ()), float.Parse (_monsterpos ["y"].ToString ()), float.Parse (_monsterpos ["z"].ToString ()));
			pvehero.init (kvp.Value,this);
			PveHeroList [index] = pvehero;
			index++;
			pos++;
			////////////////////////////////////

		}
	}
	public PveHero getHighestThreatHero(){//获取仇恨值最高的英雄，供怪物虐
		int threat = 0;
		PveHero pvehero = null;
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			if (kvp.Value.isActiveAndEnabled) {
				if (kvp.Value.threat > threat) {
					threat = kvp.Value.threat;
					pvehero = kvp.Value;
				}
			}
		}
		return pvehero;
	}
	public void attackAllMonster(int damage){
		foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			if (kvp.Value.isActiveAndEnabled) {
				kvp.Value.onHit (damage);
			}
		}
	}
	public void attackAllHero(int damage){
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			if (kvp.Value.isActiveAndEnabled) {
				kvp.Value.onHit (damage);
			}
		}
	}
	public void hideAllMonsterSelect(){
		foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			kvp.Value.hideSelect ();
		}
	}
	public void showAllMonsterSelect(){
		foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			kvp.Value.showSelect ();
		}
	}
	public void hideAllHeroSelect(){
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			kvp.Value.hideSelect ();
		}
	}
	public void showAllHeroSelect(){
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			kvp.Value.showSelect ();
		}
	}
	public void initMonster(){
		MonsterManager.getInstance ().initMonsterData (ChapterManager2.getInstance ().monsterPath);
		PveMonsterList.Clear ();
		ArrayList monsterArr = MonsterManager.getInstance ().getPveMonstersByBoShu (monsterBoShu,bg.transform);
		if (monsterArr.Count > 0) {
			Bleed bleed = (Bleed)PoolManager.getInstance ().getGameObject ("Bleed");
			bleed.transform.SetParent (bg.transform);
			bleed.show ("第 " + monsterBoShu.ToString() + " 回合",() => {
				for (int i = 0; i < monsterArr.Count; i++) {
					string[] oneData = (string[])monsterArr [i];
					JsonObject _monsterData = DataManager.getInstance ().monsterDicJson [int.Parse (oneData [1])];
					PveMonster pvmonster = (PveMonster)PoolManager.getInstance ().getGameObject ("PveMonster");
					//foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
					pvmonster.transform.SetParent (bg.transform);
					pvmonster.transform.localScale = Vector3.one;
					JsonObject _monsterpos = DataManager.getInstance ().pvePosJson [int.Parse (oneData [2])];
					pvmonster.transform.localPosition = new Vector3 (float.Parse (_monsterpos ["x"].ToString ()), float.Parse (_monsterpos ["y"].ToString ()), float.Parse (_monsterpos ["z"].ToString ()));
					pvmonster.init (_monsterData, this);
					PveMonsterList [i] = pvmonster;
					//}
				}
				monsterBoShu++;
				sortEntityBySpeed ();
				setNextAttackEntityBySpeed ();
			});
			//Loom.QueueOnMainThread (, 2.0f);

		} else {
			gameOver ();//战斗结束
		}

	}
	public void checkBout(){//检查本回合是否结束
		
		bool isOver = true;
		bool isAllHeroDead = true;
		foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			if (kvp.Value.isActiveAndEnabled) {
				isOver = false;
			}
		}
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			if (kvp.Value.isActiveAndEnabled) {
				isAllHeroDead = false;
			}
		}
		if (isOver) {//开始下一回合战斗
			//减英雄回合数
			foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
				if (kvp.Value.isActiveAndEnabled) {
					kvp.Value.updateSkillTurn ();
				}
			}
			initMonster ();
			skillPanel.gameObject.SetActive (false);
			//sortEntityBySpeed ();
		} else if (isAllHeroDead) {
			gameOver ();
		} else {
			if (!ischeckBout) {
				ischeckBout = true;
				setNextAttackEntityBySpeed ();
			}
		}
	}
	public void gameOver(){
		quitScene ();
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void sortEntityBySpeed(){
		if (actIndex > 0 && PveEntityList.Count > actIndex) {
			PveEntityList [actIndex - 1].disActive ();
		}
		actIndex = 0;
		PveEntityList.Clear ();
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			PveEntityList.Add (kvp.Value);
		}
		foreach (KeyValuePair<int,PveMonster> kvp in PveMonsterList) {
			PveEntityList.Add (kvp.Value);
		}
		PveEntity temp;

		//冒泡排序
		for (int i = 0; i < PveEntityList.Count; i++)
		{
			for (int j = i + 1; j < PveEntityList.Count; j++)
			{
				if (PveEntityList[j].speed > PveEntityList[i].speed)
				{
					temp = PveEntityList[j];
					PveEntityList[j] = PveEntityList[i];
					PveEntityList[i] = temp;
				}
			}
		}
		IconBase[] array = attackList.GetComponentsInChildren<IconBase> ();
		for(int i = 0; i < array.Length ; i++){
			IconBase icon = array [i];
			PoolManager.getInstance ().addToPool (icon.type,icon);
		}
		for (int i = 0; i < PveEntityList.Count; i++)
		{
			
			JsonObject jo = DataManager.getInstance ().getStaticData (PveEntityList [i].entityData);
			IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo["color"].ToString());
			icon.init (PveEntityList [i].entityData).Func = new callBackFunc<JsonObject> (onClickCallBack);
			icon.transform.SetParent (attackList);
			//icon.transform.localPosition = Vector3.zero;
			icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			PveEntityList [i].iconBase = icon;
		}

	
	}
	public void setNextAttackEntityBySpeed(){//通过优先度选择下一个出手的对象
		if (PveEntityList.Count > 0) {

			if (actIndex > 0) {
				PveEntityList [actIndex - 1].disActive ();
				PveEntityList [actIndex - 1].iconBase.transform.SetParent (null);
				PveEntityList [actIndex - 1].iconBase.transform.SetParent (attackList);

			}
			if (actIndex >= PveEntityList.Count) {
				//新的一轮回合

				actIndex = 0;
			}
			if (pveHero != null) {
				pveHero.resetSkillTurn ();
			}
			pveHero = null;
			if (PveEntityList [actIndex].isActiveAndEnabled) {
				PveEntityList [actIndex].active ();
				skillPanel.gameObject.SetActive (true);
				actIndex++;
			} else {
				PveEntityList.RemoveAt (actIndex);
				setNextAttackEntityBySpeed ();
			}
		}
	}
	public void addIcon(Button btn,JsonObject jo,int turn = 0){
		IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo["color"].ToString());
		icon.init (jo).Func = new callBackFunc<JsonObject> (onClickCallBack);
		icon.transform.SetParent (btn.transform);
		icon.transform.localPosition = Vector3.zero;
		icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		//equip.gameObject.SetActive (true);
		if (turn > 0) {
			icon.count.gameObject.SetActive (true);
			icon.count.text = turn.ToString () + "冷却回合";
		} else {
			icon.count.gameObject.SetActive (false);
		}

		IconBaseList.Add(icon);
	}
	public void setSkillsAndEquip(PveHero pvehero){
		///////////////////////清理图标///////////////////////////////
		for(int i=0;i < IconBaseList.Count;i++){
			IconBase icon = (IconBase)IconBaseList[i];
			if (icon != null) {
				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		IconBaseList.Clear ();
		/////////////////////////////////////////////////////////////////
		pveHero = pvehero;
		int heroId = int.Parse (pvehero.entityData ["heroId"].ToString ());
		//equipDamage =  DataManager.getInstance().getHeroDamage(pvehero.entityData);
		ArrayList equipArr = BagManager.getInstance ().getEquipByHeroId (heroId);
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (pvehero.entityData);

		//bool isHaveEquip = false;
		JsonObject sdjo = DataManager.getInstance().itemDicJson[10000];
		for(int i=0;i < equipArr.Count;i++){	
			JsonObject jo = equipArr [i] as JsonObject;
			JsonObject sdjo2 =  BagManager.getInstance().getItemStaticData (jo);
			string key = sdjo2 ["kind"].ToString ();
			if (key == "weapon") {
				sdjo = sdjo2;
				break;
				//addIcon (equip,sdjo);
				//onClickEquip (sdjo);
				//isHaveEquip = true;
			}
		}
		//if (!isHaveEquip) {//没有装备武器
			
			addIcon (equip,sdjo);
			onClickEquip (sdjo);
		//}
		for (int i = 1; i <= 4; i++) {
			int skillid = int.Parse (staticdata ["skill" + i.ToString ()].ToString ());
			JsonObject skilldata = DataManager.getInstance().skillDicJson[skillid];
			Button skill = skillList[i-1];
			skill.image.sprite = Resources.Load(skilldata["icon"].ToString(),typeof(Sprite)) as Sprite;
			skill.image.SetNativeSize ();
			skilldata ["pos"] = i;
			//skilldata ["currentTurn"] = pvehero.skillTurnDic[skillid];
			addIcon (skill,skilldata,pvehero.skillTurnDic[skillid]);
		}
	}
	public void onClickEquip(JsonObject jo){
		//Debug.Log (jo.ToString());
		pveHero.selectSkill = jo;
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (pveHero.entityData);
		int id = int.Parse (staticdata ["attackType"].ToString ()) + 10000;
		skillInfo.text = string.Format (DataManager.getInstance ().languageJson [id]["name"].ToString(), pveHero.getEquipDamage());
		selectKuang.transform.SetParent (null);
		selectKuang.transform.SetParent (equip.transform);
		selectKuang.transform.localPosition = Vector3.zero;
		hideAllHeroSelect ();
		showAllMonsterSelect ();

	}
	public void onClickCallBack(JsonObject jo){
		if (jo.ContainsKey ("skillType")) {//选择的是技能
			onClickSkill(jo);
		} else {//选择的是武器
			onClickEquip(jo);

		}

	}
	public void onClickSkill(JsonObject jo){
		//Debug.Log (jo.ToString());
		//int turn = int.Parse (jo["currentTurn"].ToString());
		if (pveHero.isCanUseSkill(jo)) {//回合数冷却了之后才能用
			//pveHero.selectSkill = jo;
			int target = int.Parse (jo ["target"].ToString ());
			//int _demage = int.Parse (jo ["attackDamage"].ToString ());
			//float _add = float.Parse (jo ["attackAdd"].ToString ());
			//技能自身伤害+普攻的百分比伤害
			skillInfo.text = skillInfo.text = string.Format (jo ["desc"].ToString (), pveHero.getSelectedSkillDamage());
			int pos = int.Parse (jo ["pos"].ToString ());
			Button skill = skillList [pos - 1];
			selectKuang.transform.SetParent (null);
			selectKuang.transform.SetParent (skill.transform);
			selectKuang.transform.localPosition = Vector3.zero;
			if (target == 1) {
				hideAllHeroSelect ();
				showAllMonsterSelect ();
			} else {
				hideAllMonsterSelect ();
				showAllHeroSelect ();
			}
		}
	}
	/// <summary>
	/// //////////
	/// </summary>
	/// <param name="pveentity">对攻击的对象.</param>
	public void attackEntity(PveEntity pveentity){
		ischeckBout = false;
		hideAllHeroSelect ();
		hideAllMonsterSelect ();
		/////////////////////////////////////////////////////////////////
		JsonObject jo = pveHero.selectSkill;
		if (jo != null && jo.ContainsKey ("skillType")) {//选择的是技能
			int skillType = int.Parse (jo["skillType"].ToString ());
			//pvescene.pveHero.updateSkillTurn ();
			switch (skillType) {
			case 1:
				pveentity.onHit (pveHero.getSelectedSkillDamage(true));
				break;
			case 2:
				attackAllMonster (pveHero.getSelectedSkillDamage(true));
				break;
			case 201:
				pveentity.onHit (-pveHero.getSelectedSkillDamage(true));
				break;
			case 202:
				attackAllHero (-pveHero.getSelectedSkillDamage(true));
				break;
			default:
				break;
			}

		} else {//选择的是武器
			Effect effect = (Effect)PoolManager.getInstance ().getGameObject ("Effect");
			effect.transform.SetParent (pveentity.transform);
			effect.transform.localPosition = Vector3.zero;
			effect.init ("skill/pugong");
			pveentity.onHit (pveHero.getEquipDamage(true));
		}
		//pvescene.checkBout ();
		/////////////////////////////////////////////////////////////////////
	}
	public void quitScene(){
		PoolManager.getInstance ().clearPool ();
		SceneManager.LoadScene ("GameScene");
	}
}
