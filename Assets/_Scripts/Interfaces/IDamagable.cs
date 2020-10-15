public interface IDamagable
{
    
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    
    // Damage resistance / threshold implementation?

    void Damage(int damageAmount);
}
