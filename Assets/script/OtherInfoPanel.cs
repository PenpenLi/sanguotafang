using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class OtherInfoPanel : MonoBehaviour {
	public static OtherInfoPanel _Current;
	public string type;
	public Transform content;
	public Text heroBB;
	public Text tip;
	public Button weapon;
	public Button Armor;
	public Button Shoes;
	public Button Amulet;
	public Button selectKind;
	//public Button shengxingBtn;
	//public Button shengjiBtn;
	public Text heroAttack;
	//public Text heroAttackRange;
	public Text heroAttackSpeed;
	public Text heroHP;
	public Text heroDefence;
	public Text heroPingFen;
	public Text shengxingNeedInfo;
	public Text shengjiNeedInfo;
	public Image panel;
	public Dictionary<string,Button> equips;

	public HeroStyle skeletonGraphic;
	public RawImage star1;
	public RawImage star2;
	public RawImage star3;
	public RawImage star4;
	private ArrayList starArr;
	public Button skillIcon;
	public Text skillName;
	public Vector3 pos1;
	public Text heroName;
	public ArrayList heroHeadList;
	public int heroSharedId = 0;
	public JsonObject data = null;
	private bool isUpdate = false;
	public int heroId = 0;
	//public Image skillInfoPanel;
	public ArrayList equipedList;

	public JsonObject staticdata;
	public Dictionary<int, JsonObject> HeroArr;
	public Dictionary<int, JsonObject> ItemArr;
	public Dictionary<int, JsonObject> otherPlayerDataCacheArr;
	//public JsonObject data;
	void Awake () {
		//messageArr.Add (Message.HERO_UPDATE);
		//HeroManager.getInstance().heroscene = this;
		equips = new Dictionary<string, Button> ();
		equips ["weapon"] = weapon;
		equips ["armor"] = Armor;
		equips ["shoes"] = Shoes;
		equips ["amulet"] = Amulet;
		equipedList = new ArrayList ();
		starArr = new ArrayList ();
		starArr.Add (star1);
		starArr.Add (star2);
		starArr.Add (star3);
		starArr.Add (star4);
		//content.rect.width = 600;
		otherPlayerDataCacheArr = new Dictionary<int, JsonObject>();

		heroHeadList = new ArrayList();

		skeletonGraphic.Func = new callBackFunc<JsonObject> (OnChangeHero);
		if (_Current == null) {
			_Current = this;
		}
	}
	public void initData (JsonObject playerdata,int heroId = 0) {
		Debug.Log("进入其他玩家信息界面");

		tip.text = playerdata["name"].ToString();
		Dictionary<int,JsonObject> heroarr = HeroManager.getInstance().getHeros();
		int index = 0;
		if (heroId > 0) {
			if (heroarr.ContainsKey (heroId)) {
				JsonObject hero = heroarr [heroId];
				addHero (hero);
				OnChangeHero (hero);
			}
		} else {
			foreach(KeyValuePair<int,JsonObject> kvp in heroarr)
			{

				addHero (kvp.Value);
				if(index == 0){
					OnChangeHero (kvp.Value);
				}
				index++;
			}
			skeletonGraphic.Func = new callBackFunc<JsonObject> (OnChangeHero);
		}

		//HeroStyle.heroarr = HeroManager.getInstance ().getHerosArrayList ();


	}

	// Update is called once per frame
	void Update () {

	}
	public void onClickHeroHead(int heroId){


	}
	public void openBagByType(string type){
		JsonObject equip = BagManager.getInstance ().getEquipByHeroIdAndKind (heroId, type);
		if (equip != null) {//装备了武器
			EquipInfo _equipInfo = (EquipInfo)PoolManager.getInstance().getGameObject(PoolManager.EQUIP_INFO);
			_equipInfo.transform.SetParent (this.transform.parent.transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			_equipInfo.init (equip,0);
		}

	}
	public void onCallBack(JsonObject jo){
		//Debug.Log (jo.ToString());

		string key = BagManager.getInstance().getItemStaticData(jo) ["kind"].ToString ();
		onClickEquip (key);
	}
	public void onClickEquip(string equipType){
		openBagByType (equipType);
		selectKind = equips[equipType];
	}
	public void onClickHead(JsonObject d){
		OnChangeHero(d);
	}
	public void addHero(JsonObject herodata){

		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (staticdata["color"].ToString());
		icon.init (herodata).Func = new callBackFunc<JsonObject> (onClickHead);
		icon.transform.SetParent (content);
		//icon.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		//Button btn = icon.GetComponent<Button> ();
		heroHeadList.Add (icon);

	}
	public void updateHero(JsonObject herodata){
		//if (data == null || herodata.heroId == data.heroId) {
		//data = herodata;
		staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		data =herodata;
		for(int i=0;i < equipedList.Count;i++){
			//Button btn = equips [kvp.Key];
			IconBase icon = (IconBase)equipedList[i];
			if (icon != null) {

				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		equipedList.Clear ();
		//JsonObject staticdata = data ["staticdata"] as JsonObject;
		//JsonObject data = data ["data"] as JsonObject;
		heroId = int.Parse (data ["heroId"].ToString ());
		heroAttack.text = DataManager.getInstance().getHeroDamage(data).ToString();
		//heroAttackRange.text = data["attackRange"].ToString();
		heroAttackSpeed.text = DataManager.getInstance().getHeroAttackSpeed(data).ToString();
		heroHP.text = DataManager.getInstance().getHeroHp(data).ToString();
		heroDefence.text =DataManager.getInstance().getHeroDefence(data).ToString();

		//升星更新
		//updateBtn(shengxingBtn,shengxingNeedInfo,heroSharedId,"starLevel","starLevelUpNeed","levelUp");


		//升级更新
		//updateBtn(shengjiBtn,shengjiNeedInfo,1000,"level","needExpPoint","levelUp");


		heroPingFen.text = (int.Parse(heroAttack.text) * 8 + int.Parse(heroDefence.text) * 5 + (int.Parse(heroAttackSpeed.text) * 6)).ToString();
		heroBB.text = staticdata["desc"].ToString();
		heroName.text = "Lv." + data["level"].ToString() + " " + staticdata["name"].ToString();
		heroName.color = DataManager.getInstance().getColor(staticdata["color"].ToString());
		ArrayList equipArr = BagManager.getInstance ().getEquipByHeroId (heroId);
		for(int i=0;i < equipArr.Count;i++){	
			JsonObject jo = equipArr [i] as JsonObject;
			JsonObject sdjo =  BagManager.getInstance().getItemStaticData (jo);
			string key = sdjo ["kind"].ToString ();
			if (equips.ContainsKey (key)) {
				Button equip = equips [key];

				//equip.sprite = 
				IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (sdjo["color"].ToString());
				icon.init (jo).Func = new callBackFunc<JsonObject> (onCallBack);
				//icon.Func = new callBackFunc<JsonObject> (onCallBack);
				icon.transform.SetParent (equip.transform);
				icon.transform.localPosition = Vector3.zero;
				icon.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
				//iTween.ScaleTo(icon.gameObject, iTween.Hash("y", 0.5f,"x", 0.5f,"z", 0.5f ,"delay", 0.0f,"time",0.5f));
				equip.gameObject.SetActive (true);
				//(icon.GetComponent<Button>()).interactable = false;
				equipedList.Add (icon);
			}
		}
		int starLevel = int.Parse (data ["starLevel"].ToString ());
		for(int i = 0;i < starLevel;i++){
			RawImage star = (RawImage)starArr [i];
			star.gameObject.SetActive (false);
			//Destroy (star);
		}
		//isUpdate = false;
		//}


	}
	public void onClose(){
		for(int i=0;i < heroHeadList.Count;i++){
			//Button btn = equips [kvp.Key];
			IconBase icon = (IconBase)heroHeadList[i];
			if (icon != null) {

				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		heroHeadList.Clear ();
		for(int i=0;i < equipedList.Count;i++){
			//Button btn = equips [kvp.Key];
			IconBase icon = (IconBase)equipedList[i];
			if (icon != null) {

				PoolManager.getInstance ().addToPool (icon.type, icon);
			}
		}
		equipedList.Clear ();
		this.gameObject.SetActive (false);
		this.transform.SetParent (null);
		HeroManager.getInstance().initData(DataManager.playerData);
		BagManager.getInstance().initData(DataManager.playerData);
	}
	public void OnClick(){
		//if (type == 1) {
		//}
		SkillInfo skillinfo = (SkillInfo)PoolManager.getInstance().getGameObject(PoolManager.SKILL_INFO);

		skillinfo.init (data);
		skillinfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
		skillinfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);

	}
	public void OnChangeHero(JsonObject herodata){
		if (herodata == null)
			return;
		selectKind = null;
		staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		data = herodata;
		//heroStyle.sprite = Resources.Load(staticdata["style"].ToString(),typeof(Sprite)) as Sprite;
		//heroStyle.SetNativeSize ();

		//for (int i = 0; i < heroHeadList.Count; i++) {
		//	Button btn2 = (Button)heroHeadList[i];
		//	btn2.interactable = true;
		//}
		//if (skeletonAnimation != null && skeletonAnimation.isActiveAndEnabled) {
		//	skeletonAnimation.transform.parent = null;
		//	skeletonAnimation.gameObject.SetActive (false);
		//}
		skeletonGraphic.init(herodata);

		//skeletonGraphic.startingAnimation = "attack";
		//btn.interactable = false;
		//技能
		JsonObject skilldata = DataManager.getInstance().skillDicJson[int.Parse(staticdata["skill1"].ToString())];
		skillName.text = skilldata["name"].ToString();

		skillIcon.image.sprite = Resources.Load(skilldata["icon"].ToString(),typeof(Sprite)) as Sprite;
		skillIcon.image.SetNativeSize ();

		for(int i = 0;i < starArr.Count;i++){
			RawImage star = (RawImage)starArr [i];
			star.gameObject.SetActive (true);
			//Destroy (star);
		}

		heroSharedId = int.Parse(staticdata["heroSharedId"].ToString());
		updateHero (herodata);
		//yield return isUpdate;

	}
}
