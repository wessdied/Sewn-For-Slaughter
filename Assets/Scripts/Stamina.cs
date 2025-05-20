using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }

    public Stamina(float maxStamina)
    {
        MaxStamina = maxStamina;
        CurrentStamina = MaxStamina;
    }

    public void DecreaseStamina(float amount)
    {
        CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
    }

    public void IncreaseStamina(float amount)
    {
        CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
    }
}
