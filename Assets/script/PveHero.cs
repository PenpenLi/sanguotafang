using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class PveHero : MonoBehaviour {

	public Image style;
	public Image select;
	public RawImage HP;
	public RawImage MP;
	public Text HPTxt;
	public Text MPTxt;
	private PveScene pvescene;
	private JsonObject heroData;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject jo,PveScene _pvescene){
		pvescene = _pvescene;
		heroData = jo;
		jo = HeroManager.getInstance ().getHeroStaticData (jo);
		style.sprite = Resources.Load("heroHanf/" + jo["style"].ToString(),typeof(Sprite)) as Sprite;
		//style.SetNativeSize ();
	}
	public void onClick(){
		pvescene.setSkillsAndEquip (heroData);
	}
}
