using HarmonyLib;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace PlaytimeTimer
{
    public class TimeShowHandler : MonoBehaviour
    {
        private const int _SECONDSDAY_ = 60 * 60 * 24;
        private const int _SECONDSHOUR_ = 60 * 60;
        private const int _SECONDSMINUTE_ = 60;

        private Timer _intervalTimer;
        private bool _update;
        private bool _endskill;

        public TimeShowHandler(IntPtr intPtr) : base(intPtr)
        { }

        public void Awake()
        {
            //PlayTimeLogger.Debug("Awake Method got called");

            _endskill = BepInExLoader.EndskillTime.Value;
            Current = this;
            if (_intervalTimer is null)
            {
                _intervalTimer = new Timer((arg) => { _update = true; }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
            }

            var inventory = GuiManager.Current.m_playerLayer.Inventory;
            var icon = inventory.m_iconDisplay;

            foreach (var sprite in icon.GetComponentsInChildren<RectTransform>(true))
            {
                if (sprite.name == "Background Fade")
                {
                    sprite.gameObject.SetActive(true);
                    TextMesh = null;
                    //Getting any 
                    foreach (var item in inventory.m_inventorySlots)
                    {
                        if (TextMesh is null)
                        {
                            TextMesh = Instantiate(item.Value.m_slim_archetypeName);
                            break;
                        }
                    }

                    var obj = new GameObject("TimerShowObject")
                    {
                        layer = 5,
                        hideFlags = HideFlags.HideAndDontSave
                    };

                    obj.transform.SetParent(sprite.transform, false);
                    TextMesh.transform.SetParent(obj.transform, false);
                    //Normal Inventory Name Objects are having the right AmmoCount in the same Parent, so the Rect is completely wrong here
                    var rect = TextMesh.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(-5f, 8f);

                    TextMesh.SetText(_endskill ? "EndskillTimer" : "NotEndskillTimer");
                    TextMesh.ForceMeshUpdate();
                }
            }
        }

        public static TimeShowHandler Current { get; private set; }

        /// <summary>
        /// Gets or Sets the TextMeshPro Object that is visible for the user
        /// </summary>
        public TextMeshPro TextMesh { get; set; }

        public void Update()
        {
            if (_update)
            {
                _update = false;
                TextMesh.SetText("Time: " + GetTimeStringFormatted());
                TextMesh.ForceMeshUpdate();
            }
        }

        public string GetTimeStringFormatted()
        {
            if(!_endskill)
            {
                return TimeSpan.FromSeconds((double)Clock.ExpeditionProgressionTime).ToString("hh':'mm':'ss");
            }
            else
            {
                //very Random TimeCalculations OwO
                var seconds = (int)Clock.ExpeditionProgressionTime - 9;
                var minutes = 0;
                var hours = 0;
                int days;

                if (seconds > (_SECONDSDAY_))
                {
                    days = seconds / _SECONDSDAY_;
                    seconds = seconds - (days * _SECONDSDAY_);
                }

                if (seconds > (_SECONDSHOUR_))
                {
                    hours = seconds / (_SECONDSHOUR_);
                    seconds = seconds - (hours * _SECONDSHOUR_);
                }

                if (seconds > _SECONDSMINUTE_)
                {
                    minutes = seconds / _SECONDSMINUTE_;
                    seconds = seconds - (minutes * _SECONDSMINUTE_);
                }

                seconds += 9;
                return $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
            }
        }
    }

    [HarmonyPatch(typeof(GS_InLevel), nameof(GS_InLevel.Enter))]
    public class PrepareInjection
    {
        private static bool _isInjected = false;

        [HarmonyPostfix]
        public static void PostFix()
        {
            //PlayTimeLogger.Debug("UpdateAllSlots Patch got called");
            //PlayTimeLogger.Debug(GameStateManager.CurrentStateName.ToString());
            if (!_isInjected)
            {
                _isInjected = true;

                var obj = new GameObject("InMissionTimer TextUpdater");
                obj.AddComponent<TimeShowHandler>();
                UnityEngine.Object.DontDestroyOnLoad(obj);
            }
        }
    }
}
