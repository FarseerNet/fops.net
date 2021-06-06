using System.ComponentModel.DataAnnotations;

namespace FOPS.Abstract.MetaInfo.Enum
{
    /// <summary>
    /// 构建方式
    /// </summary>
    public enum EumBuildType
    {
        /// <summary>
        /// dotnet publish
        /// </summary>
        [Display(Name = "dotnet publish")]
        DotnetPublish,
        /// <summary>
        /// Shell脚本
        /// </summary>
        [Display(Name = "Shell脚本")]
        Shell
    }
}