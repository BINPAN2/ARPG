/// <summary>
/// 攻击状态
/// </summary>
public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.DefaultAction);
        entity.canMove = true;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SkillAttack((int)args[0]);

    }
}

