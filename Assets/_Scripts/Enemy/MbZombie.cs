using HitAndRun.Character;

namespace HitAndRun.Enemy
{
    public sealed class MbZombie : MbEnemy
    {
        public override void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        protected override void Attack(MbCharacter character)
        {
        }
    }

}