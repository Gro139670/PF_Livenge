using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
namespace Team
{
    public class Summoner : StateMachine
    {

        public override bool Initialize()
        {
            AddState<SummonerStateIdle>();
            AddState<SummonerStateAttack>();
            AddState<CommonStateMove>();
            return true;
        }

        private class SummonerStateIdle : State
        {
            bool _IsMoveFinish = false;
            bool _IsCanSummon = false;
            public override string CheckTransition()
            {
                if(_IsMoveFinish == false)
                {
                    return "CommonStateMove";
                }
                if(_IsCanSummon == false)
                {
                    return "SummonerStateIdle";
                }
                return "SummonerStateAttack";
            }

            public override void Enter()
            {
                _IsMoveFinish = false;
                _IsCanSummon = false;
            }

            public override void Exit()
            {
            }

            public override void FixedLogic()
            {
                // 하드코딩..
                if(_OwnerInfo.CurrTile.Index.Item2 < 3)
                {
                    _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Up];
                }
                else
                {
                    _IsMoveFinish = true;
                    if (_OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Down]?.GetTakedUnit() == null)
                    {
                        _IsCanSummon = true;
                    }
                }
            }

            public override bool Initialize()
            {
                return true;
            }

            public override void Logic()
            {
            }
        }

        private class SummonerStateAttack : StateAttack
        {
            bool[] _IsCheckTile;
            public override string CheckTransition()
            {
                return "SummonerStateAttack";
            }

            public override void Exit()
            {
                int SummonTile = 0;
               
                bool summon = false;
                while (summon == false)
                {
                    SummonTile = Random.Range(0, _OwnerInfo.CurrTile.AdjacentTiles.Length);
                    if (_IsCheckTile[SummonTile] == true)
                    {
                        continue;
                    }
                    if (_OwnerInfo.CurrTile.AdjacentTiles[SummonTile] != null)
                    {
                        if (_OwnerInfo.CurrTile.AdjacentTiles[SummonTile].GetTakedUnit() == null)
                        {
                            summon = true;
                            var unit = _OwnerInfo.CurrTile.AdjacentTiles[SummonTile].SummonUnit(_OwnerInfo.SummonUnit[0]);
                            unit.GetComponent<SubUnit>().Owner = _Owner;
                        }
                        else
                        {
                            _IsCheckTile[SummonTile] = true;
                        }
                    }
                }

                Array.Fill(_IsCheckTile, false);

            }

            public override bool Initialize()
            {
                _IsCheckTile = new bool[_OwnerInfo.CurrTile.AdjacentTiles.Length];
                return true;
            }

            public override void Logic()
            {
            }
        }
    }
}