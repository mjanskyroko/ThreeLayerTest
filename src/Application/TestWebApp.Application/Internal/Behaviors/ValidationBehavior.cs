namespace TestWebApp.Application.Internal.Behaviors
{
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Internal.Exceptions;

    public class ValidationBehavior<TReq, TResp> : IPipelineBehavior<TReq, TResp>
        where TReq : notnull
    {
        private readonly IEnumerable<IValidator<TReq>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TReq>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResp> Handle(TReq request, RequestHandlerDelegate<TResp> next, CancellationToken cancellationToken)
        {
            List<ValidationResult> erros = new();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (result is not null)
                {
                    erros.Add(result);
                }
            }

            var failures = erros
                 .SelectMany(vres => vres.Errors)
                 .Where(fail => fail != null)
                 .GroupBy(fail => fail.PropertyName)
                 .ToDictionary(g => g.Key, g => g.Select(fail => fail.ErrorMessage).ToArray());

            if (failures.Any())
                throw new ServiceValidationException(failures);

            return await next();
        }
    }
}
