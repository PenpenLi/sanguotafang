using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
public class SimpleSkill : MonoBehaviour {
	private Sprite[] sprites;
	private int spriteIndex = 0;//序列帧索引
	private int spriteIndexStart = 0;//序列帧某个动作起始帧
	private int spriteIndexEnd = 0;//序列帧索引某个动作结束帧
	private float spriteChangeSpeed = 0.1f;//序列帧切换速度
	private float spriteChangeTime = 0.0f;//序列帧切换速度
	private Image img;
	private bool isPlay = false;
	//public AudioSource music;
	public bool isPalyOne = false;//技能序列帧是否播放了一轮
	public Rect attackRange;
	private JsonObject skilldata;
	public int skillId;
	public bool isCanAttack = false;
	public int startAttackIndex;
	public int shakeScreenNum = 0;
	public string type = "simple_skill";
	public Monster monster;
	public List<Vector3> pathArr;
	public int waveIndex =0;
	// Use this for initialization
	void Awake () {
		pathArr = new List<Vector3> ();
		PoolManager.getInstance ().initPoolByType (type,this,10);
	}
	void Start () {
		//SkillManager.getInstance().setSkillDemo (this);

	}
	void FixedUpdate(){
		if (isPlay) {
			if (sprites.Length > 1) {
				if (spriteChangeTime > spriteChangeSpeed){
					spriteChangeTime = 0;
					if (spriteIndexEnd > spriteIndex) {
						img.sprite = sprites [spriteIndex];
						img.SetNativeSize ();
						spriteIndex++;
						if(startAttackIndex <= spriteIndex){
							isCanAttack = true;
							if(shakeScreenNum > 0)
								iTween.ShakePosition(ChapterScene._chapterScene.bg.gameObject, new Vector3(5.0f, 5.0f, 0.0f), shakeScreenNum);
						}
					} else {
						isPlay = false;
						spriteIndex = spriteIndexStart;
						PoolManager.getInstance ().addToPool (type,this);
					}
				}
			}
			spriteChangeTime += Time.fixedDeltaTime;

			if (pathArr.Count > waveIndex) {
				transform.localPosition = pathArr [waveIndex];
				waveIndex++;

			} else {
				PoolManager.getInstance ().addToPool (this.type, this);
				if (monster != null) {
					if (monster.currentHP > 0) {
						monster.currentHP -= 5;
						monster.changHp ();
					}
				}
			}
		}

	}
	// Update is called once per frame
	void Update () {
	}
	public void playMusic(int soundId){
		if(skilldata != null && soundId != 0 && isPlay){
			
			AudioManager.instance.Play (soundId);
		}
	}
	public void init(string skillpath,Monster _monster = null){
		pathArr.Clear ();
		waveIndex = 0;
		isPalyOne = false;
		spriteIndex = 0;
		spriteIndexStart = 0;
		monster = null;
		if (_monster != null) {
			monster = _monster;
			Vector3 pos = this.transform.localPosition;
			Vector3 mosterPos = monster.transform.localPosition;
			double distance = DataManager.getInstance().GetDistance (pos, mosterPos);
			int moveNum = (int)(System.Math.Abs(distance)/10);
			float movex = (mosterPos.x - pos.x) / moveNum;
			float movey = (mosterPos.y - pos.y) / moveNum;
			for (int i = 0; i < moveNum; i++) {

				//Array pos2 = new float[3];
				pos.x +=movex;
				pos.y +=movey;
				Vector3 Wave = new Vector3 (pos.x, pos.y, pos.z);
				//Wave.z = oldtWave.z;
				pathArr.Add (Wave);
			}
		}
		// 加载此文件下的所有资源
		sprites = Resources.LoadAll<Sprite>( skillpath);
		img = this.GetComponent<UnityEngine.UI.Image> ();
		img.sprite = sprites [spriteIndex];
		img.SetNativeSize ();
		spriteIndex++;
		spriteIndexEnd = sprites.Length;
		isPlay = true;
		//playMusic (DataManager.getInstance ().getJsonIntValue(skilldata,"music"));
	}
}
