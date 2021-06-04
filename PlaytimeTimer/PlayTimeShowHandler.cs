using HarmonyLib;
using System;
using System.Threading;
using UnityEngine;

namespace PlaytimeTimer
{
    //public class PlayTimeShowHandler : MonoBehaviour
    //{
    //    public const string _BEFORE_TIME = "<#f00>";
    //    public const string _AFTER_TIME = "<size=0>";
    //    public const string _TIMER_STARTCOMMAND = "/timer";
    //    string test = "<#f00>132:64<size=0>";

    //    private Timer _intervalTimer;
    //    private bool _update;
    //    private bool _active;
    //    private PUI_GameEventLog _gameEventLogger;

    //    public PlayTimeShowHandler(IntPtr intPtr) : base(intPtr)
    //    { }

    //    public void Awake()
    //    {
    //        Current = this;
    //        if (_intervalTimer is null)
    //        {
    //            _intervalTimer = new Timer((arg) => { _update = true; }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    //        }
    //    }

    //    public static PlayTimeShowHandler Current { get; private set; }

    //    public void Update()
    //    {
    //        if (_active)
    //        {
    //            if (_update)
    //            {
    //                _update = false;
    //                PUI_GameEventLog_Item chatMessage = null;
    //                string cutChatMessage = null;

    //                foreach (var item in GuiManager.Current.m_playerLayer.m_gameEventLog.m_logItems)
    //                {
    //                    if (chatMessage is null)
    //                    {
    //                        if (item.m_text.m_text.EndsWith(_AFTER_TIME) && item.m_text.m_text.Contains(_BEFORE_TIME))
    //                        {
    //                            chatMessage = item;
    //                            break;
    //                        }
    //                        else if (item.m_text.m_text.EndsWith(_TIMER_STARTCOMMAND))
    //                        {
    //                            chatMessage = item;
    //                            cutChatMessage = chatMessage.m_text.m_text.Substring(0, chatMessage.m_text.m_text.IndexOf(_TIMER_STARTCOMMAND));
    //                            break;
    //                        }
    //                    }
    //                }

    //                if (chatMessage != null)
    //                {
    //                    if (string.IsNullOrEmpty(cutChatMessage))
    //                    {
    //                        cutChatMessage = chatMessage.m_text.m_text.Substring(0, chatMessage.m_text.m_text.IndexOf(_BEFORE_TIME));
    //                    }
    //                }
    //                else
    //                {
    //                    //TODO Message got overriden in the Chat
    //                    _active = false;
    //                }
    //            }
    //        }
    //    }

    //    public void ChatMessageReceived(string msg)
    //    {
    //        if (msg.ToLower().EndsWith(_TIMER_STARTCOMMAND))
    //        {
    //            _active = true;
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(PUI_GameEventLog), nameof(PUI_GameEventLog._Setup_b__22_2))]
    //public class PlayerSendsChatMessagePatch
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix(ref string msg)
    //    {
    //        //PlayTimeShowHandler.Current.ChatMessageReceived(msg);
    //    }
    //}
}
