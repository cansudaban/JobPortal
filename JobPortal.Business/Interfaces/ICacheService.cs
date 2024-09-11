using JobPortal.Common.ServiceResultManager;

namespace JobPortal.Business.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        /// Yeni bir sakıncalı kelime ekler.
        /// </summary>
        /// <param name="word">Eklenecek sakıncalı kelime.</param>
        /// <returns>Sakıncalı kelimenin başarıyla eklenip eklenmediği bilgisi.</returns>
        Task<ServiceResult> AddRestrictedWordAsync(string word);

        /// <summary>
        /// Sakıncalı kelimeyi siler.
        /// </summary>
        /// <param name="word">Silinecek sakıncalı kelime.</param>
        /// <returns>Sakıncalı kelimenin başarıyla silinip silinmediği bilgisi.</returns>
        Task<ServiceResult> RemoveRestrictedWordAsync(string word);

        /// <summary>
        /// Tüm sakıncalı kelimeleri getirir.
        /// </summary>
        /// <returns>Sakıncalı kelimelerin listesi.</returns>
        Task<List<string>> GetAllRestrictedWordsAsync();

    }
}
