using JobPortal.Common.Dtos.UserDtos;
using JobPortal.Common.Models;
using JobPortal.Common.ServiceResultManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Business.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Yeni bir kullanıcı kaydı yapar.
        /// </summary>
        /// <param name="userRegisterDto">Kullanıcı kaydı için gerekli veriler.</param>
        /// <returns>ServiceResult ile sonucu döner.</returns>
        Task<ServiceResult> RegisterUserAsync(UserRegisterDto userRegisterDto);

        /// <summary>
        /// Kullanıcı giriş yapar.
        /// </summary>
        /// <param name="userLoginDto">Kullanıcının giriş bilgileri.</param>
        /// <returns>ServiceResult ile sonucu döner.</returns>
        Task<ServiceResult> LoginUserAsync(UserLoginDto userLoginDto);

        /// <summary>
        /// Kullanıcının şifresini yeniler.
        /// </summary>
        /// <param name="resetPasswordDto">Şifre yenileme için gerekli veriler.</param>
        /// <returns>ServiceResult ile sonucu döner.</returns>
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        /// <returns>ServiceResult ile kullanıcı listesini döner.</returns>
        Task<ServiceResult> GetAllUsersAsync();

        /// <summary>
        /// Belirtilen kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Silinecek kullanıcının ID'si.</param>
        /// <returns>ServiceResult ile sonucu döner.</returns>
        Task<ServiceResult> DeleteUserAsync(int id);
    }
}
