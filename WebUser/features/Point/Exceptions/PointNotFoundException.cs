namespace WebUser.features.Point.Exceptions
{
    public class PointNotFoundException:Exception
    {
        public PointNotFoundException(int id) : base($"Point with ID {id} doesnt exists")
        {

        }
    }
}
