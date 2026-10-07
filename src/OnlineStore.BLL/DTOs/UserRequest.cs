using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.DTOs;

public class UserRequest
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен содержать от 3 до 50 символов")]
    [RegularExpression("^[A-Za-z0-9_.-]+$", ErrorMessage = "Логин может содержать только латинские буквы, цифры и символы _ . -")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Хеш пароля обязателен")]
    [RegularExpression("^[A-Fa-f0-9]{64}$", ErrorMessage = "PassHash должен быть SHA-256 хешем: 64 шестнадцатеричных символа")]
    public string PassHash { get; set; } = string.Empty;
}
