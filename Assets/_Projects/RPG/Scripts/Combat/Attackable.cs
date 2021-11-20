using UnityEngine;

public abstract class Attackable : MonoBehaviour {
  public abstract void GetAttacked(Attacker attacker, int damage);
  public abstract void Die();
}