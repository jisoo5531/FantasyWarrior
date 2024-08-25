public interface IAttackable
{
    int Damage { get; set; }

    void SendDamage(int damage);
}
