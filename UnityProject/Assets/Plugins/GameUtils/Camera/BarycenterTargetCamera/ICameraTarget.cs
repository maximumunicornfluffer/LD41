using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.GameUtils.Camera.BarycenterTargetCamera
{
	public interface ICameraTarget
	{
		float Weight { get; set; }
		Transform transform { get; }
	}
}
