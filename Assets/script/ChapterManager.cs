using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SimpleJson;
using System.Collections.Generic;
using SimpleJson;
public class ChapterManager {
	//public static int chapterId;
	private static ChapterManager _instance;
	private string[][] chapterList;
	private string chapterBg;
	public int loveNum;
	public int chapterLoveNum;
	public JsonObject cd;
	public JsonObject chaperData;
	public int chapterType = 0;
	public int campaignId;
	public int chapterId;
	public int chapterHpAdd1;
	public int chapterHpAdd2;
	public string towerPath;
	public string monsterPath;
	public string mapPath;

	public static ChapterManager getInstance(){//获取单例
		if(_instance == null){
			_instance = new ChapterManager();
		}
		return _instance;
	}

	public ChapterManager(){
		setChapterId (1);
		//chapterList = DataManager.getInstance ().getData ("data/chapter","\r\n");
	}
	public void setChapterId(int _campaignId){
		if (DataManager.getInstance ().campaignDicJson.ContainsKey (_campaignId)) {
			cd = DataManager.getInstance().campaignDicJson [_campaignId];
			loveNum = chapterLoveNum = int.Parse(cd["hp"].ToString());
			campaignId = _campaignId;
			chapterId = int.Parse(cd["chapter"].ToString());

			chaperData = DataManager.getInstance().chapterDicJson [chapterId];
			string[] HpAdd = chaperData["hp_add"].ToString().Split('_');
			chapterHpAdd1 = int.Parse (HpAdd [0]);
			chapterHpAdd2 = int.Parse (HpAdd [1]);

			towerPath = cd["towers"].ToString();
			monsterPath = cd["monsters"].ToString();
			mapPath = cd["mapPath"].ToString();
			//chapterType = type;
		}

	}
	public JsonObject getChapter(){
		if (cd == null)
			setChapterId (1);
		return cd;
	}
	public void GotoChapterScene(int _campaignId)
    {
		Time.timeScale = 1;
		SkillManager.getInstance ().Clear();
        TowerManager.getInstance().ClearTowers();
		MonsterManager.getInstance ().ClearMonster ();
		PoolManager.getInstance ().clearPool ();
		setChapterId (_campaignId);

        SceneManager.LoadScene("scene3D");

		//loveNum = cd.chapterLoveNum;
    }
	public void GotoNextChapterScene()
	{
		Time.timeScale = 1;
		SkillManager.getInstance ().Clear();
		TowerManager.getInstance().ClearTowers();
		MonsterManager.getInstance ().ClearMonster ();
		PoolManager.getInstance ().clearPool ();
		if (cd != null) {
			setChapterId (campaignId + 1);
		} else {
			setChapterId (1);
		}

		SceneManager.LoadScene("ChapterScene");
		//loveNum = cd.chapterLoveNum;
	}
	public void changeLoveNum(int num){
		loveNum -= num;
		ChapterScene._chapterScene.chapterQiZhi.currentHP -= num;
		ChapterScene._chapterScene.chapterQiZhi.changHp ();
	}
	public int getStar(){//星数评分
		int star = 0;
		if (loveNum == chapterLoveNum) {
			star =  3;
		} else if (loveNum > (chapterLoveNum / 2)) {
			star = 2;
		} else {
			star = 1;
		}
		//cd.star = star;
		return star;
	}
	public GameObject clone(GameObject obj,Transform _parent){
		return GameObject.Instantiate (obj,obj.transform.localPosition,obj.transform.localRotation,_parent);
	}
}
