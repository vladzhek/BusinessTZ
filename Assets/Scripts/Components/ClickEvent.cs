using Leopotam.EcsLite;

namespace Components
{
    public struct ClickEvent
    {
        public ButtonType Type;
        public EcsPackedEntity Entity;
    }
}