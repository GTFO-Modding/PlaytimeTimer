using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using PlaytimeDisplay.Information;
using GameData;
using System.Linq;
using PlaytimeDisplay.Manager;

namespace PlaytimeDisplay.Scripts
{
    public class Playtime : MonoBehaviour
    {
        private const string _DataFile = "Playtime.json";

        private static TextMeshPro _currentTextMesh;
        private static TextMeshPro _realTimeTextMesh;
        private static TextMeshPro _lifeTimeTextMesh;

        private PlaytimeData _lifeTimeWasted;

        private float _nextUpdate = 0f;
        private TimeSpan _lifetimeSpan;
        private DateTime _timeOnStart = DateTime.Now;

        public Playtime(IntPtr intPtr) : base(intPtr)
        { }

        public void Awake()
        {
            Data = EndskApi.Api.ProfileIndependentDataApi.Load<List<PlaytimeData>>(_DataFile);
            StartPlaytime();
        }

        public void OnDestroy()
        {
            LogManager.Message("OnDestroy");
            SavePlaytime();
        }

        /// <summary>
        /// Gets or sets the profile independent data of Playtime.
        /// Loading it is reading out of the cache, while setting it updates the cache and 
        /// </summary>
        public List<PlaytimeData> Data { get; set; }

        public void StartPlaytime()
        {
            Init();

            var currentMission = RundownManager.ActiveExpedition;
            var currentRundown = RundownDataBlock.GetBlock(1);

            _lifeTimeWasted = Data.FirstOrDefault(it => it.RundownName == currentRundown.name);
            if(_lifeTimeWasted is null)
            {
                _lifeTimeWasted = new PlaytimeData(currentRundown.name, (ulong)Time.time);
                Data.Add(_lifeTimeWasted);
            }
        }

        public void Update()
        {
            if(_nextUpdate < Time.time)
            {
                _currentTextMesh.m_text = $"Mission Time: {TimeSpan.FromSeconds(Clock.ExpeditionProgressionTime).ToString("h\\:mm\\:ss")}";

                _lifetimeSpan = DateTime.Now - _timeOnStart;
                _realTimeTextMesh.m_text = $"Play Time: {_lifetimeSpan.ToString("h\\:mm\\:ss")}";
                _lifetimeSpan = _lifetimeSpan.Add(TimeSpan.FromSeconds(_lifeTimeWasted.WastedSeconds));
                //I can already see the people, having more than 24 hours in a specific rundown ... So Math.Floor is better here
                _lifeTimeTextMesh.m_text = $"Rundown Time: {Math.Floor(_lifetimeSpan.TotalHours)}:{_lifetimeSpan.ToString("mm\\:ss")}";

                _currentTextMesh.ForceMeshUpdate();
                _realTimeTextMesh.ForceMeshUpdate();
                _lifeTimeTextMesh.ForceMeshUpdate();

                _nextUpdate = Time.time + 1f;
            }
        }

        internal void SavePlaytime()
        {
            _lifeTimeWasted.WastedSeconds = _lifeTimeWasted.WastedSeconds + (ulong)(DateTime.Now - _timeOnStart).TotalSeconds;
            EndskApi.Api.ProfileIndependentDataApi.Save(Data, _DataFile);
        }

        private void Init()
        {
            if(_currentTextMesh != null)
            {
                return;
            }

                var inventory = GuiManager.Current.m_playerLayer.Inventory;
            var icon = inventory.m_iconDisplay;

            foreach (var sprite in icon.GetComponentsInChildren<RectTransform>(true))
            {
                if (sprite.name == "Background Fade")
                {
                    sprite.gameObject.SetActive(true);
                    _currentTextMesh = null;
                    //Getting any 
                    foreach (var item in inventory.m_inventorySlots)
                    {
                        if (_currentTextMesh is null)
                        {
                            _currentTextMesh = UnityEngine.Object.Instantiate(item.Value.m_slim_archetypeName);
                            break;
                        }
                    }

                    var obj = new GameObject("TimerShowObject")
                    {
                        layer = 5,
                        hideFlags = HideFlags.HideAndDontSave
                    };

                    obj.transform.SetParent(sprite.transform, false);
                    _currentTextMesh.transform.SetParent(obj.transform, false);
                    //Normal Inventory Name Objects are having the right AmmoCount in the same Parent, so the Rect is completely wrong here
                    var rect = _currentTextMesh.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(-5f, 8f);

                    _currentTextMesh.SetText("EndskillTimer");
                    _currentTextMesh.ForceMeshUpdate();

                    var rect2 = UnityEngine.Object.Instantiate(sprite, sprite.parent);
                    rect2.transform.localPosition -= new Vector3(0f, 30f, 0f);
                    _realTimeTextMesh = rect2.GetComponentInChildren<TextMeshPro>();
                    _realTimeTextMesh.text = "Realtime wasted";
                    _realTimeTextMesh.ForceMeshUpdate();

                    var rect3 = UnityEngine.Object.Instantiate(sprite, sprite.parent);
                    rect3.transform.localPosition -= new Vector3(0f, 60f, 0f);
                    _lifeTimeTextMesh = rect3.GetComponentInChildren<TextMeshPro>();
                    _lifeTimeTextMesh.text = "Lifetime wasted";
                    _lifeTimeTextMesh.ForceMeshUpdate();
                }
            }
        }
    }
}
