using Unity.Entities;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct CameraComponent : IComponentData
    {
        private int Value;
    }
}