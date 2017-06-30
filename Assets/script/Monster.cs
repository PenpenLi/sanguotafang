﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJson;
public class Monster : MonoBehaviour {
	public RawImage HP;
	public RawImage HPbg;
	public bool isDead = false;
	public int currentHP = 1000;
	public int beforChangeHP = 1000;
	public int maxHP = 1000;
	public float speed = 0.0f;
	public string[][] waveArr;
	private Vector3 currentWave = new Vector3 (0,0,0);
	private Vector3 oldtWave = new Vector3 (0,0,0);
	private bool isWalk = false;
	private int waveIndex = 0;
	private int moveNum;
	private float movex;
	private float movey;
	private float dt;
	private float movedt = 0.03f;
	public int siblingIndex;
	public Text yIndex;
	private JsonObject _monsterData;
	private Sprite[] sprites;
	private int spriteIndex = 0;//序列帧索引
	private int spriteIndexStart = 0;//序列帧某个动作起始帧
	private int spriteIndexEnd = 0;//序列帧索引某个动作结束帧
	private float spriteChangeSpeed = 0.1f;//序列帧切换速度
	private float spriteChangeTime = 0.0f;//序列帧切换速度
	private int currentDir = 0;
	private Image body;
	//public AudioSource music;
	public int currentState = 0;//怪物当前状态，0为正常，1为击晕
	private float stunTime;//被击晕的时间点
	private float stunDuration;//持续时间

	private float converseTime;//逆行的时间点
	private float converseDuration;//持续时间

	private float speedSlowTime;//减速开始的时间点
	private float speedSlowDuration;//持续时间
	private Skill stunEffect;

	private ArrayList StateArr;//状态数组
	public ArrayList pathArr;
	private int pathNum;
	private bool isConverse = false;
	public defence attackedDefence;
	public int attackRange = 200;//攻击范围
	private float attackPingLv = 1.0f;//攻击平率
	private float frontTime = 0.0f;
	public string type = "monster";
	void Awake () {
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		StateArr = new ArrayList ();
		siblingIndex = transform.GetSiblingIndex();
		yIndex.text = siblingIndex.ToString ();
		//MonsterManager.getInstance().setMonsterDemo(this);
		yIndex.gameObject.SetActive (false);
		HPbg.gameObject.SetActive (false);
	}
	void OnCollisionEnter2D(Collision2D collision){
		Debug.Log( "Collided with someone" );
	}
	public int getConverseDir(int dir){
		if (dir == 1)
			return 4;
		if (dir == 2)
			return 3;
		if (dir == 3)
			return 2;
		if (dir == 4)
			return 1;
		return dir;
	}
	void FixedUpdate(){
		if (isWalk) {
			if (dt >= movedt) {
				dt = dt - movedt;
				/**dt = dt - movedt;
			if (moveNum <= 0) {
				oldtWave = currentWave;
				setWaveData ();    
			}
			oldtWave.x += movex;
			oldtWave.y += movey;
			transform.localPosition = oldtWave;
			moveNum--;
			yIndex.text = transform.GetSiblingIndex ().ToString ();**/
				if (pathNum > waveIndex) {
					Vector3 _ve = (Vector3)pathArr [waveIndex];
					transform.localPosition = _ve;
					int dir = (int)_ve.z;
					if (isConverse) {
						waveIndex--;
						if (waveIndex < 0)
							waveIndex = 0;
						dir = getConverseDir (dir);
						
					} else {
						waveIndex++;
					}
					if (currentDir != dir) {
						currentDir = dir;
						setWalkDir ();
					}
				} else {
					ChapterManager.getInstance ().changeLoveNum (DataManager.getInstance ().getJsonIntValue (_monsterData, "attack"));
					onDead ();
				}
			}
			//Debug.Log (Time.deltaTime);
			dt = dt + Time.deltaTime;



			//}

		} else {
			if (attackedDefence != null) {
				if (Time.time - frontTime > attackPingLv) {
					frontTime = Time.time;
						onAttack ();
	
				}
			}
		}

		if (currentState > 0) {
			//for (int i = 0; i < StateArr.Count; i++) {
				//ArrayList arr = (ArrayList)StateArr [i];
				//int state = (int)arr [0];
				//stunTime = (float)arr [1];
				//stunDuration = (float)arr [2];

				if(1 == currentState){//被击晕
					float _t = Time.time - stunTime;
					if(stunDuration < _t){
						//stunEffect = (Skill)arr [3];
						stunEffect.gameObject.SetActive (false);
						stunEffect.transform.SetParent (null);
						SkillManager.getInstance ().addToCache (stunEffect);
						currentState = 0;
						isWalk = true;
					}
				}
				if(2 == currentState){//逆流
					float _t = Time.time - converseTime;
					if(converseDuration < _t){
						isConverse = false;
						//stunEffect = (Skill)arr [3];
						//stunEffect.gameObject.SetActive (false);
						//stunEffect.transform.SetParent (null);
						//SkillManager.getInstance ().addToCache (stunEffect);
						currentState = 0;
						//isWalk = true;
					}
				}
				if(3 == currentState){//减速
					float _t = Time.time - speedSlowTime;
					if(speedSlowDuration < _t){
						currentState = 0;
						movedt = 0.02f;
					}
				}

			//}

		}

	}
	// Update is called once per frame
	void Update () {
		if (sprites.Length > 1) {
			if (spriteChangeTime > spriteChangeSpeed){
				spriteChangeTime = 0;
				if (spriteIndexEnd > spriteIndex) {
					body.sprite = sprites [spriteIndex];
					body.SetNativeSize ();
					spriteIndex++;
				} else {
					spriteIndex = spriteIndexStart;
				}
			}
		}
		spriteChangeTime += Time.fixedDeltaTime;
	}
	public void setWalkDir(){
		if (currentDir == 1) {
			spriteIndexStart = 0;
			spriteIndexEnd = 4;
		} else if (currentDir == 2) {
			spriteIndexStart = 4;
			spriteIndexEnd = 8;
		} else if (currentDir == 3) {
			spriteIndexStart = 8;
			spriteIndexEnd = 12;
		} else {
			spriteIndexStart = 12;
			spriteIndexEnd = 14;
		}

		spriteIndex = spriteIndexStart;
	}
	public void init(string[] monsterInfo,int _siblingIndex){
		
		_monsterData = DataManager.getInstance().monsterDicJson[int.Parse(monsterInfo[1])];
		// 加载此文件下的所有资源
		sprites = Resources.LoadAll<Sprite>(DataManager.getInstance().getJsonStringValue(_monsterData,"style"));
		if (sprites.Length > 0) {
			
		}
		//Object spri = Resources.Load (_monsterData.monsterStyle);
		body = this.GetComponent<UnityEngine.UI.Image> ();
		body.sprite = sprites[spriteIndex];
		body.SetNativeSize ();
		int monsterBaseHp = DataManager.getInstance ().getJsonIntValue (_monsterData, "hp");
		if(ChapterManager.getInstance().chapterType == 0){
			currentHP = beforChangeHP = maxHP = monsterBaseHp + ChapterManager.getInstance().chapterHpAdd1;
		}else{
			currentHP = beforChangeHP = maxHP = monsterBaseHp + ChapterManager.getInstance().chapterHpAdd2;
		}

		pathArr = DataManager.getInstance ().initChapterWaveData (monsterInfo[3]);
		speed = DataManager.getInstance ().getJsonIntValue (_monsterData, "speed");
		/**waveArr = DataManager.getInstance ().getData (monsterInfo[3], "\r\n");


		if (waveArr.Length > 0) {
			oldtWave.x = int.Parse(waveArr[waveIndex][0]);
			oldtWave.y = int.Parse(waveArr[waveIndex][1]);
			transform.localPosition = oldtWave;
		}**/
		pathNum = pathArr.Count;
		if (pathNum > 0) {
			transform.localPosition = (Vector3)pathArr [waveIndex];
			waveIndex++;
		}

		//transform.SetSiblingIndex(-10);

		isDead = false;
	}
	public void startWalk(){
		
		//setWaveData ();
		isWalk = true;
	}
	public void setWaveData(){
		if (waveArr.Length > waveIndex) {
			currentWave.x = int.Parse (waveArr [waveIndex] [0]);
			currentWave.y = int.Parse (waveArr [waveIndex] [1]);
			currentDir = int.Parse (waveArr [waveIndex][2]);
			double distance = GetDistance (currentWave, oldtWave);
			moveNum = (int)System.Math.Abs (distance / speed);
			movex = (currentWave.x - oldtWave.x) / moveNum;
			movey = (currentWave.y - oldtWave.y) / moveNum;
			waveIndex++;
			setWalkDir ();
		} else {
			ChapterManager.getInstance ().changeLoveNum (DataManager.getInstance ().getJsonIntValue (_monsterData, "attack"));
			onDead ();
		}
	}

	public  double GetDistance(Vector3 startPoint, Vector3 endPoint)
	{
		float x = System.Math.Abs(endPoint.x - startPoint.x);
		float y = System.Math.Abs(endPoint.y - startPoint.y);
		return System.Math.Sqrt(x * x + y * y);
	} 

	public void changHp(){//受到伤害
		if (!HPbg.isActiveAndEnabled) {
			HPbg.gameObject.SetActive (true);
		}
		//music.Play ();
		int damage = beforChangeHP- currentHP;//用于显示伤害数字
		ChapterScene._chapterScene.playJumpHp(damage,new Vector3(transform.position.x,transform.position.y + 40,transform.position.z));

		beforChangeHP = currentHP;
		currentHP = currentHP < 0 ? 0 : currentHP;
		float xscale = (float)currentHP / (float)maxHP;
		HP.transform.localScale = new Vector3 (xscale,1,1);
		if (currentHP == 0) {
			onDead ();
		}
		
	}
	public void stun(float t){//击晕
		//ArrayList arr = new ArrayList();
		isWalk = false;
		if (currentState == 1) {
			stunDuration += t;
		} else {
			stunTime = Time.time;
			stunDuration = t;
		}
		//arr.Add (1);
		//arr.Add (Time.time);
		//arr.Add (stunDuration);
		currentState = 1;
		stunEffect = SkillManager.getInstance ().getSkillDemo (1000);
		JsonObject sd = DataManager.getInstance ().skillDicJson [1000];

		stunEffect.init (sd);
		stunEffect.transform.SetParent (transform);
		stunEffect.transform.localPosition = new Vector3 (0,32,0);
		stunEffect.gameObject.SetActive (true);
		//arr.Add (stunEffect);
		//StateArr.Add (arr);
	}
	public void speedReduction(float t){//减速
		

		//if (currentState == 3) {
		//	return;
		//} else {
			speedSlowTime = Time.time;
			speedSlowDuration = t;
		//}
		movedt = 0.04f;
		currentState =3;
		
	}
	public void attackByTowerSkill(Tower tower){
		JsonObject skildata = DataManager.getInstance ().skillDicJson [tower.skillId];
		int attackDamage = DataManager.getInstance ().getJsonIntValue (skildata,"attackDamage");
		int stateDuration = DataManager.getInstance ().getJsonIntValue (skildata,"stateDuration");
		if (attackDamage > 0) {
			int damage = attackDamage + tower.attackDamage;//目前技能总伤害为：技能初始伤害+塔的伤害;
			ChapterScene._chapterScene.changeSkillDamages(damage);
			currentHP -= damage;
			changHp ();
		}

		if (currentHP <= 0) {
			isDead = true;
		} else {
			
			if (stateDuration > 3000) {//逆行
				int t = stateDuration - 3000;
				converse (t);
								
			}else if(stateDuration > 2000){//减速
				int t = stateDuration - 2000;
				speedReduction (t);
			}else if(stateDuration > 1000){//眩晕
				int t = stateDuration - 1000;
				float chance = Random.Range (0.0f,10000.0f);
				float d = 10000*t;//击晕的伪概率
				if(chance <= d){
					stun (t);
				}
			}

		}
	}
	public void converse(float t){
		//ArrayList arr = new ArrayList();
		//isWalk = false;
		if (currentState == 2) {
			converseDuration += t;
		} else {
			converseTime = Time.time;
			converseDuration = t;
		}
		//arr.Add (2);
		//arr.Add (Time.time);
		//arr.Add (stunDuration);
		currentState = 2;
		isConverse = true;
		/**stunEffect = SkillManager.getInstance ().getSkillDemo ();
		skillData sd = DataManager.getInstance ().skillDic [1000];

		stunEffect.init (sd);
		stunEffect.transform.SetParent (transform);
		stunEffect.transform.localPosition = new Vector3 (0,32,0);
		arr.Add (stunEffect);**/
		//StateArr.Add (arr);
	}
	public void onAttacked(){

	}
	public void onAttack(){//怪物攻击防御塔
		if (attackedDefence != null) {
			if (attackedDefence.hit (1)) {
				isWalk = true;
			}
		}
	}
	public void onStopToAttack(defence df){
		isWalk = false;
		attackedDefence = df;
		//df.hit (1);
	}
	public void onDead(){
		currentHP = 0;
		isDead = true;
		isWalk = false;
		AudioManager.instance.Play (9);

		//gameObject.SetActive (false);

		PoolManager.getInstance ().addToPool (this.type,this);
		//SimpleSkill _simpleSkill = (SimpleSkill)PoolManager.getInstance ().getGameObject ("simple_skill");
		//_simpleSkill.init ("skill2/dead_red");
		//_simpleSkill.transform.SetParent (this.transform.parent.transform);
		//_simpleSkill.transform.localPosition = this.transform.localPosition;

		//transform.SetParent (null);
		//MonsterManager.getInstance ().removeMonster (this);
		//MonsterManager.getInstance ().addToCachePool (this);//放进回收池


	}

}
