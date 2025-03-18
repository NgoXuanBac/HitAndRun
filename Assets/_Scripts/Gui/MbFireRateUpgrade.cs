using UnityEngine;

namespace HitAndRun.Gui
{
    public class MbFireRateUpgrade : MbUpgrade
    {
        protected override void UpdateUI(SaveData data)
        {
            _curText.text = data.FireRate.ToString();
            _nextText.text = (data.FireRate + 1).ToString();
        }
    }
}