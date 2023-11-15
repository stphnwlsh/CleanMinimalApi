namespace CleanMinimalApi.Presentation.Filters;

using FluentValidation;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var x = context.Arguments.FirstOrDefault();
        var y = x.GetType();

        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T argument)
        {
            return Results.BadRequest("Unable to find parameters or body for validation");
        }

        var validationResult = await validator.ValidateAsync(argument!);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}
