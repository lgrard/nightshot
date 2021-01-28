// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/PlayerInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""BasicScheme"",
            ""id"": ""b1a96fc7-5e80-4312-b707-822545e42b50"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""70cc3454-697d-4676-8414-73dc7121fb05"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(min=0.2)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aiming"",
                    ""type"": ""Value"",
                    ""id"": ""a57ce33b-768b-45f5-9043-537652ead438"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(min=0.2)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""6154a5bb-8740-4f57-beff-b9570c789859"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Defense"",
                    ""type"": ""Button"",
                    ""id"": ""2d5644de-aaa3-4031-8af7-18da813396f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""f8ff6e42-be90-4e1a-a936-91ae0936c899"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""8a66e2d2-e6dd-485a-991c-83a18891016e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a28753b0-5abb-4526-9f48-03c9a48f992f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""564c731e-45c1-4dd8-a3d2-7762d10fdff8"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab07bd97-aad6-469b-8e58-881a1f1fe646"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aiming"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43d6ed9d-37a6-4916-aeef-0be24bb8cf8b"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ef5e7ac-104c-46f3-a8ec-b0842fcbb6ba"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Defense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abdf67e3-89ac-4d4b-8d43-4f833a198061"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""793bed66-ddcb-498d-a708-17a275770ec5"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BasicScheme
        m_BasicScheme = asset.FindActionMap("BasicScheme", throwIfNotFound: true);
        m_BasicScheme_Movement = m_BasicScheme.FindAction("Movement", throwIfNotFound: true);
        m_BasicScheme_Aiming = m_BasicScheme.FindAction("Aiming", throwIfNotFound: true);
        m_BasicScheme_Fire = m_BasicScheme.FindAction("Fire", throwIfNotFound: true);
        m_BasicScheme_Defense = m_BasicScheme.FindAction("Defense", throwIfNotFound: true);
        m_BasicScheme_Dash = m_BasicScheme.FindAction("Dash", throwIfNotFound: true);
        m_BasicScheme_Melee = m_BasicScheme.FindAction("Melee", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // BasicScheme
    private readonly InputActionMap m_BasicScheme;
    private IBasicSchemeActions m_BasicSchemeActionsCallbackInterface;
    private readonly InputAction m_BasicScheme_Movement;
    private readonly InputAction m_BasicScheme_Aiming;
    private readonly InputAction m_BasicScheme_Fire;
    private readonly InputAction m_BasicScheme_Defense;
    private readonly InputAction m_BasicScheme_Dash;
    private readonly InputAction m_BasicScheme_Melee;
    public struct BasicSchemeActions
    {
        private @PlayerInputAction m_Wrapper;
        public BasicSchemeActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_BasicScheme_Movement;
        public InputAction @Aiming => m_Wrapper.m_BasicScheme_Aiming;
        public InputAction @Fire => m_Wrapper.m_BasicScheme_Fire;
        public InputAction @Defense => m_Wrapper.m_BasicScheme_Defense;
        public InputAction @Dash => m_Wrapper.m_BasicScheme_Dash;
        public InputAction @Melee => m_Wrapper.m_BasicScheme_Melee;
        public InputActionMap Get() { return m_Wrapper.m_BasicScheme; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicSchemeActions set) { return set.Get(); }
        public void SetCallbacks(IBasicSchemeActions instance)
        {
            if (m_Wrapper.m_BasicSchemeActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMovement;
                @Aiming.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnAiming;
                @Aiming.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnAiming;
                @Aiming.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnAiming;
                @Fire.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnFire;
                @Defense.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDefense;
                @Defense.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDefense;
                @Defense.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDefense;
                @Dash.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnDash;
                @Melee.started -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMelee;
                @Melee.performed -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMelee;
                @Melee.canceled -= m_Wrapper.m_BasicSchemeActionsCallbackInterface.OnMelee;
            }
            m_Wrapper.m_BasicSchemeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Aiming.started += instance.OnAiming;
                @Aiming.performed += instance.OnAiming;
                @Aiming.canceled += instance.OnAiming;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Defense.started += instance.OnDefense;
                @Defense.performed += instance.OnDefense;
                @Defense.canceled += instance.OnDefense;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Melee.started += instance.OnMelee;
                @Melee.performed += instance.OnMelee;
                @Melee.canceled += instance.OnMelee;
            }
        }
    }
    public BasicSchemeActions @BasicScheme => new BasicSchemeActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IBasicSchemeActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAiming(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnDefense(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnMelee(InputAction.CallbackContext context);
    }
}
