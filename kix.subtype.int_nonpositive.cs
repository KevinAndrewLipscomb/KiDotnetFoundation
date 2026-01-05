namespace kix
  {
  static public partial class k
    {

    public class int_nonpositive
      {
      public int_nonpositive()
        {
        current = new subtype<int>(int.MinValue,0);
        }
      public int_nonpositive(int val)
        {
        current = new subtype<int>(int.MinValue,0);
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
      private subtype<int> current;
      }

    }
  }
