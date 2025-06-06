using HitAndRun.Ads;
using UnityEngine;

namespace HitAndRun.Gui.Upgrade
{
    public class MbDamageUpgrade : MbUpgrade
    {
        protected override void HandleClick()
        {
            var data = MbGameManager.Instance.Data;
            if (_icon.sprite == _coinIcon)
            {
                var price = _priceBase * (int)(1 + data.Damage * _priceScale);
                if (data.Amount >= price)
                {
                    data.Damage++;
                    MbGameManager.Instance.Upgrade(data, price);
                }
            }
            else
            {
                MbRewardAds.Instance.ShowRewardedAd(() =>
                {
                    data.Damage++;
                    MbGameManager.Instance.Upgrade(data, 0);
                });
            }
        }

        protected override void UpdateUI(GameData data)
        {
            _curText.text = data.Damage.ToString();
            _nextText.text = (data.Damage + 1).ToString();

            var price = _priceBase * (int)(1 + data.Damage * _priceScale);

            if (data.Amount >= price)
            {
                _icon.sprite = _coinIcon;
                _price.text = FormatNumber(price);
            }
            else
            {
                _icon.sprite = _ytIcon;
                _price.text = "Free";
            }
        }
    }

}
