using UnityEngine;
/// <summary>
/// 玩家逻辑实体
/// </summary>
public class PlayerEntity : EntityBase
{
    public PlayerEntity()
    {
        entityType = EntityType.Player;
    }

    public override Vector2 GetDirInput()
    {
       return battleMgr.GetDirInput();
    }

    public override void SetHpVal(int oldVal, int newVal)
    {
        BattleSys.Instance.playerCtrlWnd.SetPlayerHpBarVal(newVal);
    }

    public override void SetDodge()
    {
        GameRoot.Instance.dynamicWnd.SetPlayerDodge();
    }
}

