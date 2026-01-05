namespace kix
  {
  static public partial class k
    {

    public class int_positive
      {
      public int_positive()
        {
        current = new subtype<int>(1,int.MaxValue);
        }
      public int_positive(int val)
        {
        current = new subtype<int>(1,int.MaxValue);
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
