using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect : MonoBehaviour {
	private Sprite[] sprites;
	private int spriteIndex = 0;//序列帧索引
	private int spriteIndexStart = 0;//序列帧某个动作起始帧
	private int spriteIndexEnd = 0;//序列帧索引某个动作结束帧
	private float spriteChangeSpeed = 0.05f;//序列帧切换速度
	private float spriteChangeTime = 0.0f;//序列帧切换速度
	private Image img;
	private bool isPlay = false;
	public int startAttackIndex;
	//public AudioSource music;
	public bool isPalyOne = false;//技能序列帧是否播放了一轮
	public string type = "Effect";

	void Awake () {
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isPlay) {
			if (sprites.Length > 1) {
				if (spriteChangeTime > spriteChangeSpeed){
					spriteChangeTime = 0;
					if (spriteIndexEnd > spriteIndex) {
						img.sprite = sprites [spriteIndex];
						img.SetNativeSize ();
						spriteIndex++;
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
	public void init(string effectPath){
		// 加载此文件下的所有资源
		sprites = Resources.LoadAll<Sprite>( effectPath);
		img = this.GetComponent<UnityEngine.UI.Image> ();
		img.sprite = sprites [spriteIndex];
		img.SetNativeSize ();
		spriteIndex++;
		spriteIndexEnd = sprites.Length;
		isPlay = true;
		//playMusic (DataManager.getInstance ().getJsonIntValue(skilldata,"music"));
	}
}
