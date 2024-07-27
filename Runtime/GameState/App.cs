using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    [DefaultExecutionOrder(-500)]
    public class App : MonoBehaviour
    {
        private static S_App _state;
        public static S_App state
        {
            get
            {
                if (_state == null)
                {
                    _state = new S_App();
                }
                return _state;
            }
        }
    }
}
