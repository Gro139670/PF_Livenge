using System.Collections;
using UnityEngine;

public class BallistarProjectile : StateMachine
{
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        var state = GetState<BallistarProjectileStateMove>();
        if(state != null)
        {
            state.SetAttackUnitNum();
        }
    }
    public override bool Initialize()
    {
        AddState<BallistarProjectileStateMove>();
        ChangeState<BallistarProjectileStateMove>();
        return true;
    }

    private class BallistarProjectileStateMove : ProjectileStateMove
    {
        private int _AttackUnitNum = 0;
        public override string CheckTransition()
        {
            return "BallistarProjectileStateMove";
        }

        public override void Exit()
        {
            if(_OwnerInfo.AttackEnemy(_OwnerInfo.CurrTile.GetTakedUnit(),_OwnerInfo.Status.Damage / (1 + _AttackUnitNum)) > 0)
            {
                _AttackUnitNum++;
            }

            if(_AttackUnitNum > 4)
            {
                _Owner.SetActive(false);
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
        public void SetAttackUnitNum()
        {
            _AttackUnitNum = 0;
        }

    }
}