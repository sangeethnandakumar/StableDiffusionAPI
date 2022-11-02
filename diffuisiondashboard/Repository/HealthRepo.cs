using Diffusion.Models;

namespace diffuisiondashboard.Repository
{
    public class HealthRepo : BaseRepo<HealthInfo>
    {
        public HealthRepo(string tableName) : base(tableName)
        {

        }
    }
}
