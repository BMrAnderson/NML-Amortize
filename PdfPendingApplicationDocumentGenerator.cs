using amortize.real.Mappers;
using Nml.Improve.Me.Dependencies;

namespace Nml.Improve.Me
{
    public class PdfPendingApplicationDocumentGenerator : PdfApplicationDocumentGenerator
    {
        public PdfPendingApplicationDocumentGenerator(IDataContext dataContext,
                                                      IPathProvider templatePathProvider,
                                                      IViewGenerator viewGenerator,
                                                      IConfiguration configuration,
                                                      IPdfGenerator pdfGenerator,
                                                      ILogger<PdfApplicationDocumentGenerator> logger) :
            base(dataContext, 
                 templatePathProvider, 
                 viewGenerator, 
                 configuration, 
                 pdfGenerator, 
                 logger)
        {
        }

        protected override object GetViewModelFromApplication(Application application)
        {
            var viewModel = PdfPendingApplicationDocumentViewModelMapper.Map(application, Configuration);
            return viewModel;
        }
    }
}