// Auto-generated input actions for camera controls
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.Collections;
using System.Collections.Generic;

public class CameraInputActions : IInputActionCollection
{
    private InputActionAsset asset;
    public CameraInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
            ""name"": ""Camera"",
            ""maps"": [
                {
                    ""name"": ""Camera"",
                    ""actions"": [
                        { ""name"": ""Toggle"", ""type"": ""Button"", ""expectedControlType"": ""Button"" },
                        { ""name"": ""Look"", ""type"": ""Value"", ""expectedControlType"": ""Vector2"" }
                    ],
                    ""bindings"": [
                        { ""name"": """", ""id"": ""toggle"", ""path"": ""<Keyboard>/tab"", ""action"": ""Toggle"" },
                        { ""name"": """", ""id"": ""look"", ""path"": ""<Mouse>/delta"", ""action"": ""Look"" }
                    ]
                }
            ]
        }");
        Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        Toggle = Camera.FindAction("Toggle", throwIfNotFound: true);
        Look = Camera.FindAction("Look", throwIfNotFound: true);
    }
    public InputActionMap Camera { get; }
    public InputAction Toggle { get; }
    public InputAction Look { get; }
    public void Enable() => asset.Enable();
    public void Disable() => asset.Disable();

    // IInputActionCollection implementation
    public InputBinding? bindingMask { get => asset.bindingMask; set => asset.bindingMask = value; }
    public ReadOnlyArray<InputDevice>? devices { get => asset.devices; set => asset.devices = value; }
    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;
    public bool Contains(InputAction action) => asset.Contains(action);
    public IEnumerator<InputAction> GetEnumerator() => asset.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => asset.GetEnumerator();
}
