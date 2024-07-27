using System;

namespace Deep
{
    [Serializable]
    public class S_App
    {
        public S_Game game { get; private set; } = new S_Game();
        public S_UI ui { get; private set; } = new S_UI();
    }
}
