using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 常量管理类
/// </summary>

public class Constants {
    //场景名称
    public const string SceneLogin = "01-Login";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "02-SceneMainCity";

    //音效名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";

    //登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";

    //常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";

    //默认屏幕分辨率
    public const int ScreenStandardHeight = 1080;
    public const int ScreenStandardWidth = 1920;

    //摇杆滑动标准距离
    public const int ScreenOPDis = 90;

    //角色移动速度
    public const float PlayerMoveSpeed = 8;
    public const float MonsterMoveSpeed = 4;

    //运动平滑
    public const float AccelerSpeed = 5;

    //混合参数
    public const float BlendIdle = 0;
    public const float BlendWalk = 1;
}
