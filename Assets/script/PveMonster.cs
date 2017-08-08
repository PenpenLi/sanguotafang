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
			isCanHit = false;
			pvescene.hideAllMonsterSelect ();
			/////////////////////////////////////////////////////////////////
			JsonObject jo = pvescene.selectSkill;
			if (jo != null && jo.ContainsKey ("skillType")) {//选择的是技能
				int skillType = int.Parse (jo.ContainsKey ("skillType").ToString ());
				if (skillType > 10) {//技能类型是攻击性的
				
				} else {//技能类型是加BUFF的
				
				}
			
			} else {//选择的是武器
				onHit (pvescene.equipDamage);
			}

			/////////////////////////////////////////////////////////////////////

			Loom.QueueOnMainThread (() => {
				pvescene.setNextAttackEntityBySpeed ();
			}, 1.0f);
		}
	}
	public override void active(){
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 50, "easeType", iTween.EaseType.linear, "loopType", "pingPing", "time", 0.3f));
		//iTween.MoveBy(style.gameObject, iTween.Hash("y", 0, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.2f));
		Loom.QueueOnMainThread (() => {
			disActive();
			pvescene.PveEntityList[0].onHit(100);
			Loom.QueueOnMainThread (() => {
				pvescene.setNextAttackEntityBySpeed ();
			}, 1.0f);
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
