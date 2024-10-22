namespace WebUser.features.Review.Exceptions;

using WebUser.Domain.exceptions;

public class ReviewNotFoundException : NotFoundException
{
    public ReviewNotFoundException(int id)
        : base($"Review with ID {id} doesnt exists") { }
}
