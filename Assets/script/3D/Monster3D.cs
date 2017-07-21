using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class Monster3D : MonoBehaviour {

	public Animator animator;
	//动画状态信息
	private AnimatorStateInfo mStateInfo;
	public JsonObject data;
	public string type = "Monster3D";
	private const string ActionId="ActionId";
	private const int IdleState=0;
	private const int WalkState=1;
	private const int AttackState=2;
	private const int DeadState=3;
	void Awake () {
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		//this.transform.localPosition = new Vector3 (300F,0F,800F);
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 p3 = this.transform.localPosition;
		//p3.z -= 1;
		//this.transform.localPosition = p3;
	}

	public void init(JsonObject jo){//初始化3D怪物
		//GameObject objPrefab = GameObject.Instantiate((GameObject)Resources.Load(jo["style"].ToString()));
		GameObject objPrefab = GameObject.Instantiate((GameObject)Resources.Load("shibing"));
		objPrefab.transform.SetParent(transform);
		objPrefab.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		animator = GetComponentInChildren<Animator> ();
		if (animator != null) {
			mStateInfo = animator.GetCurrentAnimatorStateInfo (0);
			stop();
		}
	}
	/// <summary>
	/// 动作
	/// </summary>
	public void walk(){
		animator.SetInteger (ActionId,1);
	}
	public void attack(){
		animator.SetInteger (ActionId,2);
	}
	public void stop(){
		animator.SetInteger (ActionId,0);
	}
	public void dead(){
		animator.SetInteger (ActionId,3);
	}
	public void hit(){
		//animator.SetInteger ("ActionId",1);
	}
	/////////////////////////////////////////////////
}
