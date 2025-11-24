namespace KJakub.Octave.Data
{
    public class Accuracy
    {
        public string Title { get; set; }
        public float Distance { get; set; }
        public int Weight { get; set; }
        public Accuracy(string title = "Ok", float distance = 0.5f, int weight = 2)
        {
            (Title, Distance, Weight) = (title, distance, weight);
        }
    }
}