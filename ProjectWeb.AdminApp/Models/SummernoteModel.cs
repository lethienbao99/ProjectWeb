namespace ProjectWeb.AdminApp.Models
{
    public class SummernoteModel
    {
        public SummernoteModel(string idAreaTextBox, bool loadSrcipt = true)
        {
            IdAreaTextBox = idAreaTextBox;
            LoadSrcipt = loadSrcipt;
        }
        public string IdAreaTextBox { get; set; }
        public bool LoadSrcipt { get; set; }
        public int Height { get; set; } = 120;
        public string Toolbar { get; set; } = @" [
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['font', ['strikethrough', 'superscript', 'subscript']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']]
            ]";
    }
}
