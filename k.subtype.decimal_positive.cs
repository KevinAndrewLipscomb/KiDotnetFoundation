namespace kix
  {
  static public partial class k
    {
    public class decimal_positive
      {
      private subtype<decimal> current;
      public decimal_positive()
        {
        current = new subtype<decimal>(1,decimal.MaxValue);
        }
      public decimal_positive(decimal val)
        {
        current = new subtype<decimal>(1,decimal.MaxValue);
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
