/// <summary>
/// Add FXs for "3rd Person Fly" asset.
/// </summary>
public class MovementFX_3rdPersonFly : MovementFX {
  private FlyBehaviour _flyBehaviour;

  public override bool IsFlying => _flyBehaviour.IsFlying && _flyKey.IsTriggering && IsRigidBodyMoving();
  public override bool IsSprintFlying => _flyBehaviour.IsFlying && _sprintFlyKey.IsTriggering && IsRigidBodyMoving();

  protected override void Awake() {
    base.Awake();
    _flyBehaviour = GetComponent<FlyBehaviour>();
  }
}
