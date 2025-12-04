namespace FullStack.Produtos.Domain;

public static class ResponseHelper
{
    public static Response<object?> Success(string message) => Response<object?>.Success(message);
    public static Response<object?> Success() => Response<object?>.Success();    
    public static Response<object?> Validation(IEnumerable<ErrorResponse> errors) => Response<object?>.Validation(errors);
    public static Response<object?> Notification(string description) => Response<object?>.Notification(description);
    public static Response<object?> Error(string description) => Response<object?>.Error(description);    
}