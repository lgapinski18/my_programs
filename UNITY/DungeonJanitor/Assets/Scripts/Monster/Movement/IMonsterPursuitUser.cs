//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

public interface IMonsterPursuitUser : IGeneralMovementUser
{
    public void SetMovementComponent(MonsterPursuitComponent movementComponent);

    public void OnPursuitEndedCallback(MonsterPursuitComponent pursuitComponent);
}

