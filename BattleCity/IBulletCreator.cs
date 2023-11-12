using System.Collections.Generic;


namespace BattleCity
{
    interface IBulletCreator
    {
        List<CreateBulletRequest> createBulletRequests { get; }
    }
}
