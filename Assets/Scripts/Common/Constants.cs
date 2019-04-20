using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 常量管理类
/// </summary>


public enum TxtColor
{
    Red,
    Blue,
    Yellow,
    Green
}

public enum DamageType
{
    None,
    Ad=1,
    Ap=2,
}

public class Constants {
    //字符颜色
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str,TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
        }
        return result;
    }

    //场景名称
    public const string SceneLogin = "01-Login";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "02-SceneMainCity";

    //音效名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";

    //登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    public const string UIOpenPage = "uiOpenPage";

    //常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string FBItem = "fbitem";

    //默认屏幕分辨率
    public const int ScreenStandardHeight = 1080;
    public const int ScreenStandardWidth = 1920;

    //摇杆滑动标准距离
    public const int ScreenOPDis = 90;

    //角色移动速度
    public const float PlayerMoveSpeed = 5;
    public const float MonsterMoveSpeed = 2.5f;

    //运动平滑
    public const float AccelerSpeed = 5;
    //血条渐变速度
    public const float AccelerHpSpeed = 0.3f; 

    //player action 默认
    public const int DefaultAction = -1;
    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 101;

    public const int DieAniLength = 5000;

    //混合参数
    public const float BlendIdle = 0;
    public const float BlendMove = 1;

    //AutoGuide
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;
    public const int NPCNone = -1;
}
