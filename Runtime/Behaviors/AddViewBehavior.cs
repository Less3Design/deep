using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class AddViewBehavior : DeepBehavior
    {
        public string viewName;
        public Action<DeepViewLink> viewInit;

        private DeepViewLink view;

        public AddViewBehavior(string viewName, Action<DeepViewLink> viewInit)
        {
            this.viewName = viewName;
            this.viewInit = viewInit;
        }

        public override void Init()
        {
            view = parent.AddView(viewName, viewInit);
        }

        public override void Teardown()
        {
            view.StartReturn();
        }
    }
}
