using System.Threading.Tasks;

namespace FOPS.Com.BuilderServer.Build
{
    public interface IBuildOpr
    {
        /// <summary>
        /// 构建
        /// </summary>
        Task Build();
    }
}