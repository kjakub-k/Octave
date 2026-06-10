using System.Collections.Generic;
namespace KJakub.Octave.Data
{
    public class Attempt
    {
        public List<int> Scores { get; set; }
        public Attempt(List<int> scores)
        {
            Scores = scores;
        }
    }
}
