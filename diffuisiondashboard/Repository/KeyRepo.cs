using Diffusion.Models;

namespace diffuisiondashboard.Repository
{
    public class KeyRepo : BaseRepo<APIKey>
    {
        public KeyRepo(string tableName) : base(tableName)
        {

        }
    }
}
