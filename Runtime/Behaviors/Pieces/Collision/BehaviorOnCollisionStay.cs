using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deep
{
    /// <summary>
    /// Apply behavior to all entities that collide with parent entity. Filter based on team and type. Behavior is always applied so things will stack.
    /// </summary>
    public class BehaviorOnCollisionStay : DeepBehavior
    {
        public DeepBehavior behavior;
        public D_TeamSelector teamTarget;
        public D_EntityTypeSelector typeTarget;

        private HashSet<Tuple<DeepEntity, DeepAction>> containedEntities = new HashSet<Tuple<DeepEntity, DeepAction>>();

        public BehaviorOnCollisionStay(DeepBehavior behavior, D_TeamSelector teamTarget = D_TeamSelector.All, D_EntityTypeSelector typeTarget = D_EntityTypeSelector.All)
        {
            this.behavior = behavior;
            this.teamTarget = teamTarget;
            this.typeTarget = typeTarget;
        }

        public override void Init()
        {
            parent.events.OnEntityCollisionEnter += EntityEnter;
            parent.events.OnEntityCollisionExit += EntityExit;
        }

        public override void Teardown()
        {
            foreach (Tuple<DeepEntity, DeepAction> pair in containedEntities)
            {
                pair.Item2.Execute();
            }

            parent.events.OnEntityCollisionEnter -= EntityEnter;
            parent.events.OnEntityCollisionExit -= EntityExit;
        }

        private void EntityEnter(DeepEntity e)
        {
            if (!teamTarget.HasTeam(e.team) || !typeTarget.HasEntityType(e.type))
            {
                return;
            }
            
            //add the behavior and create a remove action we can execute later
            AddBehaviorAction addAction = new AddBehaviorAction(e, owner, behavior.Clone());
            DeepAction removeHandle = addAction.CreateRemoveAction(owner);
            addAction.Execute();
            containedEntities.Add(new Tuple<DeepEntity, DeepAction>(e, removeHandle));
        }

        private void EntityExit(DeepEntity e)
        {
            var found = containedEntities.FirstOrDefault(pair => pair.Item1 == e);

            if (found != null)
            {
                found.Item2.Execute();
                containedEntities.Remove(found);
            }
        }
    }
}