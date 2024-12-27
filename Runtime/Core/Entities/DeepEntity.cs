using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Deep.Views;
using System.Linq;

namespace Deep
{
    //TODO: -------------------------------------------------------------------

    // 1. need to define collision type somewhere inside deepEntity (currently everything is set to bounce)
    // 2. Movement body reads from attributes.movementRadius which needs to be removed.

    //TODO: -------------------------------------------------------------------

    [RequireComponent(typeof(DeepMovementBody)), RequireComponent(typeof(Rigidbody2D))]
    public class DeepEntity : MonoBehaviour
    {
        // * Views
        public List<DeepViewLink> views = new List<DeepViewLink>();
        // * Resources
        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();
        // * Attributes
        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();
        // * Flags
        public Dictionary<D_Flag, DeepFlag> flags = new Dictionary<D_Flag, DeepFlag>();
        // * Behaviors
        [SerializeReference]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();
        // * Inventory
        public DeepEntityInventory inventory { get; private set; }
        // * Events
        public DeepEntityEvents events = new DeepEntityEvents();
        // * Team
        public D_Team team;
        // * Type
        public D_EntityType type;

        // * Status
        public bool dying;
        public bool initialized;

        // * Ownership
        public DeepEntity creator;
        public DeepEntity owner;
        public bool hasOwner => owner != null;

        // * Lookups
        //abilities are a subset of behaviors
        public List<DeepAbility> abilities { get; private set; } = new List<DeepAbility>();

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public Rigidbody2D rb { get; private set; }
        public CircleCollider2D col { get; private set; }
        public DeepMovementBody mb { get; private set; }
        public Vector2 lookDirection { get; set; }
        public Transform cachedTransform { get; private set; }//slightly faster than mono.transform

        public Dictionary<Collider2D, DeepEntity> activeCollisions { get; private set; } = new Dictionary<Collider2D, DeepEntity>();

        public DeepEntity Initialize(EntityTemplate template, DeepEntity creator = null)
        {
            //pointless GC! optimize this! Requires replacing addAtt with setAtt etc below.
            views.Clear();
            attributes.Clear();
            resources.Clear();
            flags.Clear();

            behaviors.Clear();
            abilities.Clear();

            inventory = new DeepEntityInventory(this);

            if (rb == null)
            {
                rb = gameObject.GetComponent<Rigidbody2D>();
            }
            if (col == null)
            {
                col = gameObject.GetComponent<CircleCollider2D>();
            }
            if (mb == null)
            {
                mb = gameObject.GetComponent<DeepMovementBody>();
                mb.entity = this;
            }

            cachedTransform = transform;

            team = template.team;
            type = template.type;

            this.creator = creator;
            owner = creator;

            //get attributes from template
            foreach (KeyValuePair<D_Attribute, A> attPair in template.attributes)
            {
                this.AddAttribute(attPair.Key, attPair.Value);
            }
            //fill in missing attributes
            foreach (D_Attribute att in Enum.GetValues(typeof(D_Attribute)))
            {
                if (!attributes.ContainsKey(att))
                {
                    this.AddAttribute(att, new DeepAttribute(0f));
                }
            }
            //get resources from template
            foreach (KeyValuePair<D_Resource, R> res in template.resources)
            {
                this.AddResource(res.Key, res.Value);
            }
            //fill in missing resources
            foreach (D_Resource res in Enum.GetValues(typeof(D_Resource)))
            {
                if (!resources.ContainsKey(res))
                {
                    this.AddResource(res, new DeepResource(1, 0));
                }
            }
            //fill in flags (they are all false by default)
            foreach (D_Flag flag in Enum.GetValues(typeof(D_Flag)))
            {
                if (!flags.ContainsKey(flag))
                    flags.Add(flag, new DeepFlag());
            }

            rb.velocity = Vector2.zero;
            mb.SetVelocity(Vector2.zero);

            lookDirection = Vector2.right;

            initialized = true;
            //OnEnable gets called before this, so we need to initialize here when entities are created.
            App.state.game.RegisterEntity(this);
            RefreshColliderSize();

            //* ACTIVE
            gameObject.SetActive(true);

            //add behaviors from template. We do this later so that coroutines will work.
            foreach (DeepBehavior b in template.behaviors)
            {
                b.parent = this;
                b.owner = owner != null ? owner : this;
                behaviors.Add(b);
                b.Init();
                if (b is DeepAbility a)
                {
                    abilities.Add(a);
                }
            }

            if (template.initialInventory != null)
            {
                foreach (ItemInInventory item in template.initialInventory)
                {
                    inventory.TryAddItem(item);
                }
            }

            //Important we do this after adding behaviors ^ 
            events.OnEntityEnable?.Invoke();

            foreach (string v in template.views)
            {
                this.AddView(v);
            }
            return this;
        }

        private void OnDisable()
        {
            dying = false;
            events.OnEntityDisable?.Invoke();
        }

        public void RefreshColliderSize()
        {
            float r = 0f;
            foreach (DeepViewLink view in views)
            {
                if (!view.viewAffectsHitbox)
                {
                    continue;
                }
                r = Mathf.Max(r, view.viewRadius);
            }
            col.radius = r;
        }

        //////////////////////////////////////////////////////////////

        private void OnTriggerEnter2D(Collider2D col)
        {
            events.OnTriggerEnter2D?.Invoke(col);
            if (col.gameObject.TryGetComponent(out DeepEntity e))
            {
                activeCollisions[col] = e;
                events.OnEntityCollisionEnter?.Invoke(e);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            events.OnTriggerExit2D?.Invoke(col);
            activeCollisions.Remove(col);
            if (col.gameObject.TryGetComponent(out DeepEntity e))
            {
                events.OnEntityCollisionExit?.Invoke(e);
            }
        }

        //-----------------------------------
        //            CREATE
        //-----------------------------------

        public static DeepEntity Create(EntityTemplate template, Vector3 position, Quaternion rotation, params string[] extraViews)
        {
            return Create(template, null, position, rotation, Vector3.one, extraViews);
        }
        public static DeepEntity Create(EntityTemplate template, DeepEntity creator, Vector3 position, Quaternion rotation, params string[] extraViews)
        {
            return Create(template, creator, position, rotation, Vector3.one, extraViews);
        }

        public static DeepEntity Create(EntityTemplate template, DeepEntity creator, Vector3 position, Quaternion rotation, Vector3 scale, params string[] extraViews)
        {
            DeepEntity e = DeepManager.instance.PullEntity();
            e.transform.position = position;
            e.transform.rotation = rotation;
            e.transform.localScale = scale;
            e.Initialize(template, creator);
            foreach (string view in extraViews)
            {
                e.AddView(view);
            }
            return e;
        }
    }
}
