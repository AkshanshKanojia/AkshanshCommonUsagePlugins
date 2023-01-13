using UnityEngine;
using AkshanshKanojia.Inputs.Mobile;

namespace AkshanshKanojia.Controllers.PointClick
{
    public class PointClickInput : MobileInputs
    {
        PointClickManager clickMang;
        public override void Start()
        {
            base.Start();
            clickMang = FindObjectOfType<PointClickManager>();
        }

        #region Inputs
        public override void OnTapEnd(MobileInputManager.TouchData _data)
        {
        }

        public override void OnTapMove(MobileInputManager.TouchData _data)
        {
        }

        public override void OnTapped(MobileInputManager.TouchData _data)
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(_data.TouchPosition),out RaycastHit _hit))
            {
                clickMang.SetTarget(_hit.point);
            }
        }

        public override void OnTapStay(MobileInputManager.TouchData _data)
        {
        }
        #endregion

    }
}
