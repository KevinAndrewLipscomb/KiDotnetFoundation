namespace kix
  {
  static public partial class k
    {

    public class int_month
      {
      public int_month()
        {
        current = new subtype<int>(1,12);
        }
      public int_month(int val)
        {
        current = new subtype<int>(1,12);
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
