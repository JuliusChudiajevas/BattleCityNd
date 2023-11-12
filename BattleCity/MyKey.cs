using System.Windows.Forms;

namespace BattleCity
{
    class MyKey
    {
        public Keys keyCode { get; }

        public KeyState currentState { get; set; }
        public MyKey(Keys key)
        {
            keyCode = key;
            currentState = KeyState.Released;
        }
        public bool isPressed() { return currentState == KeyState.Pressed; }
        public void press()
        {
            currentState = KeyState.Pressed;
        }

        public void release()
        {
            currentState = KeyState.Released;
        }
    }
}
