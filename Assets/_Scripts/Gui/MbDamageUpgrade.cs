namespace HitAndRun.Gui
{
    public class MbDamageUpgrade : MbUpgrade
    {
        protected override void UpdateUI(SaveData data)
        {
            _curText.text = data.Damage.ToString();
            _nextText.text = (data.Damage + 1).ToString();
        }
    }

}
