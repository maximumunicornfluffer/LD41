using System.Collections.Generic;
using System.Linq;
using Plugins.Utils.CustomEventMethods;
using UnityEngine;

namespace Plugins.GameUtils.Camera.BarycenterTargetCamera
{
  public class BaryCameraController : MonoBehaviour, ILateUpdate
  {
    [SerializeField]
    private Vector3 _offset;
    
    private List<ICameraTarget> _targets;
    public void SetPlayerToFollow(List<ICameraTarget> targets)
    {
      _targets = targets;
    }

    public void CustomLateUpdate()
    {
      if (_targets != null)
      {
        var sum = _targets.Aggregate(new Vector2(),
                    (a, b) => new Vector2(a.x + b.Weight * b.transform.position.x,
                      a.y + b.Weight * b.transform.position.y)) / _targets.Count;

        transform.position = new Vector3(sum.x, sum.y, -10)+_offset;
      }
    }
  }
  
}