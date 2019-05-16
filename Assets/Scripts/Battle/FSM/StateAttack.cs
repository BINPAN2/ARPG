/// <summary>
/// 攻击状态
/// </summary>
public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        entity.curSkillCfg = ResSvc.Instance.GetSkillCfg((int)args[0]);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        entity.ExitCurSkill();
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.entityType == EntityType.Player)
        {
            entity.canRlsSkill = false;
        }
        entity.SkillAttack((int)args[0]);

    }
}

