
public class AllianceStateDead : AllianceState
{


    public override string CheckAlliance()
    {
        if (_OwnerInfo?.Status.IsDead == true)
        {
            return typeof(AllianceStateDead).Name;
        }
        return null;
    }

    public override string CheckTransition()
    {
        return null;
    }

    public override void Enter()
    {
        _OwnerInfo.CurrTile.SetTakedUnit(null);
        _Owner.SetActive(false);
    }

    public override void Exit()
    {

    }

    public override void FixedLogic()
    {

    }


    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}