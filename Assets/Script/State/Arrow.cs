using System.Collections;
using UnityEngine;


public class Arrow : StateMachine
{
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    public override bool Initialize()
    {
        AddState<ArrowStateMove>();
        ChangeState<ArrowStateMove>();

        return true;
    }

    private class ArrowStateMove : StateMove
    {

        public override void Exit()
        {
            base.Exit();
            _OwnerInfo.AttackEnemy(_OwnerInfo.AttackUnit);
            
        }

        public override void Enter()
        {
            base.Enter();
            

        }
        public override string CheckTransition()
        {
            return "ArrowStateMove";
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

        protected override void IsMove()
        {
            if (_IsMove == true)
                return;

            if (_OwnerInfo.AttackUnit != null)
            {
                _OwnerInfo.CurrTile = _OwnerInfo.AttackUnit.CurrTile;
                _Owner.transform.SetParent(_OwnerInfo.CurrTile.gameObject.transform);
                _PrevPosition = _Owner.transform.localPosition;
                _Owner.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(_PrevPosition.y, _PrevPosition.x) * 180 / 3.14f);
                _IsMove = true;
            }
            else
            {
                IsStateFinish = true;
                _Owner.SetActive(false);
            }
        }
    }
}

