using UnityEngine;

namespace Deep
{
    public class PlayerAim : DeepBehavior
    {
        private float _trackingSpeed = 30f;

        public override void Init()
        {
            parent.events.UpdateNorm += Aim;
        }

        public override void Teardown()
        {
            parent.events.UpdateNorm -= Aim;
        }

        RaycastHit hit;
        Ray ray;

        private void Aim()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layers.mouseLayerMask))
            {
                Vector2 currentLookDirection = parent.lookDirection;
                Vector2 newLookDirection = new Vector2(hit.point.x, hit.point.y) - new Vector2(parent.cachedTransform.position.x, parent.cachedTransform.position.y);
                //rotate to new look direction limited by max degrees per second
                float newAngle = Vector2.Angle(currentLookDirection, newLookDirection);
                float limitedAngle = Mathf.Min(_trackingSpeed * Time.deltaTime, newAngle);
                Vector2 rotated = rotate(currentLookDirection, limitedAngle);

                new LookAction(parent, rotated).ExecuteSilent();
            }
        }
        
        public static Vector2 rotate(Vector2 v, float degrees)
        {
            //convert degrees to radians
            degrees = Mathf.Deg2Rad * degrees;
            return new Vector2(
                v.x * Mathf.Cos(degrees) - v.y * Mathf.Sin(degrees),
                v.x * Mathf.Sin(degrees) + v.y * Mathf.Cos(degrees)
            );
        }
    }
}
