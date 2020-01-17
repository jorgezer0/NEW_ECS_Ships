using Unity.Entities;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct TeamB : IComponentData
    {
        private int Value;
    }
}