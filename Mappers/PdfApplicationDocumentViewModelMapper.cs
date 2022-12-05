using System.Collections.Generic;
using System.Linq;
using Nml.Improve.Me.Dependencies;

namespace amortize.real.Mappers
{
    public static class PdfApplicationDocumentViewModelMapper
    {
        public static T Map<T>(T viewModel, Application application, IConfiguration configuration) where T : ApplicationViewModel
        {
            viewModel.ReferenceNumber = application.ReferenceNumber;
            viewModel.State           = application.State.ToDescription();
            viewModel.FullName        = MapFullName(application.Person);
            viewModel.AppliedOn       = application.Date;
            viewModel.SupportEmail    = configuration.SupportEmail;
            viewModel.Signature       = configuration.Signature;
            
            return viewModel;
        }
        private static string MapFullName(Person person) => $"{person.FirstName} {person.Surname}";
        public static LegalEntity MapLegalEntity(Application application) => application.IsLegalEntity ? application.LegalEntity : null;
        public static IEnumerable<Fund> MapFunds(IEnumerable<Product> products) => products.SelectMany(f => f.Funds);
    }
}