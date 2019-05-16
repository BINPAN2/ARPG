
using UnityEngine;
/// <summary>
/// 怪物控制器
/// </summary>
public class MonsterController:Controller
{
    public void Init()
    {

    }

    private void Update()
    {
        if (isMove)
        {
            SetDir();
            SetMove();
        }
    }

    public void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }


    public void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
    }
}

