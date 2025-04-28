using Leopotam.EcsLite;

namespace Extensions
{
    public static class EcsExtensions
    {
        public static int FirstOrDefault(this EcsFilter filter)
        {
            foreach (var entity in filter)
                return entity;
            return -1;
        }
    }
}