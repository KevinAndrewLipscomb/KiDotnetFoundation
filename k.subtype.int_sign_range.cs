namespace kix
  {
  static public partial class k
    {
    public class int_sign_range
      {
      private subtype<int> current;
      public int_sign_range()
        {
        current = new subtype<int>(-1,1);
        }
      public int_sign_range(int val)
        {
        current = new subtype<int>(-1,1);
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
