namespace Assets.Scripts.PlayerManagement
{
  public enum PadUsedType
  {
    SINGLE,
    DUAL_LEFT,
    DUAL_RIGHT

  }

  public class PadUsedTypeUtils
  {
    public static PadUsedType GetOtherSide(PadUsedType side)
    {
      switch (side)
      {
        case PadUsedType.DUAL_LEFT :
          return PadUsedType.DUAL_RIGHT;
        case PadUsedType.DUAL_RIGHT :
          return PadUsedType.DUAL_LEFT;
        default: return PadUsedType.SINGLE;
      }
    }
  }
}