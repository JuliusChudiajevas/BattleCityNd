using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BattleCity
{
    class KeyboardController
    {
        private List<MyKey> registeredKeys = new List<MyKey>();

        public void registerKeys(IKeyboardSupport entity)
        {
            var keyCodes = entity.keys.Select(k => k.keyCode).ToList();

            foreach (var keyCodeToAdd in keyCodes)
            {
                var keyExists = registeredKeys.Exists(k => k.keyCode == keyCodeToAdd);
                if (!keyExists) registeredKeys.Add(new MyKey(keyCodeToAdd));
            }
        }

        public void pressKey(Keys keyCode)
        {
            var maybeKey = registeredKeys.FirstOrDefault(k => k.keyCode == keyCode);
            if (maybeKey != null) maybeKey.currentState = KeyState.Pressed;
        }
        public void releaseKey(Keys keyCode)
        {
            var maybeKey = registeredKeys.FirstOrDefault(k => k.keyCode == keyCode);
            if (maybeKey != null) maybeKey.currentState = KeyState.Released;
        }

        public void relayKeys(IKeyboardSupport entity)
        {
            var entityKeys = entity.keys.Select(k => k.keyCode).ToList();
            var legalKeys = registeredKeys.Where(k => entityKeys.Contains(k.keyCode)).ToList();
            entity.relayKeys(legalKeys);
        }
    }
}
