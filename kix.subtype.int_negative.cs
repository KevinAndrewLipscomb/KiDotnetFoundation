namespace kix
  {
  static public partial class k
    {

    public class int_negative
      {
      public int_negative()
        {
        current = new subtype<int>(int.MinValue,-1);
        }
      public int_negative(int val)
        {
        current = new subtype<int>(int.MinValue,-1);
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
