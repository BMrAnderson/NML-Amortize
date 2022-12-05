using Nml.Improve.Me.Dependencies;

namespace amortize.real.Mappers
{
    public static class PdfInReviewApplicationDocumentViewModelMapper
    {
        public static InReviewApplicationViewModel Map(string inReviewMessage,
                                                        double portfolioFundsTotal,
                                                        Application application,
                                                        IConfiguration configuration)
        {
            var viewModel       = new InReviewApplicationViewModel();
            var viewModelResult = PdfApplicationDocumentViewModelMapper.Map(viewModel, application, configuration);
            
            viewModelResult.LegalEntity          = PdfApplicationDocumentViewModelMapper.MapLegalEntity(application);
            viewModelResult.PortfolioFunds       = PdfApplicationDocumentViewModelMapper.MapFunds(application.Products);
            viewModelResult.PortfolioTotalAmount = portfolioFundsTotal;
            viewModelResult.InReviewMessage      = inReviewMessage;
            viewModelResult.InReviewInformation  = application.CurrentReview;

            return viewModelResult;
        }
    }
}