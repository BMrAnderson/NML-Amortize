using System;
using System.Linq;
using amortize.real.Constants;
using Nml.Improve.Me.Dependencies;

namespace Nml.Improve.Me
{
    public abstract class PdfApplicationDocumentGenerator : IApplicationDocumentGenerator
    {
        private readonly IDataContext                             _dataContext;
        private readonly IPathProvider                            _templatePathProvider;
        private readonly IViewGenerator                           _viewGenerator;
        private readonly ILogger<PdfApplicationDocumentGenerator> _logger;
        private readonly IPdfGenerator                            _pdfGenerator;

        protected readonly IConfiguration Configuration;

        protected PdfApplicationDocumentGenerator(IDataContext dataContext,
                                                  IPathProvider templatePathProvider,
                                                  IViewGenerator viewGenerator,
                                                  IConfiguration configuration,
                                                  IPdfGenerator pdfGenerator,
                                                  ILogger<PdfApplicationDocumentGenerator> logger)
        {
            _dataContext          = dataContext;
            _templatePathProvider = templatePathProvider;
            _viewGenerator        = viewGenerator;
            Configuration         = configuration;
            _logger               = logger;
            _pdfGenerator         = pdfGenerator;
        }

        protected abstract object GetViewModelFromApplication(Application application);

        private string GetTargetForTemplatePath(ApplicationState applicationState)
        {
            switch (applicationState)
            {
                case ApplicationState.Pending:
                    return TemplatePathTargetConstants.PendingApplication;
                case ApplicationState.Activated:
                    return TemplatePathTargetConstants.ActivatedApplication;
                case ApplicationState.InReview:
                    return TemplatePathTargetConstants.InReviewApplication;
                default:
                    _logger.LogWarning($"The application is in state '{applicationState}' and no valid document can be generated for it.");
                    return null;
            }
        }

        private PdfOptions GetPdfSettings()
        {
            var pdfOptions = new PdfOptions
            {
                PageNumbers = PageNumbers.Numeric,
                HeaderOptions = new HeaderOptions
                {
                    HeaderRepeat = HeaderRepeat.FirstPageOnly,
                    HeaderHtml   = PdfConstants.Header
                }
            };
            return pdfOptions;
        }

        public byte[] Generate(Guid applicationId, string baseUri)
        {
            var application = _dataContext.Applications.SingleOrDefault(app => app.Id == applicationId);
            if (application == null)
            {
                _logger.LogWarning($"No application found for id '{applicationId}'");
                return Array.Empty<byte>();
            }
            if (baseUri.EndsWith("/"))
                baseUri = baseUri.Substring(baseUri.Length - 1);

            var target       = GetTargetForTemplatePath(application.State);
            var templatePath = _templatePathProvider.Get(target);
            var viewModel    = GetViewModelFromApplication(application);
            var view         = _viewGenerator.GenerateFromPath($"{baseUri} {templatePath}", viewModel);
           
            var pdfOptions   = GetPdfSettings();
            var pdf          = _pdfGenerator.GenerateFromHtml(view, pdfOptions);
            
            return pdf.ToBytes();
        }
    }
}