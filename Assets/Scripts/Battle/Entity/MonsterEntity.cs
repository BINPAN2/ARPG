using UnityEngine;
/// <summary>
/// 怪物逻辑实体
/// </summary>
public class MonsterEntity : EntityBase
{
    public MonsterData md;

    public override void SetBattleProps(BattleProps props)
    {
        int level = md.level;
        BattleProps p = new BattleProps
        {
            hp = level*props.hp,
            ad = level*props.ad,
            ap = level*props.ap,
            addef = level*props.addef,
            apdef = level*props.apdef,
            dodge = level*props.dodge,
            critical = level * props.critical,
            pierce = level*props.pierce,
        };

        Props = p;
        Hp = p.hp;
    }
}
