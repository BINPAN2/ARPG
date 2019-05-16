using UnityEngine;
/// <summary>
/// 移动状态
/// </summary>
public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Hit;

        entity.SetSkillMoveState(false);
        entity.SetDir(Vector2.zero);

        entity.RemoveSkillCB();
    }

    public void Exit(EntityBase entity, params object[] args)
    {

    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.entityType == EntityType.Player)
        {
            entity.canRlsSkill = false;
            AudioSource playerAudio = entity.GetAudioSource();
            AudioSvc.Instance.PlayPlayerAudio(Constants.AssassinHit, playerAudio);
        }
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.DefaultAction);
            entity.Idle();
        },(int)(GetHitAniLength(entity)*1000));
    }

    private float GetHitAniLength(EntityBase entity)
    {
        AnimationClip[] clips = entity.GetAniClips();
        for (int i = 0; i < clips.Length; i++)
        {
            string clipName = clips[i].name;
            if (clipName.Contains("hit")||
                clipName.Contains("Hit")||
                clipName.Contains("HIT"))
            {
                return clips[i].length;
            }
        }

        return 1;
    }
}
