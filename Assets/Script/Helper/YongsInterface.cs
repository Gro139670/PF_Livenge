using System.Collections;
using UnityEngine;


interface ISingleton<T> : IInitializeable
{
    public static T Instance
    {
        get;
    }
}
public interface IInitializeable
{
    bool Initialize();
}

public interface ISystem : IInitializeable
{

}

public interface ITeamSetting
{
    void SetEnemyID();
    void SetTeamID();
}

public interface ISubUnit
{
    void SetOwner();
}