﻿using UnityEngine;
/// <summary>
/// 待机状态
/// </summary>
public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        entity.skillEndCB = -1;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.nextSkillID !=0)
        {
            entity.Attack(entity.nextSkillID);
        }
        else
        {
            if (entity.entityType == EntityType.Player)
            {
                entity.canRlsSkill = true;
            }
            if (entity.GetDirInput() != Vector2.zero)
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
}

