using System.Collections;
using UnityEngine;

public class ReaperProjectile : ProjectileStateMachine
{
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    public override bool Initialize()
    {
        AddState<ReaperProjectileStateMove>();
        ChangeState<ReaperProjectileStateMove>();
        return true;
    }

    private class ReaperProjectileStateMove : ProjectileStateMove
    {
        public override string CheckTransition()
        {
            return "ReaperProjectileStateMove";
        }

        public override void Exit()
        {
            base.Exit();
            _OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.GetTakedUnit());
            if(IsWidth(_OwnerInfo.LookDir) == true)
            {
                _OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Left]?.GetTakedUnit());
                _OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Right]?.GetTakedUnit());

            }
            else
            {
                _OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Up]?.GetTakedUnit());
                _OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Down]?.GetTakedUnit());
            }

        }

        public override void FixedLogic()
        {
            DoMove();
        }

        public override bool Initialize()
        {
            return true;
        }

        public override void Logic()
        {
            IsMove();
        }

        
    }
}