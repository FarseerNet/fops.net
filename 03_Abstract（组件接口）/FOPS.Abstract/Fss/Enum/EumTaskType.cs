using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FOPS.Abstract.Fss.Enum
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum EumTaskType
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Display(Name = "未开始")]
        None = 0,
        /// <summary>
        /// 已调度
        /// </summary>
        [Display(Name = "已调度")]
        Scheduler = 1,
        /// <summary>
        /// 执行中
        /// </summary>
        [Display(Name = "执行中")]
        Working = 2,
        /// <summary>
        /// 失败
        /// </summary>
        [Display(Name = "失败")]
        Fail = 3,
        /// <summary>
        /// 完成
        /// </summary>
        [Display(Name = "完成")]
        Success = 4,
        /// <summary>
        /// 重新调度
        /// </summary>
        [Display(Name = "重新调度")]
        ReScheduler = 5,
    }
}