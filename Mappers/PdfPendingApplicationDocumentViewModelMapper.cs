using Nml.Improve.Me.Dependencies;

namespace amortize.real.Mappers
{
    public static class PdfPendingApplicationDocumentViewModelMapper
    {
        public static PendingApplicationViewModel Map(Application application, IConfiguration configuration)
        {
            var viewModel       = new PendingApplicationViewModel();
            var viewModelResult = PdfApplicationDocumentViewModelMapper.Map(viewModel, application, configuration);
            return viewModelResult;
        }
    }
}