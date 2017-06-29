using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
	// Use this for initialization
	void Awake () {
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
	public void init(string skillpath){
		isPalyOne = false;
		spriteIndex = 0;
		spriteIndexStart = 0;
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
