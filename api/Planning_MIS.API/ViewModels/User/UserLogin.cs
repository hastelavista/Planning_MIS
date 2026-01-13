using System.ComponentModel.DataAnnotations;

namespace Planning_MIS.API.ViewModels.User
{
    public class LoginModel
    {
        [Required(ErrorMessage = "इमेल वा प्रयोगकर्ताको नाम आवश्यक छ ।"), Display(Name = "इमेल वा प्रयोगकर्ताको नाम")]
        public string Username { get; set; }
        [Required(ErrorMessage = "पासवर्ड आवश्यक छ ।"), Display(Name = "पासवर्ड")]
        public string Password { get; set; }
    }

    public class LoginUserDataModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Salt { get; set; }
        public string WardId { get; set; }
        public int UserTypeId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDepartmentUser { get; set; }

    }

    public class ChangePassword
    {
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "New password doesn't match.")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public int UserId { get; set; }
    }

    public class ForgotPassword
    {
        [Required(ErrorMessage = "कृपया इमेल ठेगाना प्रविष्ट गर्नुहोस् ।")]
        [Display(Name = "इमेल ठेगाना")]
        [EmailAddress(ErrorMessage = "अवैध इमेल ठेगाना ।")]
        public string Email { get; set; }
    }

    public class ResetPassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} कम्तिमा {2} क्यारेक्टर लामो हुनुपर्दछ ।", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "नया पासवर्ड")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "नया पासवर्ड निश्चित गर्नुहोस् ।")]
        [Compare("NewPassword", ErrorMessage = "नयाँ पासवर्ड र पुष्टि पासवर्ड मिलेन ।")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }

    public class UserPasswordReset
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} कम्तिमा {2} क्यारेक्टर लामो हुनुपर्दछ ।", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "नया पासवर्ड")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "नया पासवर्ड निश्चित गर्नुहोस्")]
        [Compare("NewPassword", ErrorMessage = "नयाँ पासवर्ड र पुष्टि पासवर्ड मिलेन ।")]
        public string ConfirmPassword { get; set; }

    }
}
