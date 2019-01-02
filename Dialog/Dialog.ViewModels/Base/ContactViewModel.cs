using System.ComponentModel.DataAnnotations;

namespace Dialog.ViewModels.Base
{
    public class ContactViewModel
    {
        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string HereAppId { get; set; }
        public string HereAppCode { get; set; }
    }
}