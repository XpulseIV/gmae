﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AstralAssault;

public class CollisionSystem : IUpdateEventListener 
{
    public List<Collider> Colliders { get; } = new();
    private List<Tuple<Collider, Collider>> _lastCollisions = new();

    public CollisionSystem()
    {
        UpdateEventSource.UpdateEvent += OnUpdate;
    }

    public void OnUpdate(object sender, UpdateEventArgs e)
    {
        List<Tuple<Collider, Collider>> currentCollisions = new();

        for (int i = 0; i < Colliders.Count - 1; i++)
        {
            Collider collider = Colliders[i];
            for (int j = i + 1; j < Colliders.Count; j++)
            {
                Collider other = Colliders[j];
                if (collider == other) continue;

                Tuple<Collider, Collider> colliderPair = new(collider, other);

                if (!collider.CollidesWith(
                    other,
                    e.DeltaTime,
                    out Vector2 initialImpulseThis,
                    out Vector2 initialImpulseOther,
                    out Vector2 totalImpulseThis,
                    out Vector2 totalImpulseOther))
                {
                    if (_lastCollisions.Any(t => 
                        t.Item1 == collider && t.Item2 == other ||
                        t.Item2 == collider && t.Item1 == other))
                    {
                        collider.Parent.OnCollisionExit(other);
                        other.Parent.OnCollisionExit(collider);
                    }

                    continue;
                }

                if (collider.IsSolid && other.IsSolid && collider.Parent.TimeSinceSpawned > 1000)
                {
                    collider.Parent.Position += initialImpulseThis;
                    other.Parent.Position += initialImpulseOther;
                    
                    collider.Parent.Velocity += totalImpulseThis / e.DeltaTime / 10F;
                    other.Parent.Velocity += totalImpulseOther / e.DeltaTime / 10F;
                    
                    collider.SetPosition(collider.Parent.Position.ToPoint());
                    other.SetPosition(other.Parent.Position.ToPoint());
                }
                
                currentCollisions.Add(colliderPair);
                if (_lastCollisions.Any(t => 
                    t.Item1 == collider && t.Item2 == other ||
                    t.Item2 == collider && t.Item1 == other))
                    continue;

                collider.Parent.OnCollisionEnter(other);
                other.Parent.OnCollisionEnter(collider);
            }
        }

        if (currentCollisions.Count < _lastCollisions.Count)
        {
            List<Tuple<Collider, Collider>> removedCollisions = _lastCollisions.Except(currentCollisions).ToList();
            foreach (Tuple<Collider, Collider> removedCollision in removedCollisions)
            {
                removedCollision.Item1.Parent.OnCollisionExit(removedCollision.Item2);
                removedCollision.Item2.Parent.OnCollisionExit(removedCollision.Item1);
            }
        }

        _lastCollisions = currentCollisions;
    }

    public void AddCollider(Collider collider)
    {
        Colliders.Add(collider);
    }

    public void RemoveCollider(Collider collider)
    {
        Colliders.Remove(collider);
    }
}