using Diffusion.Models;

namespace diffuisiondashboard.Repository
{
    public class QueueRepo : BaseRepo<LoadRequest>
    {
        public QueueRepo(string tableName) : base(tableName)
        {

        }
    }
}
