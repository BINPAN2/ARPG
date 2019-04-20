﻿/// <summary>
/// 移动状态
/// </summary>
public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Born;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionBorn);
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.DefaultAction);
        }, 500);
    }
}
