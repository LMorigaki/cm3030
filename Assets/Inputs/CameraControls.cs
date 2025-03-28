//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Inputs/CameraControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CameraControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""12c2545f-07c7-43ff-8a99-5255c13b2ee0"",
            ""actions"": [
                {
                    ""name"": ""RotateCam"",
                    ""type"": ""Value"",
                    ""id"": ""e571484a-1394-494c-9cfb-b71d911f412a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ZoomCam"",
                    ""type"": ""Value"",
                    ""id"": ""72949952-bd80-43ae-bdb8-cb74bb465561"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveCam"",
                    ""type"": ""Value"",
                    ""id"": ""0bb8dacd-da07-42da-9c38-6cf4e9a5ba3d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b18a92d3-fdf5-480f-b10a-6bc6c8e19fb9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ac2cda2-0472-4fa0-a973-ef560276eece"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""64710bde-248b-469d-a9fd-4ee05eb7ecc4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCam"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""03c41ea0-63c4-46de-90c4-d8ed6b4d2086"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a8f99fba-407a-476f-8630-44209bfe8112"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6646d930-d367-4bdd-a3a9-cc2d5f1554c8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""116711c6-3210-4099-9312-60257aa02fa6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_RotateCam = m_Camera.FindAction("RotateCam", throwIfNotFound: true);
        m_Camera_ZoomCam = m_Camera.FindAction("ZoomCam", throwIfNotFound: true);
        m_Camera_MoveCam = m_Camera.FindAction("MoveCam", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_RotateCam;
    private readonly InputAction m_Camera_ZoomCam;
    private readonly InputAction m_Camera_MoveCam;
    public struct CameraActions
    {
        private @CameraControls m_Wrapper;
        public CameraActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateCam => m_Wrapper.m_Camera_RotateCam;
        public InputAction @ZoomCam => m_Wrapper.m_Camera_ZoomCam;
        public InputAction @MoveCam => m_Wrapper.m_Camera_MoveCam;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @RotateCam.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnRotateCam;
                @RotateCam.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnRotateCam;
                @RotateCam.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnRotateCam;
                @ZoomCam.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomCam;
                @ZoomCam.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomCam;
                @ZoomCam.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoomCam;
                @MoveCam.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCam;
                @MoveCam.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCam;
                @MoveCam.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveCam;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RotateCam.started += instance.OnRotateCam;
                @RotateCam.performed += instance.OnRotateCam;
                @RotateCam.canceled += instance.OnRotateCam;
                @ZoomCam.started += instance.OnZoomCam;
                @ZoomCam.performed += instance.OnZoomCam;
                @ZoomCam.canceled += instance.OnZoomCam;
                @MoveCam.started += instance.OnMoveCam;
                @MoveCam.performed += instance.OnMoveCam;
                @MoveCam.canceled += instance.OnMoveCam;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    public interface ICameraActions
    {
        void OnRotateCam(InputAction.CallbackContext context);
        void OnZoomCam(InputAction.CallbackContext context);
        void OnMoveCam(InputAction.CallbackContext context);
    }
}
