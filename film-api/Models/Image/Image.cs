namespace FilmApi.Models
{
    public class Image
    {
        public long ImageID { get; set; }
        public long FilmID { get; set; }
        public string Data { get; set; }
        public Image(long filmID, string data)
            => (FilmID, Data) = (filmID, data);
    }
}
