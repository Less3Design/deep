using UnityEngine;

namespace Deep
{
    public class AvoidOtherEntities : DeepBehavior
    {
        //* Optimization settings.
        private const int UPDATE_EVERY = 6;
        private const int MAX_COLLISIONS = 3;//how many valid collisions we consider per update.

        public D_TeamSelector teamToAvoid;
        public D_EntityTypeSelector typeToAvoid;
        public float avoidStrength;

        private Vector3 nudge;
        private Vector3 parentPos;
        private Vector3 dir;
        private int frame;
        private float deltaTimeAcrossFrames;

        public AvoidOtherEntities(D_TeamSelector team, D_EntityTypeSelector type, float force)
        {
            teamToAvoid = team;
            typeToAvoid = type;
            avoidStrength = force;
        }

        public override void Init()
        {
            parent.events.OnEntityCollisionStay += Avoid;
        }

        public override void Teardown()
        {
            parent.events.OnEntityCollisionStay -= Avoid;
        }

        private void Avoid()
        {
            deltaTimeAcrossFrames += Time.deltaTime;
            if (frame % UPDATE_EVERY != 0)
            {
                frame++;
                return;
            }
            frame++;

            nudge = Vector3.zero;
            parentPos = parent.cachedTransform.position;
            float deltaStrength = deltaTimeAcrossFrames * avoidStrength;
            int i = 0;
            foreach (DeepEntity entity in parent.activeCollisions.Values)
            {
                if (teamToAvoid.HasTeam(entity.team) && typeToAvoid.HasEntityType(entity.type))
                {
                    dir = parentPos - entity.cachedTransform.position;
                    //slightly faster normalize. (we might run thousands per frame)
                    nudge += (dir / Mathf.Sqrt(dir.sqrMagnitude)) * deltaStrength;
                    i++;
                    if (i > MAX_COLLISIONS)
                    {
                        continue;
                    }
                }
            }
            parent.mb.AddForce(nudge);
            deltaTimeAcrossFrames = 0f;
        }
    }
}
