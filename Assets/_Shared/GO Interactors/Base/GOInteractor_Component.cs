using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// </summary>
public abstract class GOInteractor<TSelf, TComponent> : GOInteractor<TSelf>
where TSelf : GOInteractor<TSelf, TComponent>
where TComponent : Component {
  protected new Dictionary<GameObject, TComponent> _interactedGos = new Dictionary<GameObject, TComponent>();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
}
