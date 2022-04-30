using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FOPS.Abstract.K8S.Enum;

/// <summary>
/// K8S负载类型
/// </summary>
public enum EumK8sControllers
{
    /// <summary>
    /// 无状态应用
    /// </summary>
    [Display(Name = "deployment")]
    Deployment,
    /// <summary>
    /// 有状态应用
    /// </summary>
    [Display(Name = "statefulSet")]
    StatefulSet,
    /// <summary>
    /// 所有节点都会运行一个实例
    /// </summary>
    [Display(Name = "daemonSet")]
    DaemonSet,
    /// <summary>
    /// 一次性任务
    /// </summary>
    [Display(Name = "job")]
    Job,
    /// <summary>
    /// 定时任务
    /// </summary>
    [Display(Name = "cronjob")]
    Cronjob
}