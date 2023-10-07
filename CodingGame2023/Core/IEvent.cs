using System.ComponentModel.DataAnnotations;

namespace Core
{
    public interface IEvent
    {
        string Name { get; }
        Key Id { get; }
    }
}