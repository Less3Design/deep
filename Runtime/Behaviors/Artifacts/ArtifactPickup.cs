using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    public class ArtifactPickup : DeepBehavior
    {
        //if an entity collides and has artifact space, artifact is destroyed and entity gets +1 artifact

        public override void Init()
        {
            parent.events.OnEntityCollisionEnter += OnCollision;
        }

        public override void Teardown()
        {
            parent.events.OnEntityCollisionEnter -= OnCollision;
        }

        private void OnCollision(DeepEntity other)
        {
            if (other.type == D_EntityType.Projectile || other.team != D_Team.Player)
            {
                return;
            }
            DeepResource r = other.resources[D_Resource.Artifacts];
            new KillAction(parent).Execute();
        }
    }
}