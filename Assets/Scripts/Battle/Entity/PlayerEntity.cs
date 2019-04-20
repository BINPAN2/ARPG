using UnityEngine;
/// <summary>
/// 玩家逻辑实体
/// </summary>
public class PlayerEntity : EntityBase
{

    public override Vector2 GetDirInput()
    {
       return battleMgr.GetDirInput();
    }
}

