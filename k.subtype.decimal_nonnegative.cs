namespace kix
  {
  static public partial class k
    {
    public class decimal_nonnegative
      {
      private subtype<decimal> current;
      public decimal_nonnegative()
        {
        current = new subtype<decimal>(0,decimal.MaxValue);
        }
      public decimal_nonnegative(decimal val)
        {
        current = new subtype<decimal>(0,decimal.MaxValue);
        current.val = val;
        }
      public decimal val
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
