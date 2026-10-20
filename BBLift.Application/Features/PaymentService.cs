using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class PaymentService(
    IPaymentRepository repository,
    IValidator<CreatePaymentRequest> createValidator,
    IValidator<UpdatePaymentRequest> updateValidator
    )
    : IPaymentService
{
    public async Task<Result<PaymentResponse>> CreateAsync(
        CreatePaymentRequest request,
        CancellationToken ct = default
        )
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<PaymentResponse>.Failure(string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage)));

        var payment = await repository.AddAsync(new()
        {
            OrderId = request.OrderId,
            Amount = Money.TRY(request.Amount),
            Method = request.Method,
            Status = PaymentStatus.Pending
        });

        return Result<PaymentResponse>.Success(new(
            payment.Id,
            payment.OrderId,
            payment.Amount,
            payment.Method,
            payment.Status,
            payment.CreatedOn.LocalDateTime
            ));
    }

    public async Task<Result> UpdateStatusAsync(
        UpdatePaymentRequest request,
        CancellationToken ct = default
        )
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage)));

        var payment = await repository.GetAsync(predicate: x => x.Id == request.PaymentId, ct: ct);
        if (payment is null)
            return Result.Failure("Ödeme bulunamadı.");

        payment.Status = request.Status;
        await repository.UpdateAsync(payment);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<PaymentResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var payments = await repository.GetListAsync(ct: ct);
        if (!payments.Any())
            return Result<IEnumerable<PaymentResponse>>.Failure("Ödeme kaydı bulunamadı.");

        return Result<IEnumerable<PaymentResponse>>.Success(
            [.. payments.Select(x => new PaymentResponse(
                x.Id,
                x.OrderId,
                x.Amount,
                x.Method,
                x.Status,
                x.CreatedOn.LocalDateTime
                ))]
            );
    }

    public async Task<Result<PaymentResponse>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default
        )
    {
        var payment = await repository.GetAsync(
            predicate: x => x.Id == id,
            ct: ct
            );
        if (payment is null)
            return Result<PaymentResponse>.Failure("Ödeme bulunamadı.");

        return Result<PaymentResponse>.Success(new(
            payment.Id,
            payment.OrderId,
            payment.Amount,
            payment.Method,
            payment.Status,
            payment.CreatedOn.LocalDateTime
            ));
    }
}