namespace FullStack.Produtos.Domain;

public record Response<T>
{
    public Response(bool isSuccess, string? message, IEnumerable<ErrorResponse> errors, T? value)
    {
        if (isSuccess && errors.Any() || !isSuccess && !errors.Any())
            throw new ArgumentException("Erro inválido", nameof(errors));        
        
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
        Value = value;
    }

    public Response(bool isSuccess, IEnumerable<ErrorResponse> errors, T? value) : this(isSuccess, null, errors, value)
    {
    }

    public bool IsSuccess { get; }
    public string? Message { get; }
    public IEnumerable<ErrorResponse> Errors { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    
    public static Response<T> Success(string message, T? value = default) => new(true, message, [], value);
    public static Response<T> Success(T? value = default) => new(true, [], value);    
    public static Response<T> Validation(IEnumerable<ErrorResponse> errors) => new(false, "Erros de validações", errors, default);
    public static Response<T> Notification(string description) => new(false, "Notificação ao usuário", [new ErrorResponse("Notificacao", description)], default);
    public static Response<T> Error(string description) => new(false, "Erro inesperado", [new ErrorResponse("Erro Interno", description)], default);    
}