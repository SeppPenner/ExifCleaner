namespace ExifRemove.Models
{
    public class ExifItem
    {
        public ExifItem(string extension, string name, string path, string fullpath)
        {
            Name = name;
            Path = path;
            Extension = extension;
            Fullpath = fullpath;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Extension { get; set; }

        public string Fullpath { get; set; }
    }
}