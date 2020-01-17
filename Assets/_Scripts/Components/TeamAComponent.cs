using Unity.Entities;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct TeamA : IComponentData
    {
        private int Value;
    }
}