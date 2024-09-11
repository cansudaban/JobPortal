using JobPortal.Business.Interfaces;
using JobPortal.Common.Helpers;
using JobPortal.Common.ServiceResultManager;
using JobPortal.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Business.Classes
{
    public class CacheService : ICacheService
    {
        private readonly RedisConnectionService _redisService;

        private const string RestrictedWordsKey = "RestrictedWords";

        public CacheService(RedisConnectionService redisService)
        {
            _redisService = redisService;
        }
        public async Task<ServiceResult> AddRestrictedWordAsync(string word)
        {
            var db = _redisService.GetDatabase();
            await db.ListRightPushAsync(RestrictedWordsKey, word);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("RestrictedWordAddedSuccessfully"));
        }

        public async Task<ServiceResult> RemoveRestrictedWordAsync(string word)
        {
            var db = _redisService.GetDatabase();
            await db.ListRemoveAsync(RestrictedWordsKey, word);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("RestrictedWordRemovedSuccessfully"));
        }

        public async Task<List<string>> GetAllRestrictedWordsAsync()
        {
            var db = _redisService.GetDatabase();
            var restrictedWords = await db.ListRangeAsync(RestrictedWordsKey);

            var wList = new List<string>();
            foreach (var word in restrictedWords)
            {
                wList.Add(word.ToString());
            }

            return wList;
        }
    }
}
