using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum OperationCode : byte
    {
        Login,Signin,Chat,Show,Move,BattleFiled,Matching,
    }
    public enum ParaCode : byte
    {
        UserName,Password,Chat,Show,Move,ParaType,BF_Join,BF_Move,BF_Att,BF_Hurt,Matching_Start,Matching_confirm,BF_Ending,
    }
    public enum ReturnCode: short
    {
        Success,Failed,
    }
    public enum AttackType : short
    {
        Normal = 0,
        Skill1 = 1
    }

}
