namespace WebUser.features.Image.Exceptions
{
    public class ImageNotFoundException:Exception
    {
        public ImageNotFoundException(int id) : base($"Image with ID {id} doesnt exists")
        {

        }
    }
}
