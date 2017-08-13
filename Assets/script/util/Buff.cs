using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ////////////////////////////////
/// 目标或自身的某项属性在x回合内减少或增加xx%或具体的数值
/// </summary>
public class Buff : MonoBehaviour {
	public PveEntity pveEntity;
	private int turn = 0;//Buff持续的回合数
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(PveEntity _pveEntity){
		pveEntity = _pveEntity;
	}
	public void addBuff(){
		
	}

	public void removeBuff(){
	}
	private void changeByBuff(){
		
	}
}
