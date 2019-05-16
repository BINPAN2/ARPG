using UnityEngine;
/// <summary>
/// 移动状态
/// </summary>
public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Die;
        entity.SetSkillMoveState(false);
        entity.SetDir(Vector2.zero);
        entity.RemoveSkillCB();
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {

        entity.SetAction(Constants.ActionDie);
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            //entity.SetActive(false);
        }, Constants.DieAniLength);
    }
}
