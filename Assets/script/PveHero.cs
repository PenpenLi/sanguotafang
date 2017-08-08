using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveHero : PveEntity {

	// Use this for initialization
	void Start () {
		
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

		//select.gameObject.SetActive (false);
		//style.SetNativeSize ();
	}
	public void onClick(){
		
	}
	public override void active(){
		iTween.MoveBy(style.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		pvescene.setSkillsAndEquip (entityData);
		pvescene.showAllMonsterSelect ();
	}
	public override void disActive(){
		iTween.Stop (style.gameObject);
		style.transform.localPosition = Vector3.zero;

	}
}
