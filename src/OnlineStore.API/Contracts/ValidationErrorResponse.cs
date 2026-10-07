namespace OnlineStore.API.Contracts;

public record ValidationErrorResponse(string Message, IDictionary<string, string[]> Errors);
