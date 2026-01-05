namespace kix
  {
  static public partial class k
    {
    //--
    //
    // Classes based on instatiations of generic struct 'subtype'
    //
    //--

    public class int_month
      {
      private subtype<int> current;
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
      }

    }
  }
