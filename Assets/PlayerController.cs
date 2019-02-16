using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Transform CamTrans;
    private Vector3 camOffset;
    public Animator anim;
    public CharacterController ctrl;

    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;

        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }

    }


    private bool isMove = false;
    private float curBlend;
    private float targetBlend;

    public void Init()
    {
        CamTrans = Camera.main.transform;
        camOffset = transform.position - CamTrans.position;
    }

    private void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        //Vector2 _dir = new Vector2(h, v).normalized;
        //if (_dir!=Vector2.zero)
        //{
        //    Dir = _dir;
        //    SetBlend(Constants.BlendWalk);
        //}
        //else
        //{
        //    Dir = Vector2.zero;
        //    SetBlend(Constants.BlendIdle);
        //}
        if (curBlend!=targetBlend)
        {
        UpdateMixBlend();

        }
        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }

    }

    public void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1))+CamTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;

    }

    public void SetMove()
    {
        ctrl.Move(transform.forward*Time.deltaTime*Constants.PlayerMoveSpeed);
    }


    public void SetCam()
    {
        if (CamTrans!=null)
        {
            CamTrans.position = transform.position - camOffset;
        }
    }
    
    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(curBlend - targetBlend)<Constants.AccelerSpeed*Time.deltaTime)
        {
            curBlend = targetBlend;
        }
        else if (curBlend>targetBlend)
        {
            curBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            curBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        anim.SetFloat("Blend", curBlend);

    }
}
