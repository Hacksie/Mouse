
namespace HackedDesign
{
    public class GoapFactory 
    {
        public static GoapFactory Instance = new();

        public IGoapPlanner CreatePlanner()
        {
            return new GoapPlanner();
        }
    }
}