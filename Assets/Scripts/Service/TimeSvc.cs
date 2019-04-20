using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSvc : MonoBehaviour
{
    private static TimeSvc instance = null;
    private TimeSvc() { }
    public static TimeSvc Instance
    {
        get
        {
            return instance;
        }
    }

    private PETimer pt;

    private void Awake()
    {
        instance = this;
    }

    public void InitSvc()
    {
        pt = new PETimer();

        //设置日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimeSvc...");
    }

    private void Update()
    {
        pt.Update();   
    }

    public int AddTimeTask(Action<int>callback,double delay,PETimeUnit timeUnit=PETimeUnit.Millisecond,int count=1 )
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

}
