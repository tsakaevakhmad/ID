namespace ID.Domain.Dto
{
    public class DisplayName
    {
        private string _culture;
        public string Culture
        {
            get => _culture;
            set => _culture = value.ToLower();
        }
        public string Name { get; set; }
    }
}
