using UnityEngine;
using System.Collections;

public class PETools
{
    public static int GetRdInt(int min,int max, System.Random rd=null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }

        return rd.Next(min, max + 1);
    }
}
