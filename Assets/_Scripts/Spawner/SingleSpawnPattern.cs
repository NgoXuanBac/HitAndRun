namespace HitAndRun.Spawner
{
    public class SingleSpawnPattern : SpawnPattern
    {
        public override float[] GetSpawnPosX()
        {
            return new float[] { -0.5f, -1, 0, 0.5f, 1 };
        }
    }
}

