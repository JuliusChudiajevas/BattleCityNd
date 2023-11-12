using System.Collections.Generic;

namespace BattleCity
{
    interface IWallCreator
    {
        List<CreateWallRequest> createWallRequests { get; }
    }
}
