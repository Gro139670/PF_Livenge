

namespace Team
{
    public class TeamUnit : TeamHelper<TeamUnit>
    {
        public override void SetEnemyID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Enemy");
        }

        public override void SetTeamID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Our");
        }

        private new void Start()
        {
            base.Start();
            _OwnerInfo.LookDir = Unit.Direction.Up;
        }   
    }
}
