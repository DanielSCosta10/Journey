using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Journey.Exception;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Journey.Infrastructure.Entities;
using FluentValidation.Results;

namespace Journey.Application.UseCases.Activities.Register;

internal class RegisterActivityForTripUseCase
{
    public ResponseActivityJson Execute(Guid tripId, RequestRegisterActivityJson resquest)
    {
        var dbContext = new JourneyDbContext();

        var trip = dbContext
            .Trips
            .Include(trip => trip.Activities)
            .FirstOrDefault(trip => trip.Id == tripId);

        if (trip is null)
        {
            throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);
        }



        return null;
    }

    private void Validate(Trip trip, RequestRegisterActivityJson resquest)
    {
        var validator = new RegisterActivityValidator();

        var result = validator.Validate(resquest);

        if ((resquest.Date >= trip.StartDate && resquest.Date <= trip.EndDate) == false)
        {
            result.Errors.Add(new ValidationFailure("Date", Res));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
