namespace kix
  {
  static public partial class k
    {
    public class int_nonnegative
      {
      private subtype<int> current;
      public int_nonnegative()
        {
        current = new subtype<int>(0,int.MaxValue);
        }
      public int_nonnegative(int val)
        {
        current = new subtype<int>(0,int.MaxValue);
        current.val = val;
        }
      public int val
        {
        get
          {
          return current.val;
          }
        set
          {
          current.val = value;
          }
        }
      }

    }
  }
