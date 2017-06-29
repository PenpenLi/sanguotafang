using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJson;
public class Skill : MonoBehaviour {
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
	public int shakeScreenNum;
	public string type = "skill";
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
						spriteIndex = spriteIndexStart;
						isPalyOne = true;
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
	public void init(JsonObject skilldata){
		isPalyOne = false;
		spriteIndex = 0;
		spriteIndexStart = 0;
		isCanAttack = false;
		int _skillid = DataManager.getInstance ().getJsonIntValue (skilldata, "id");
		if (skillId == _skillid) {
			img.sprite = sprites [spriteIndex];
			spriteIndex++;
			img.SetNativeSize ();
			gameObject.SetActive (true);
			isPlay = true;
			playMusic (DataManager.getInstance ().getJsonIntValue(skilldata,"music"));
			return;
		}
			
		this.skilldata = skilldata;
		startAttackIndex = DataManager.getInstance ().getJsonIntValue (skilldata, "startAttackIndex");
		shakeScreenNum = DataManager.getInstance ().getJsonIntValue (skilldata, "shakeScreen");
		skillId = _skillid;
		img = this.GetComponent<UnityEngine.UI.Image> ();
		// 加载此文件下的所有资源
		sprites = Resources.LoadAll<Sprite>( DataManager.getInstance ().getJsonStringValue(skilldata,"effectName"));
		img.sprite = sprites [spriteIndex];
		img.SetNativeSize ();
		img.rectTransform.pivot = new Vector2 (0.5f,DataManager.getInstance ().getJsonFloatValue(skilldata,"priotY"));
		attackRange = img.rectTransform.rect;
		int _re = DataManager.getInstance ().getJsonIntValue (skilldata, "attackRange");
		attackRange.width = _re;
		attackRange.height = _re;
		//RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		//rectTransform.localPosition = new Vector3(200, 60, 0);
		//rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, skilldata.attackRange);
		//transform.localScale = new Vector3 (skilldata.effectScale,skilldata.effectScale,skilldata.effectScale);
		spriteIndex++;
		gameObject.SetActive (true);
		spriteIndexEnd = sprites.Length;
		//music.pl
		//music.clip = (AudioClip)Resources.Load(DataManager.getInstance ().getJsonStringValue(skilldata,"music"), typeof(AudioClip));//调用Resources方法加载AudioClip资源
		//music.loop = true;

		//iTween.ShakePosition(ChapterScene._chapterScene.bg.gameObject, new Vector3(5.0f, 5.0f, 0.0f), 1);
		isPlay = true;
		playMusic (DataManager.getInstance ().getJsonIntValue(skilldata,"music"));
	}
}
