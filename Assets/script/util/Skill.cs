/// <summary>
/// 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;
namespace Game.Util
{
	public class Skill
	{
		public string skillName;//技能名称
		public int skillId;//技能ID
		public string skillInfo;//技能描述
		public string skillType;//技能类型
		public string skillIcon;//技能图标
		public int attackValue;//技能伤害
		public int defenceValue;//技能防御
		public int HPBonus = 0;//血量加成
		public int AttackSpeedBonus = 0;//攻速加成
		public string pinzhi = "";//技能品质
		public int needHeroLevel = 0;
		public List<Buff> BuffDic;

		public Skill(JsonObject jo,JsonObject herodata){
			BuffDic = new List<Buff> ();
			string[] buffs = jo["buff"].ToString ().Split('_');//一个技能可能存在多个Buff效果
			for(int i= 0; i < buffs.Length;i++){
				int buffId = int.Parse(buffs[i]);
				if (DataManager.getInstance ().buffJson.ContainsKey (buffId)) {
					JsonObject buffData = DataManager.getInstance ().buffJson [buffId];
					BuffDic.Add (new Buff(buffData));
					if (buffData != null) {
						skillInfo += buffData ["desc"].ToString ();
					}
				}
			}


		}
		public void getSkillDesc(){//获取技能描述
		}
	}

}
