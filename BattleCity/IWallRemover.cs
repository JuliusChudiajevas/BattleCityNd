using System.Collections.Generic;


namespace BattleCity
{
    interface IWallRemover
    {
        List<RemoveWallRequest> removeWallRequests { get; }
    }
}
