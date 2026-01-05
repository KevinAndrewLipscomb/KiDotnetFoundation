namespace kix
  {
  static public partial class k
    {
    public class int_year_mysql
      {
      private subtype<int> current;
      public int_year_mysql()
        {
        current = new subtype<int>(1901,2155);
        }
      public int_year_mysql(int val)
        {
        current = new subtype<int>(1901,2155);
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
