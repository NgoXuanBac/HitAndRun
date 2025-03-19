namespace HitAndRun.Gui.Upgrade
{
    public class MbDamageUpgrade : MbUpgrade
    {
        protected override void UpdateUI(GameData data)
        {
            _curText.text = data.Damage.ToString();
            _nextText.text = (data.Damage + 1).ToString();
        }
    }

}
