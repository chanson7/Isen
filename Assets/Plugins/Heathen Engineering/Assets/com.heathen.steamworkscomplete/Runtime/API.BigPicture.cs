#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using HeathenEngineering.Events;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration.API
{
    public static class BigPicture
    {
        public static class Client
        {
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void Init()
            {
                m_GamepadTextInputDismissed_t = null;
                
                eventGamepadTextIputDismissed = null;
            }

            /// <summary>
            /// Invoked when Show Text Input is successfuly called.
            /// </summary>
            public static UnityEvent EventGamepadTextInputShown => eventGamepadTextInputShown;
            /// <summary>
            /// Invoked when the gamepad text input is dismissed and returns the resulting input string.
            /// </summary>
            public static UnityStringEvent EventGamepadTextIputDismissed
            {
                get
                {
                    if (m_GamepadTextInputDismissed_t == null)
                        m_GamepadTextInputDismissed_t = Callback<GamepadTextInputDismissed_t>.Create((r) =>
                        {
                            SteamUtils.GetEnteredGamepadTextInput(out string textValue, SteamUtils.GetEnteredGamepadTextLength());
                            eventGamepadTextIputDismissed.Invoke(textValue);
                        });

                    return eventGamepadTextIputDismissed;
                }
            }

            private static UnityStringEvent eventGamepadTextIputDismissed = new UnityStringEvent();
            private static UnityEvent eventGamepadTextInputShown = new UnityEvent();

#pragma warning disable IDE0052 // Remove unread private members
            private static Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed_t;
#pragma warning restore IDE0052 // Remove unread private members

            public static bool InBigPicture => SteamUtils.IsSteamInBigPictureMode();
            public static bool RunningOnDeck => SteamUtils.IsSteamRunningOnSteamDeck();

            /// <summary>
            /// Activates the Big Picture text input dialog which only supports gamepad input.
            /// </summary>
            /// <param name="inputMode">Selects the input mode to use, either Normal or Password (hidden text)</param>
            /// <param name="lineMode">Controls whether to use single or multi line input.</param>
            /// <param name="description">Sets the description that should inform the user what the input dialog is for.</param>
            /// <param name="maxLength">The maximum number of characters that the user can input.</param>
            /// <param name="currentText">Sets the preexisting text which the user can edit.</param>
            /// <returns>True if the big picture overlay is running; otherwise, false.</returns>
            public static bool ShowTextInput(EGamepadTextInputMode inputMode, EGamepadTextInputLineMode lineMode, string description, uint maxLength, string currentText)
            {
                if (SteamUtils.ShowGamepadTextInput(inputMode, lineMode, description, maxLength, currentText))
                {
                    eventGamepadTextInputShown.Invoke();
                    return true;
                }
                else
                    return false;
            }

            /// <summary>
            /// Activates the Big Picture text input dialog which only supports gamepad input.
            /// </summary>
            /// <param name="inputMode">Selects the input mode to use, either Normal or Password (hidden text)</param>
            /// <param name="lineMode">Controls whether to use single or multi line input.</param>
            /// <param name="description">Sets the description that should inform the user what the input dialog is for.</param>
            /// <param name="maxLength">The maximum number of characters that the user can input.</param>
            /// <param name="currentText">Sets the preexisting text which the user can edit.</param>
            /// <returns>True if the big picture overlay is running; otherwise, false.</returns>
            public static bool ShowTextInput(EGamepadTextInputMode inputMode, EGamepadTextInputLineMode lineMode, string description, int maxLength, string currentText)
            {
                if(SteamUtils.ShowGamepadTextInput(inputMode, lineMode, description, System.Convert.ToUInt32(maxLength), currentText))
                {
                    eventGamepadTextInputShown.Invoke();
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
#endif