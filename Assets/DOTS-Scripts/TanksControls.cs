// GENERATED AUTOMATICALLY FROM 'Assets/DOTS-Scripts/TanksControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TanksControls : IInputActionCollection
{
    private InputActionAsset asset;
    public TanksControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TanksControls"",
    ""maps"": [
        {
            ""name"": ""InGame"",
            ""id"": ""bbb28852-5fcb-423c-a322-ded9f2bbe9e5"",
            ""actions"": [
                {
                    ""name"": ""Player1Move"",
                    ""id"": ""3bd33c22-252b-4e74-878e-509613379960"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Player2Move"",
                    ""id"": ""8651d88f-db25-45c5-a7c0-a0fc47e3a21c"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Player1Shoot"",
                    ""id"": ""a4c057a6-3942-4c58-b25b-9bac71a2d961"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Player2Shoot"",
                    ""id"": ""1b42d30a-d819-480a-841b-ef903ff2c6e4"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""67b4e422-381c-4242-bf2d-96d40b0f2211"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9524686b-c0b0-446c-b844-4032a0e8fdfa"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""56eda515-6bc9-4441-95a6-4828e04046ab"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ce21e422-5aec-41bc-9179-b4021fafdd8b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3404b46b-c614-4945-ad25-c3fa6fd32680"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f745686f-c915-44e4-af1e-7ee948d05f3c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bde4886c-c29e-4509-80e3-3626fbd9135b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""36efce07-fb74-4990-bebc-27f1031e1e43"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6ed43886-a55f-45e5-9d11-7b002c51e890"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9157543a-fbee-4390-8af2-4774ec1c947b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player1Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""856b220b-d28d-416c-865b-c32d7b4e42db"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""013e6cf5-dca0-45ea-8b5b-3c2ab4f685b3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""29485c1a-dad5-412d-a568-94b0b0540d06"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""172541a8-c487-416f-b78f-8f57ee440c3f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""821ab0a1-57c9-4e11-8ca5-93519693b47a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""186b0f96-6803-4e14-a7e3-5fa81c7b7c18"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player2Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""5ae7d7ae-3ebc-4926-84c0-00cf86bfafc6"",
            ""actions"": [
                {
                    ""name"": ""MouseLeftButton"",
                    ""id"": ""040640d9-a15a-4fc8-b5e6-fad26e67581a"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""MousePosition"",
                    ""id"": ""d30992a9-56cc-4790-9013-fb59a5e1da07"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""65914105-10be-4cdf-b83d-10a4ae7b7b23"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""36ac26e1-f9b1-4f4f-967e-282d965765ac"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // InGame
        m_InGame = asset.GetActionMap("InGame");
        m_InGame_Player1Move = m_InGame.GetAction("Player1Move");
        m_InGame_Player2Move = m_InGame.GetAction("Player2Move");
        m_InGame_Player1Shoot = m_InGame.GetAction("Player1Shoot");
        m_InGame_Player2Shoot = m_InGame.GetAction("Player2Shoot");
        // UI
        m_UI = asset.GetActionMap("UI");
        m_UI_MouseLeftButton = m_UI.GetAction("MouseLeftButton");
        m_UI_MousePosition = m_UI.GetAction("MousePosition");
    }

    ~TanksControls()
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

    public ReadOnlyArray<InputControlScheme> controlSchemes
    {
        get => asset.controlSchemes;
    }

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

    // InGame
    private InputActionMap m_InGame;
    private IInGameActions m_InGameActionsCallbackInterface;
    private InputAction m_InGame_Player1Move;
    private InputAction m_InGame_Player2Move;
    private InputAction m_InGame_Player1Shoot;
    private InputAction m_InGame_Player2Shoot;
    public struct InGameActions
    {
        private TanksControls m_Wrapper;
        public InGameActions(TanksControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Player1Move { get { return m_Wrapper.m_InGame_Player1Move; } }
        public InputAction @Player2Move { get { return m_Wrapper.m_InGame_Player2Move; } }
        public InputAction @Player1Shoot { get { return m_Wrapper.m_InGame_Player1Shoot; } }
        public InputAction @Player2Shoot { get { return m_Wrapper.m_InGame_Player2Shoot; } }
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterface != null)
            {
                Player1Move.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Move;
                Player1Move.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Move;
                Player1Move.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Move;
                Player2Move.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Move;
                Player2Move.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Move;
                Player2Move.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Move;
                Player1Shoot.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Shoot;
                Player1Shoot.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Shoot;
                Player1Shoot.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer1Shoot;
                Player2Shoot.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Shoot;
                Player2Shoot.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Shoot;
                Player2Shoot.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPlayer2Shoot;
            }
            m_Wrapper.m_InGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                Player1Move.started += instance.OnPlayer1Move;
                Player1Move.performed += instance.OnPlayer1Move;
                Player1Move.canceled += instance.OnPlayer1Move;
                Player2Move.started += instance.OnPlayer2Move;
                Player2Move.performed += instance.OnPlayer2Move;
                Player2Move.canceled += instance.OnPlayer2Move;
                Player1Shoot.started += instance.OnPlayer1Shoot;
                Player1Shoot.performed += instance.OnPlayer1Shoot;
                Player1Shoot.canceled += instance.OnPlayer1Shoot;
                Player2Shoot.started += instance.OnPlayer2Shoot;
                Player2Shoot.performed += instance.OnPlayer2Shoot;
                Player2Shoot.canceled += instance.OnPlayer2Shoot;
            }
        }
    }
    public InGameActions @InGame
    {
        get
        {
            return new InGameActions(this);
        }
    }

    // UI
    private InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private InputAction m_UI_MouseLeftButton;
    private InputAction m_UI_MousePosition;
    public struct UIActions
    {
        private TanksControls m_Wrapper;
        public UIActions(TanksControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseLeftButton { get { return m_Wrapper.m_UI_MouseLeftButton; } }
        public InputAction @MousePosition { get { return m_Wrapper.m_UI_MousePosition; } }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                MouseLeftButton.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLeftButton;
                MouseLeftButton.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLeftButton;
                MouseLeftButton.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLeftButton;
                MousePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
                MousePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
                MousePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                MouseLeftButton.started += instance.OnMouseLeftButton;
                MouseLeftButton.performed += instance.OnMouseLeftButton;
                MouseLeftButton.canceled += instance.OnMouseLeftButton;
                MousePosition.started += instance.OnMousePosition;
                MousePosition.performed += instance.OnMousePosition;
                MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public UIActions @UI
    {
        get
        {
            return new UIActions(this);
        }
    }
    public interface IInGameActions
    {
        void OnPlayer1Move(InputAction.CallbackContext context);
        void OnPlayer2Move(InputAction.CallbackContext context);
        void OnPlayer1Shoot(InputAction.CallbackContext context);
        void OnPlayer2Shoot(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMouseLeftButton(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
