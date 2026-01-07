namespace EFCoreTableRelationsTutorial.Dtos
{
    public record BookForGettingDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AuthorForGettingDto Author { get; set; }
    }
}
