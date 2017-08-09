using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveMonster : PveEntity {
	void Awake () {
		type = "PveMonster";
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
		pvescene = _pvescene;
		entityData = jo;
		currentHP = maxHP = int.Parse(jo["hp"].ToString());
		//jo = HeroManager.getInstance ().getHeroStaticData (jo);
		style.sprite = Resources.Load(jo["style"].ToString(),typeof(Sprite)) as Sprite;
		style.SetNativeSize ();
		entityName.text = jo["name"].ToString();
		speed = int.Parse (jo ["speed"].ToString ());
		hideSelect ();
	}
	public void onClick(){
		if (isCanHit) {
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
					onHit (pvescene.skillDamage);
					break;
				case 2:
					pvescene.attackAllMonster (pvescene.skillDamage);
					break;
				default:
					break;
				}
			
			} else {//选择的是武器
				onHit (pvescene.equipDamage);
			}
			//pvescene.checkBout ();
			/////////////////////////////////////////////////////////////////////

		}
	}
	public override void active(){
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 50, "easeType", iTween.EaseType.linear, "loopType", "pingPing", "time", 0.3f));
		//iTween.MoveBy(style.gameObject, iTween.Hash("y", 0, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.2f));
		Loom.QueueOnMainThread (() => {
			disActive();
			pvescene.ischeckBout = false;
			pvescene.PveEntityList[0].onHit(100);

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
