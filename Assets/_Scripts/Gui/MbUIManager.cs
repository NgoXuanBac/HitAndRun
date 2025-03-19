using System;
using System.Collections.Generic;
using System.Linq;
using HitAndRun.Gui.Popup;
using UnityEngine;

namespace HitAndRun.Gui
{
    public class MbUIManager : MbSingleton<MbUIManager>
    {
        [SerializeField] private List<MbPopup> _popups;
        [SerializeField] private Dictionary<Type, MbPopup> _popupsByType = new();
        private void Reset()
        {
            _popups = FindObjectsOfType<MbPopup>(true).ToList();
        }

        private void Awake()
        {
            foreach (var popup in _popups)
            {
                var type = popup.GetType();
                if (!_popupsByType.ContainsKey(type))
                {
                    _popupsByType.Add(type, popup);
                }
            }
        }

        public MbPopup ShowPopup<T>() where T : MbPopup
        {
            var popup = _popupsByType[typeof(T)];
            popup.ShowPopup();
            return popup;
        }
    }

}
