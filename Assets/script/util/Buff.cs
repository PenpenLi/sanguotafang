using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ////////////////////////////////
/// 目标或自身的某项属性在x回合内减少或增加xx%或具体的数值
/// </summary>
public class Buff {
	public PveEntity pveEntity;
	public int targetEntity = 0;
	public int turn = 0;//Buff持续的回合数
	private Property changeEntityProperty = Property.MAXINDEX;//BUff修改Entity的属性类型
	/// <summary>
	/// 修改属性的方式：
	/// 1.一次性
	/// 2.每回合都修改（比如持续3回合流血324）
	/// 3.回合内增加或减少某项属性值，Buff消失后还原属性值
	/// </summary>
	private int changeType = 0;
	private int valueType = 0;//0.增加到基础数值（比如回复HP），1.某项属性的加成数值（比如x回合内攻击增加300）2.某项属性的百分比加成
	private int addType = 0;//1数值 2百分比
	private int changeValue = 0;//改变值，可以是具体的数值，也可以是某项属性的百分比,通过valueType累判断
								
	// Use this for initialization
	public Buff(string[] str){
		if (str.Length > 1) {
			targetEntity = int.Parse (str [0]);
			changeEntityProperty = (Property)int.Parse (str [1]);
			turn = int.Parse (str [2]);
			changeType = int.Parse (str [3]);
			valueType = int.Parse (str [4]);
			addType = int.Parse (str [5]);
			changeValue = int.Parse (str [6]);
		}

	}
	public void init(PveEntity _pveEntity){
		pveEntity = _pveEntity;
		changeByBuff ();
	}
	public void updateBuff(){
		turn--;
		//change ();
		if(changeType == 2){//每回合都修改值，如掉血
			change ();
		}
		if (turn <= 0) {
			removeBuff ();
		}
	}
	public void removeBuff(){
		//pveEntity.removeBuff ();
		if (changeType == 3) {//buff消失需要还原
			change (false);
		}
	}
	private void changeByBuff(){
			change ();
	}
	private void change(bool addOrRemove = true){//true为增加Buff flase为Buff结束后移除Buff
		int _changeValue = addOrRemove?changeValue:-changeValue;
		if (changeEntityProperty != Property.MAXINDEX) {
			pveEntity.PropertyAllDic [valueType] [changeEntityProperty] += _changeValue;
			pveEntity.showPropertyChange (changeEntityProperty, _changeValue, valueType);
		}
	}
}
