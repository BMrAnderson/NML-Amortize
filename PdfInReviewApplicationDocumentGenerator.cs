using System.Collections.Generic;
using System.Linq;
using amortize.real.Mappers;
using amortize.real.Utilities;
using Nml.Improve.Me.Dependencies;

namespace Nml.Improve.Me
{
    public class PdfInReviewApplicationDocumentGenerator : PdfApplicationDocumentGenerator
    {
        private readonly IFundCalculator _fundCalculator;

        public PdfInReviewApplicationDocumentGenerator(IDataContext dataContext,
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
            var funds           = application.Products.SelectMany(f => f.Funds);
            var fundsTotal      = GetCalculatedFundsTotal(funds);
            var inReviewMessage = GetInReviewMessage(application.CurrentReview);

            var viewModel = PdfInReviewApplicationDocumentViewModelMapper.Map(inReviewMessage,
                                                                              fundsTotal,
                                                                              application,
                                                                              Configuration);
            return viewModel;
        }

        private string GetInReviewMessage(Review review)
        {
            const string prefixMessage         = "Your application has been placed in review";
            const string bankPostfixMessage    = "pending outstanding bank account verification.";
            const string addressPostfixMessage = "pending outstanding address verification for FICA purposes.";
            const string warningPostfixMessage = "because of suspicious account behaviour. Please contact support ASAP.";

            const string addressWord = "address";
            const string bankWord    = "bank";

            if (review.Reason.Contains(addressWord))
                return $"{prefixMessage} {bankPostfixMessage}";

            if (review.Reason.Contains(bankWord))
                return $"{prefixMessage} {addressPostfixMessage}";

            return $"{prefixMessage} {warningPostfixMessage}";
        }

        private double GetCalculatedFundsTotal(IEnumerable<Fund> funds)
        {
            var total = funds.Select(f => _fundCalculator.Calculate(f, Configuration.TaxRate)).Sum();

            return total;
        }
    }
}