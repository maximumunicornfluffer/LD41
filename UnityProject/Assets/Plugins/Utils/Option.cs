namespace Plugins.Utils
{
  public class Option<T>
  {
    public static Option<T> of(T obj)
    {
      return new Option<T>(obj);
    }
    
    private readonly T _object;

    private Option(T o)
    {
      _object = o;
    }

    public bool IsPresent
    {
      get { return _object != null; }
    }

    public T Get
    {
      get { return _object; }
    }
  }
}