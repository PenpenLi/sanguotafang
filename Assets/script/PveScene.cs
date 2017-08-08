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

	public PveHero pvehero1;
	public PveHero pvehero2;
	public PveHero pvehero3;
	public PveHero pvehero4;
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
	public JsonObject selectSkill;
	public int equipDamage =0;
	public Image bg;
	public int monsterBoShu = 1;//怪物第几波
	// Use this for initialization
	void Start () {
		PveHeroList = new Dictionary<int, PveHero> ();
		PveMonsterList = new Dictionary<int, PveMonster> ();
		PveEntityList = new List<PveEntity> ();
		skillList = new Dictionary<int, Button> ();
		IconBaseList = new ArrayList ();
		PveHeroList [0] = pvehero1;
		PveHeroList [1] = pvehero2;
		PveHeroList [2] = pvehero3;
		PveHeroList [3] = pvehero4;

		//PveMonsterList [0] = monster1;
		//PveMonsterList [1] = monster2;
		//PveMonsterList [2] = monster3;

		skillList [0] = skill1;
		skillList [1] = skill2;
		skillList [2] = skill3;
		skillList [3] = skill4;
		foreach (KeyValuePair<int,PveHero> kvp in PveHeroList) {
			kvp.Value.gameObject.SetActive (false);
		}

		initBg ();
		initHero ();
		///////////////////////////////////////////////////////////
		initMonster();
		//sortEntityBySpeed ();//根据出手速度进行排序
		setNextAttackEntityBySpeed ();
	}
	public void initBg(){
		bg.sprite = (Resources.Load (ChapterManager2.getInstance ().mapPath,typeof(Sprite)) as Sprite);
		//bg.SetNativeSize ();

		//屏幕适配,按宽度缩放
		float retio = (float)(Screen.width) / (float)(bg.sprite.rect.width);
		float retioBg = (float)(Screen.height) / (float)(bg.sprite.rect.height);
		if (retio < retioBg)
		{
			bg.transform.localScale = new Vector3(retio, retio, 0);
		}
		else
		{
			bg.transform.localScale = new Vector3(retioBg, retioBg, 0);
		}
	}
	public void initHero(){
		Dictionary<int,JsonObject> heroarr = HeroManager.getInstance().getHeros();
		int index = 0;
		foreach(KeyValuePair<int,JsonObject> kvp in heroarr)
		{

			PveHeroList[index].init (kvp.Value,this);
			PveHeroList[index].gameObject.SetActive (true);
			index++;
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
			initMonster ();
			//sortEntityBySpeed ();
		}else if(isAllHeroDead){
			gameOver ();
		}
	}
	public void gameOver(){
		quitScene ();
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void sortEntityBySpeed(){
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
	}
	public void setNextAttackEntityBySpeed(){//通过优先度选择下一个出手的对象
		if (PveEntityList.Count > 0) {
			if (actIndex > 0) {
				PveEntityList [actIndex - 1].disActive ();
			}
			if (actIndex >= PveEntityList.Count) {
				actIndex = 0;
			}
			if (PveEntityList [actIndex].isActiveAndEnabled) {
				PveEntityList [actIndex].active ();
				actIndex++;
			} else {
				PveEntityList.RemoveAt (actIndex);
				setNextAttackEntityBySpeed ();
			}
		}
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
		equipDamage =  DataManager.getInstance().getHeroDamage(herodata);
		ArrayList equipArr = BagManager.getInstance ().getEquipByHeroId (heroId);
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		onClickEquip (null);
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
				onClickEquip (sdjo);
			}
		}
		for (int i = 1; i <= 4; i++) {
			JsonObject skilldata = DataManager.getInstance().skillDicJson[int.Parse(staticdata["skill" + i.ToString()].ToString())];
			//skillName.text = skilldata["name"].ToString();
			Button skill = skillList[i-1];
			skill.image.sprite = Resources.Load(skilldata["icon"].ToString(),typeof(Sprite)) as Sprite;
			skill.image.SetNativeSize ();
			IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (skilldata["color"].ToString());
			skilldata ["pos"] = i;
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
		selectSkill = jo;
		if (jo != null) {
			skillInfo.text = jo ["desc"].ToString ();
		}
		selectKuang.transform.SetParent (null);
		selectKuang.transform.SetParent (equip.transform);
		selectKuang.transform.localPosition = Vector3.zero;
		hideAllHeroSelect ();
		showAllMonsterSelect ();
	}
	public void onClickSkill(JsonObject jo){
		//Debug.Log (jo.ToString());
		selectSkill = jo;
		skillInfo.text = jo["desc"].ToString();
		int pos = int.Parse(jo ["pos"].ToString ());
		Button skill = skillList[pos-1];
		selectKuang.transform.SetParent (null);
		selectKuang.transform.SetParent (skill.transform);
		selectKuang.transform.localPosition = Vector3.zero;
		hideAllMonsterSelect ();
		showAllHeroSelect ();
	}
	public void quitScene(){
		PoolManager.getInstance ().clearPool ();
		SceneManager.LoadScene ("GameScene");
	}
}
