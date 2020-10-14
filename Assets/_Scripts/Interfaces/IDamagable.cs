using UnityEngine;

public interface IDamagable
{
    
    // CurrentHealth and MaxHealth?
    int Health { get; set; }
    
    // Damage resistance / threshold implementation?

    void Damage();
}
