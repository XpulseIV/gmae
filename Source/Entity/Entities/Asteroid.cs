﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstralAssault;

public class Asteroid : Entity
{
    private readonly float _rotSpeed;
    private readonly Sizes _size;
    private bool _hasExploded;

    public enum Sizes
    {
        Smallest,
        Small,
        Medium
    }

    public Asteroid(GameplayState gameState, Vector2 position, Sizes size) : base(gameState, position)
    {
        _size = size;

        Random rnd = new();
        _rotSpeed = rnd.Next(5, 20) / 10F;
        Velocity.X = rnd.Next(-100, 100);
        Velocity.Y = rnd.Next(-100, 100);

        Texture2D spriteSheet;
        int colliderSize;
        int spriteSize;
        float mass;

        switch (size)
        {
            case Sizes.Smallest:
                spriteSheet = AssetManager.LoadTexture("asteroid1");
                spriteSize = 16;
                colliderSize = 10;
                MaxHP = 12;
                HP = MaxHP;
                ContactDamage = 5;
                mass = 6;
                break;
            case Sizes.Small:
                spriteSheet = AssetManager.LoadTexture("asteroid2");
                spriteSize = 24;
                colliderSize = 16;
                MaxHP = 24;
                HP = MaxHP;
                ContactDamage = 7;
                mass = 12;
                break;
            case Sizes.Medium:
                spriteSheet = AssetManager.LoadTexture("asteroid3");
                spriteSize = 32;
                colliderSize = 24;
                MaxHP = 36;
                HP = MaxHP;
                ContactDamage = 12;
                mass = 18;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Frame frame = new(
              e:new Rectangle(0,              0, spriteSize, spriteSize),
            see:new Rectangle(spriteSize,     0, spriteSize, spriteSize),
             se:new Rectangle(spriteSize * 2, 0, spriteSize, spriteSize),
            sse:new Rectangle(spriteSize * 3, 0, spriteSize, spriteSize));

        Animation animation = new(new[] { frame }, true);

        SpriteRenderer = new SpriteRenderer(spriteSheet, new[] { animation });
        
        Collider = new Collider(
            this, 
            new Rectangle(
                new Point((int)Position.X - colliderSize / 2, (int)Position.Y - colliderSize / 2), 
                new Point(colliderSize, colliderSize)),
            true,
            mass);
        GameState.CollisionSystem.AddCollider(Collider);

        OutOfBoundsBehavior = OutOfBounds.Wrap;

        IsActor = true;
    }

    protected override void OnDeath()
    {
        if (!_hasExploded && _size - 1 >= 0)
        {
            Random rnd = new();
            int amount = rnd.Next(1, 4);
            
            for (int i = 0; i < amount; i++)
            {
                GameState.Entities.Add(new Asteroid(GameState, Position, _size - 1));
            }
        }
        
        _hasExploded = true;

        base.OnDeath();
    }

    public override void OnUpdate(object sender, UpdateEventArgs e)
    {
        base.OnUpdate(sender, e);
        
        Rotation += _rotSpeed * e.DeltaTime;
        if (Rotation > Math.PI) Rotation = (float)-Math.PI;
    }
}