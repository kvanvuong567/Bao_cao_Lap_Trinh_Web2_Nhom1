namespace API_MPICTURE.Models.Interfaces
{
    public interface IUDonwImageRepository
    {
        UpDownImage Upload(UpDownImage upDownImage);

        List<UpDownImage> GetAllInfoImages();

        (byte[], string, string) DownloadFile(int Id);
    }
}
