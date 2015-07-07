
public interface  Attackable 
{
    float health { get; set; }
    float maxhealth { get; set; }
    float maxCombo { get; set; }
    bool IsAttacking { get; set; }
    int actualCombo { get; set; }

    void Kill();
    void Hit();
    void Hurt(float d);
    void Health(float h);
}
