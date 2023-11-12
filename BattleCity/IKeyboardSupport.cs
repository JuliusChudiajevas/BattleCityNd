using System.Collections.Generic;

namespace BattleCity
{
    interface IKeyboardSupport
    {
        List<MyKey> keys { get; }
        void relayKeys(List<MyKey> newKeys);

    }
}
