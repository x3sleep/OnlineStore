using OnlineStore.BLL.DTOs;

namespace OnlineStore.API.Contracts;

public record UserOperationResponse(string Message, UserResponse User);
