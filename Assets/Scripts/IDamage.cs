/// <summary>
/// Interface for a GameObject that deals damage to other GameObjects.
/// </summary>
public interface IDamage {
    /// <summary>How much Damage this GameObject inflicts on its target.</summary>
    int Damage { get; }
}