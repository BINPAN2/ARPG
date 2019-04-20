using UnityEngine;
/// <summary>
/// 待机状态
/// </summary>
public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Idle;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.GetDirInput()!= Vector2.zero )
        {
            entity.Move();
            entity.SetDir(entity.GetDirInput());
        }
        else
        {
        entity.SetBlend(Constants.BlendIdle);

        }
    }
}

