using System.Configuration;

namespace FOPS.Abstract.MetaInfo.Enum
{
    /// <summary>
    /// 运行环境
    /// </summary>
    public enum EumRuntimeEnv
    {
        /// <summary>
        /// 开发环境
        /// </summary>
        Dev = 0,

        /// <summary>
        /// 测试环境
        /// </summary>
        Test = 1,

        /// <summary>
        /// 预发布
        /// </summary>
        PreRelease = 2,

        /// <summary>
        /// 生产环境
        /// </summary>
        Prod = 3,
    }
}