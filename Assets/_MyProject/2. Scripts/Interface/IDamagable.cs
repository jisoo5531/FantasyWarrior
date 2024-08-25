public interface IDamagable
{
    int MaxHp { get; set; }
    int Hp { get; set; }

    void GetDamage(int damage);
    void Death();
}