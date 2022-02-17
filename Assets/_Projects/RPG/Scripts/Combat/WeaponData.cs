using UnityEngine;
using Sirenix.OdinInspector;
using Giezi.Tools;
using Enginooby.Audio;

namespace Project.RPG.Combat {
  [SOVariant]
  [CreateAssetMenu(fileName = "Weapon", menuName = "Project/RPG/Weapon Data", order = 0)]
  /// <summary>
  /// Invoke Init() to setup weapon data.
  /// </summary>
  public class WeaponData : ScriptableObject {
    [OnValueChanged(nameof(GetProjectileSpawner))]
    public GameObject Prefab;

    // TODO: add multiple random AnimatorOverrideController
    public AnimatorOverrideController AnimController;

    [Tooltip("Move to target and stop at this distance to attack.")]
    public float Range = 2;

    public int Damage = 1;

    [Tooltip("Delay between attacks - Opposite of rate.")] [Min(0.5f)]
    public float Cooldown = 1f;

    public bool IsRightHand = true;

    [SerializeField, InlineEditor] [LabelText("SFXs On Launch")]
    private SFXData _sfxsOnLaunch;

    [SerializeField, InlineEditor] [LabelText("SFXs On Hit")]
    private SFXData _sfxsOnHit; // ? Enable if HasProjectile

    [OnValueChanged(nameof(GetProjectileSpawner))]
    [InfoBox("If use projectile, make sure animation has OnShoot/Shoot/OnHit/Hit() event")]
    public bool HasProjectile;

    [ShowIf(nameof(HasProjectile))] [InlineEditor(InlineEditorModes.FullEditor)]
    // TODO: Validate if projectile spawner exist in prefab
    // this links to Spawner in prefab, hence changes auto update
    public Spawner ProjectileSpawner;

    public SFXData SfxsOnLaunch => _sfxsOnLaunch;
    public SFXData SfxsOnHit => _sfxsOnHit;

    private void GetProjectileSpawner() {
      if (HasProjectile) ProjectileSpawner = Prefab.GetComponentInChildren<Spawner>();
    }

    public GameObject Init(Transform weaponSlot, Animator animator) {
      GameObject weapon = null;
      if (Prefab) weapon = Instantiate(Prefab, parent: weaponSlot);

      var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
      if (AnimController) {
        animator.runtimeAnimatorController = AnimController;
      }
      else if (overrideController) {
        animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
      }

      return weapon;
    }
  }
}