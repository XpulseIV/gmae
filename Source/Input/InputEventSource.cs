﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace astral_assault;

public static class InputEventSource
{
    public static event EventHandler<KeyboardEventArgs> 
        KeyboardEvent, 
        KeyboardPressedEvent;
    
    public static event EventHandler<MouseButtonEventArgs> 
        MouseButtonEvent, 
        MouseButtonPressedEvent;

    public static event EventHandler<MouseMoveEventArgs>
        MouseMoveEvent;

    private static List<Keys> _keysDown = new(); 
    private static List<Keys> _prevKeysDown = new();
    
    private static readonly List<MouseButtons> MouseDown = new();
    private static List<MouseButtons> _prevMouseDown = new();

    private static Point _mousePos;
    private static Point _prevMousePos;

    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        Side1,
        Side2
    }

    public static void Initialize()
    {
        UpdateEventSource.UpdateEvent += OnUpdate;
    }

    private static void OnUpdate(object sender, UpdateEventArgs e)
    {
        HandleKeyboard();
        HandleMouseButtons();
        HandleMouseMovement();
    }

    private static void HandleKeyboard()
    {
        _prevKeysDown = _keysDown;
        _keysDown = Keyboard.GetState().GetPressedKeys().ToList();

        if (_keysDown.Count == 0)
        {
            _prevKeysDown.Clear();
            return;
        }

        List<Keys> keysDown = new();
        List<Keys> keysPressed = new();
        
        foreach (Keys key in _keysDown)
        {
            keysDown.Add(key);
            
            if (!_prevKeysDown.Contains(key))
            {
                keysPressed.Add(key);
            }
        }
        
        KeyboardEvent?.Invoke(null, new KeyboardEventArgs(keysDown.ToArray()));
        KeyboardPressedEvent?.Invoke(null, new KeyboardEventArgs(keysPressed.ToArray()));
    }
    
    private static void HandleMouseButtons()
    {
        MouseState mouseState = Mouse.GetState();

        _prevMouseDown = MouseDown;
        MouseDown.Clear();

        ButtonState[] buttonStates = new ButtonState[5];
        buttonStates[0] = mouseState.LeftButton;
        buttonStates[1] = mouseState.RightButton;
        buttonStates[2] = mouseState.MiddleButton;
        buttonStates[3] = mouseState.XButton1;
        buttonStates[4] = mouseState.XButton2;

        for (int i = 0; i < 5; i++)
        {
            if (buttonStates[i] != ButtonState.Pressed) continue;

            MouseButtons button = (MouseButtons)i;
            
            MouseDown.Add(button);
            MouseButtonEvent?.Invoke(null, new MouseButtonEventArgs(button));

            if (!_prevMouseDown.Contains(button))
            {
                MouseButtonPressedEvent?.Invoke(null, new MouseButtonEventArgs(button));
            }
        }
    }

    private static void HandleMouseMovement()
    {
        _prevMousePos = _mousePos;
        _mousePos = Mouse.GetState().Position;

        if (_mousePos != _prevMousePos)
        {
            MouseMoveEvent?.Invoke(null, new MouseMoveEventArgs(_mousePos));
        }
    }
}