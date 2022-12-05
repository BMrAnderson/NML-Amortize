using System.Collections.Generic;
using System.Linq;
using amortize.real.Mappers;
using amortize.real.Utilities;
using Nml.Improve.Me.Dependencies;

namespace Nml.Improve.Me
{
    public sealed class PdfActivatedApplicationDocumentGenerator : PdfApplicationDocumentGenerator
    {
        private readonly IFundCalculator _fundCalculator;
        
        public PdfActivatedApplicationDocumentGenerator(IDataContext dataContext, 
                                                        IPathProvider templatePathProvider,
                                                        IViewGenerator viewGenerator, 
                                                        IConfiguration configuration,
                                                        IPdfGenerator pdfGenerator,
                                                        ILogger<PdfApplicationDocumentGenerator> logger,
                                                        IFundCalculator fundCalculator) :
            base(dataContext, 
                 templatePathProvider,
                 viewGenerator, 
                 configuration, 
                 pdfGenerator, 
                 logger)
        {
            _fundCalculator = fundCalculator;
        }

        protected override object GetViewModelFromApplication(Application application)
        {
            var funds     = application.Products.SelectMany(f => f.Funds);
            var total     = GetCalculatedFundsTotal(funds);
            var viewModel = PdfActivatedApplicationDocumentViewModelMapper.Map(total, application, Configuration);

            return viewModel;
        }

        private double GetCalculatedFundsTotal(IEnumerable<Fund> funds)
        {
            var total = funds.Select(f => _fundCalculator.Calculate(f, Configuration.TaxRate)).Sum();
            return total;
        }
    }
}