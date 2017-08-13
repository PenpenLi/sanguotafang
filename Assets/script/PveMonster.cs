using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveMonster : PveEntity {
	void Awake () {
		type = "PveMonster";
		//PropertyDic = new Dictionary<Property, int> ();
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		//Random.Range (0, 10) * 0.1)
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		showSelect ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject jo,PveScene _pvescene){
		resetPropert ();
		pvescene = _pvescene;
		entityData = jo;
		PropertyDic[Property.HP] = PropertyDic[Property.MAXHP] = int.Parse(jo["hp"].ToString());
		//jo = HeroManager.getInstance ().getHeroStaticData (jo);
		style.sprite = Resources.Load(jo["style"].ToString(),typeof(Sprite)) as Sprite;
		style.SetNativeSize ();
		entityName.text = jo["name"].ToString();
		entityName.color = DataManager.getInstance ().getColor (jo["color"].ToString());
		PropertyDic[Property.SPEED] = int.Parse (jo ["speed"].ToString ());
		PropertyDic[Property.AP] = int.Parse (jo ["attack"].ToString ());
		hideSelect ();
	}
	public void onClick(){
		base.onClick ();
		/**if (isCanHit) {
			pvescene.ischeckBout = false;
			isCanHit = false;
			pvescene.hideAllMonsterSelect ();
			/////////////////////////////////////////////////////////////////
			JsonObject jo = pvescene.pveHero.selectSkill;
			if (jo != null && jo.ContainsKey ("skillType")) {//选择的是技能
				int skillType = int.Parse (jo["skillType"].ToString ());
				//pvescene.pveHero.updateSkillTurn ();
				switch (skillType) {
				case 1:
					onHit (pvescene.pveHero.getSelectedSkillDamage(true));
					break;
				case 2:
					pvescene.attackAllMonster (pvescene.pveHero.getSelectedSkillDamage(true));
					break;
				default:
					break;
				}
			
			} else {//选择的是武器
				onHit (pvescene.pveHero.getEquipDamage(true));
			}
			//pvescene.checkBout ();
			/////////////////////////////////////////////////////////////////////

		}**/
	}
	public override void active(){
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 50, "easeType", iTween.EaseType.linear, "loopType", "pingPing", "time", 0.3f));
		//iTween.MoveBy(style.gameObject, iTween.Hash("y", 0, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.2f));
		Loom.QueueOnMainThread (() => {
			disActive();
			pvescene.ischeckBout = false;
			pvescene.getHighestThreatHero().onHit(PropertyDic[Property.AP]);

		}, 0.2f);
		//iTween.MoveBy(style.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		//pvescene.setSkillsAndEquip (entityData);
	}
	public override void disActive(){
		iTween.Stop (style.gameObject);
		style.transform.localPosition = Vector3.zero;
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));

	}
}
