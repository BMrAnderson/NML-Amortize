using Nml.Improve.Me;
using Nml.Improve.Me.Dependencies;

namespace amortize.real.Utilities
{
    public class FundCalculator : IFundCalculator
    {
        public double Calculate(Fund fund, double taxRate)
        {
            return (fund.Amount - fund.Fees) * taxRate;
        }
    }
}