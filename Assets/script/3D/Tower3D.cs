using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class Tower3D : MonoBehaviour {

	public Image range;
	public RawImage HP;//防御盾的血量
	public int currentHP = 100;//当前血量
	public int maxHP = 100;//最大血量
	public int step =0;
	public float attackPingLv = 0.3f;//攻击平率
	private float frontTime = 0.0f;
	public bool isCanAttackMonster = false;
	public int attacKType = 0;//攻击方式 0为进展,1为远程，远程涉及到子弹的移动
	public bool isAttackAny = false;//攻击单个还是多个
	//public int attacKType = 0;//攻击方式 0为进展,1为远程，远程涉及到子弹的移动
	public int damage = 5;
	public Image zidan;
	// Use this for initialization
	void Start () {

	}
	void FixedUpdate(){
		if (Time.time - frontTime > attackPingLv) {
			frontTime = Time.time;
			onAttack ();

		}
	}
	// Update is called once per frame
	void Update () {
		/**if (step > 50) {
			step = 0;
			List<Monster> list = MonsterManager.getInstance ().getMonstersByRect (range);
			for (int i = 0; i < list.Count; i++) {
				Monster monster = list [i];
				monster.onStopToAttack (this);
			}
		}
		step++;
		**/


	}
	public void init(JsonObject jo){

	}
	public bool hit(int damage){
		if (currentHP == 0) {
			return true;
		}
		currentHP -= damage;
		currentHP = currentHP < 0 ? 0 : currentHP;
		float xscale = (float)currentHP / (float)maxHP;
		HP.transform.localScale = new Vector3 (xscale,1,1);
		if (currentHP == 0) {
			onDead ();
			return true;
		}
		return false;
	}
	public void onAttack(){//怪物攻击防御塔
		List<Monster> list = MonsterManager.getInstance ().getMonstersByRect (range,isAttackAny);
		for (int i = 0; i < list.Count; i++) {
			Monster monster = list [i];
			if (attacKType == 0) {//近战
				//monster.onStopToAttack (this);
				if (isCanAttackMonster) {
					if (monster.currentHP > 0) {
						monster.currentHP -= this.damage;
						monster.changHp ();
					}
				}
			} else {
				SimpleSkill _simpleSkill = (SimpleSkill)PoolManager.getInstance ().getGameObject ("simple_skill");

				_simpleSkill.transform.SetParent (this.transform.parent.transform);
				_simpleSkill.transform.localPosition = this.transform.localPosition;
				_simpleSkill.init ("skill/zidan",monster);
				//break;
			}
		}

	}
	public void onDead(){
		this.gameObject.SetActive (false);
	}
}
