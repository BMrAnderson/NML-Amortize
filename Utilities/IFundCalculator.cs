using Nml.Improve.Me.Dependencies;

namespace amortize.real.Utilities
{
    public interface IFundCalculator
    {
        double Calculate(Fund fund, double taxRate);
    }
}