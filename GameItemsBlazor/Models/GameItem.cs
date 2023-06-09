namespace GameItemsBlazor.Models
{
    internal class GameItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.Now;
        public string ItemName { get; set; }
        public bool Played { get; set; }
    }

    internal class CreateGameItem
    {
        public string ItemName { get; set; }
    }

    internal class UpdateGameItem
    {
        public string ItemName { get; set; }
        public bool Played { get; set; }
    }
}
