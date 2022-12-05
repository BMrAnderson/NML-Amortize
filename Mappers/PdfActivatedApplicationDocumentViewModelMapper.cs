using Nml.Improve.Me;
using Nml.Improve.Me.Dependencies;

namespace amortize.real.Mappers
{
    public static class PdfActivatedApplicationDocumentViewModelMapper
    {
        public static ActivatedApplicationViewModel Map(double portfolioFundsTotal,
                                                        Application application,
                                                        IConfiguration configuration)
        {
            var viewModel       = new ActivatedApplicationViewModel();
            var viewModelResult = PdfApplicationDocumentViewModelMapper.Map(viewModel, application, configuration);
            
            viewModelResult.LegalEntity          = PdfApplicationDocumentViewModelMapper.MapLegalEntity(application);
            viewModelResult.PortfolioFunds       = PdfApplicationDocumentViewModelMapper.MapFunds(application.Products);
            viewModelResult.PortfolioTotalAmount = portfolioFundsTotal;

            return viewModelResult;
        }
    }
}