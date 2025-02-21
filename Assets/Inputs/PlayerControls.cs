// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""496b570d-8c8f-4915-b5a9-7ed8a85e6d64"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b008781c-3cd8-4390-bd6a-22e9b01f1a1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a29a60d0-a311-4386-ae8e-5672b3057cb8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2bb9be4f-6cc2-4951-babd-26e01c6264c4"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b82126f1-5890-4c40-a50a-39d2a8c5be97"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Gameplay"",
            ""id"": ""12c2545f-07c7-43ff-8a99-5255c13b2ee0"",
            ""actions"": [
                {
                    ""name"": ""RotateCam"",
                    ""type"": ""Value"",
                    ""id"": ""e571484a-1394-494c-9cfb-b71d911f412a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ZoomCam"",
                    ""type"": ""Value"",
                    ""id"": ""72949952-bd80-43ae-bdb8-cb74bb465561"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveCam"",
                    ""type"": ""Value"",
                    ""id"": ""0bb8dacd-da07-42da-9c38-6cf4e9a5ba3d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
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
        },
        {
            ""name"": ""Gameboard"",
            ""id"": ""a00218c2-40a4-40cd-a741-f9d25a3e87ea"",
            ""actions"": [
                {
                    ""name"": ""Point"",
                    ""type"": ""Value"",
                    ""id"": ""804ee9ec-47e3-4982-ab50-6b72ea627f8e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""2063ff3e-f8b7-4551-a609-f795b6051abb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""51605565-da08-4f67-adcb-0a12a3d6ad26"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c531dfe-f811-49d1-bf53-a8e8dabb0d1e"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Placeable"",
            ""id"": ""3d0b3ab7-1a4f-4c42-81fa-3f421f5ba0b1"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""ec6b94ee-20ac-41bb-9c51-3eb58884e459"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""eda6745e-0c10-4fd4-a643-522fd58f7a1d"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_RotateCam = m_Gameplay.FindAction("RotateCam", throwIfNotFound: true);
        m_Gameplay_ZoomCam = m_Gameplay.FindAction("ZoomCam", throwIfNotFound: true);
        m_Gameplay_MoveCam = m_Gameplay.FindAction("MoveCam", throwIfNotFound: true);
        // Gameboard
        m_Gameboard = asset.FindActionMap("Gameboard", throwIfNotFound: true);
        m_Gameboard_Point = m_Gameboard.FindAction("Point", throwIfNotFound: true);
        m_Gameboard_Click = m_Gameboard.FindAction("Click", throwIfNotFound: true);
        // Placeable
        m_Placeable = asset.FindActionMap("Placeable", throwIfNotFound: true);
        m_Placeable_Rotate = m_Placeable.FindAction("Rotate", throwIfNotFound: true);
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

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_Point;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_RotateCam;
    private readonly InputAction m_Gameplay_ZoomCam;
    private readonly InputAction m_Gameplay_MoveCam;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateCam => m_Wrapper.m_Gameplay_RotateCam;
        public InputAction @ZoomCam => m_Wrapper.m_Gameplay_ZoomCam;
        public InputAction @MoveCam => m_Wrapper.m_Gameplay_MoveCam;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @RotateCam.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCam;
                @RotateCam.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCam;
                @RotateCam.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCam;
                @ZoomCam.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoomCam;
                @ZoomCam.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoomCam;
                @ZoomCam.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoomCam;
                @MoveCam.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCam;
                @MoveCam.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCam;
                @MoveCam.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCam;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
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
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Gameboard
    private readonly InputActionMap m_Gameboard;
    private IGameboardActions m_GameboardActionsCallbackInterface;
    private readonly InputAction m_Gameboard_Point;
    private readonly InputAction m_Gameboard_Click;
    public struct GameboardActions
    {
        private @PlayerControls m_Wrapper;
        public GameboardActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Point => m_Wrapper.m_Gameboard_Point;
        public InputAction @Click => m_Wrapper.m_Gameboard_Click;
        public InputActionMap Get() { return m_Wrapper.m_Gameboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameboardActions set) { return set.Get(); }
        public void SetCallbacks(IGameboardActions instance)
        {
            if (m_Wrapper.m_GameboardActionsCallbackInterface != null)
            {
                @Point.started -= m_Wrapper.m_GameboardActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_GameboardActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_GameboardActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_GameboardActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_GameboardActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_GameboardActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_GameboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public GameboardActions @Gameboard => new GameboardActions(this);

    // Placeable
    private readonly InputActionMap m_Placeable;
    private IPlaceableActions m_PlaceableActionsCallbackInterface;
    private readonly InputAction m_Placeable_Rotate;
    public struct PlaceableActions
    {
        private @PlayerControls m_Wrapper;
        public PlaceableActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_Placeable_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_Placeable; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlaceableActions set) { return set.Get(); }
        public void SetCallbacks(IPlaceableActions instance)
        {
            if (m_Wrapper.m_PlaceableActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_PlaceableActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlaceableActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlaceableActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_PlaceableActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public PlaceableActions @Placeable => new PlaceableActions(this);
    public interface IUIActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
    }
    public interface IGameplayActions
    {
        void OnRotateCam(InputAction.CallbackContext context);
        void OnZoomCam(InputAction.CallbackContext context);
        void OnMoveCam(InputAction.CallbackContext context);
    }
    public interface IGameboardActions
    {
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
    }
    public interface IPlaceableActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
}
