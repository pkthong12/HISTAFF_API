namespace API.All.SYSTEM.CoreAPI.SysUser
{
    public class ResetPasswordRequest
    {
        public string? Username { get; set; }
        public string? VerificationCode { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
    }
}