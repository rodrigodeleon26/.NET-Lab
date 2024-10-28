using Google.Authenticator;
using Microsoft.AspNetCore.Identity;
using DAL.Models;

namespace AuthWebApi.Services
{
    public class TwoFactorAuthService
    {
        private readonly UserManager<AppUsers> _userManager;

        public TwoFactorAuthService(UserManager<AppUsers> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(string qrCodeImageUrl, string manualEntrySetupCode)> GenerateQrCodeAsync(AppUsers user)
        {
            var key = user.TwoFactorAuthKey;
            if (string.IsNullOrEmpty(key))
            {
                key = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(key))
                {
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    key = await _userManager.GetAuthenticatorKeyAsync(user);
                }
                user.TwoFactorAuthKey = key;
                await _userManager.UpdateAsync(user); // Guardar la clave en la base de datos
            }

            var tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("AuthWebApi", user.Email, key, false, 3);

            return (setupInfo.QrCodeSetupImageUrl, setupInfo.ManualEntryKey);
        }

        public bool ValidateTwoFactorCode(AppUsers user, string code)
        {
            var key = user.TwoFactorAuthKey;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(key, code);
        }
    }
}
