namespace WebUser.features.Point.DTO;

using Swashbuckle.AspNetCore.Filters;

public class PointMinDTOSW : IExamplesProvider<PointMinDTO>
{
    public PointMinDTO GetExamples() =>
        new()
        {
            ID = 1,
            BalanceLeft = 100,
            IsActive = true,
            IsUsed = false,
            Value = 100,
        };
}
