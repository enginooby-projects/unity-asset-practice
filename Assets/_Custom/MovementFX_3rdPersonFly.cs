/// <summary>
/// Add FXs for "3rd Person Fly" asset.
/// </summary>
public class MovementFX_3rdPersonFly : MovementFX {
  private FlyBehaviour _flyBehaviour;

  // TODO: if character is not blocked/collided (hence not moving even while triggering key)
  // Specify by velocity of rigidbody instead
  public override bool IsFlying => _flyBehaviour.IsFlying && _flyKey.IsTriggering && IsRigidBodyMoving();
  public override bool IsSprintFlying => _flyBehaviour.IsFlying && _sprintFlyKey.IsTriggering && IsRigidBodyMoving();

  protected override void Awake() {
    base.Awake();
    _flyBehaviour = GetComponent<FlyBehaviour>();
  }
}
