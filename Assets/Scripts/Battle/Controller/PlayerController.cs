using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色控制器
/// </summary>
public class PlayerController : Controller
{
    public GameObject daggerSkill1fx;
    public GameObject daggerSkill2fx;
    public GameObject daggerSkill3fx;

    public GameObject daggeratk1fx;
    public GameObject daggeratk2fx;
    public GameObject daggeratk3fx;
    public GameObject daggeratk4fx;
    public GameObject daggeratk5fx;


    private Vector3 camOffset;


    private float curBlend;
    private float targetBlend;

    public void Init()
    {
        CamTrans = Camera.main.transform;
        camOffset = transform.position - CamTrans.position;
        if (daggerSkill1fx != null)
        {
            fxDic.Add(daggerSkill1fx.name, daggerSkill1fx);
        }
        if (daggerSkill2fx != null)
        {
            fxDic.Add(daggerSkill2fx.name, daggerSkill2fx);
        }
        if (daggerSkill3fx != null)
        {
            fxDic.Add(daggerSkill3fx.name, daggerSkill3fx);
        }

        if (daggeratk1fx != null)
        {
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
        }
        if (daggeratk2fx != null)
        {
            fxDic.Add(daggeratk2fx.name, daggeratk2fx);
        }
        if (daggeratk3fx != null)
        {
            fxDic.Add(daggeratk3fx.name, daggeratk3fx);
        }
        if (daggeratk4fx != null)
        {
            fxDic.Add(daggeratk4fx.name, daggeratk4fx);
        }
        if (daggeratk5fx != null)
        {
            fxDic.Add(daggeratk5fx.name, daggeratk5fx);
        }
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

        if (isSkillMove)
        {
            SetSkillMove();
            SetCam();
        }

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

    public void SetSkillMove()
    {
        ctrl.Move(transform.forward * skillMoveSpeed * Time.deltaTime);
    }

    public void SetCam()
    {
        if (CamTrans!=null)
        {
            CamTrans.position = transform.position - camOffset;
        }
    }
    
    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    public override void SetFX(string name, float destroy)
    {
        GameObject go;
        if (fxDic.TryGetValue(name,out go))
        {
            go.SetActive(true);
        }

        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            go.SetActive(false);
        }, destroy);
    }
}
