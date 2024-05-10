using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public override bool Initialize()
    {
        return true;
    }

    public bool SetTime(ref float time, float speed)
    {
        bool result = false;
        time += Time.deltaTime;
        if (time >= speed)
        {
            result = true;
            time = speed;
        }
        return result;
    }
}