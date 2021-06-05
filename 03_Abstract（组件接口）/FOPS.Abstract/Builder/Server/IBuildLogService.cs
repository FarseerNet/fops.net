using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IBuildLogService: ITransientDependency
    {
        /// <summary>
        /// 写入构建日志
        /// </summary>
        void Write(int id, string log);

        /// <summary>
        /// 查看日志
        /// </summary>
        string[] View(int buildId);

        /// <summary>
        /// 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
        /// </summary>
        void Clear(int buildId);
    }
}