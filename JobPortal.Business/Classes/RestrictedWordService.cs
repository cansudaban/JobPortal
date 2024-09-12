using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos;
using JobPortal.Common.Helpers;
using JobPortal.Common.ServiceResultManager;
using Nest;

namespace JobPortal.Business.Classes
{
    public class RestrictedWordService : IRestrictedWordService
    {
        private readonly IElasticClient _elasticClient;
        private const string IndexName = "restricted_words";

        public RestrictedWordService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<ServiceResult> AddRestrictedWordAsync(string word)
        {
            var response = await _elasticClient.IndexDocumentAsync(new { Word = word });

            if (!response.IsValid)
                return Common.ServiceResultManager.Result.ReturnAsFail(ResourceHelper.GetMessage("RestrictedWordAddedFailed"));

            return Common.ServiceResultManager.Result.ReturnAsSuccess(ResourceHelper.GetMessage("RestrictedWordAddedSuccessfully"));
        }

        public async Task<ServiceResult> RemoveRestrictedWordAsync(string word)
        {
            var searchResponse = await _elasticClient.SearchAsync<RestrictedWord>(s => s
                .Index(IndexName)
                .Query(q => q
                    .Term(t => t.Word, word)));

            var document = searchResponse.Documents.FirstOrDefault();
            if (document == null)
                return Common.ServiceResultManager.Result.ReturnAsFail(ResourceHelper.GetMessage("RestrictedWordNotFound"));

            var deleteResponse = await _elasticClient.DeleteAsync<RestrictedWord>(document.Id, d => d.Index(IndexName));

            if (!deleteResponse.IsValid)
                return Common.ServiceResultManager.Result.ReturnAsFail(ResourceHelper.GetMessage("RestrictedWordRemovedFailed"));

            return Common.ServiceResultManager.Result.ReturnAsSuccess(ResourceHelper.GetMessage("RestrictedWordRemovedSuccessfully"));
        }


        public async Task<List<string>> GetAllRestrictedWordsAsync()
        {
            var searchResponse = await _elasticClient.SearchAsync<RestrictedWord>(s => s
                .Index(IndexName)
                .Size(1000)
                .Query(q => q.MatchAll()));

            var restrictedWords = searchResponse.Documents.Select(doc => doc.Word).ToList();

            return restrictedWords;
        }
    }
}
