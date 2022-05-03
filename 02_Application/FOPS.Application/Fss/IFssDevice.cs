using FOPS.Application.Fss.Entity;
using FS.Core;
using FS.Core.Net;
using Microsoft.Extensions.Logging;

namespace FOPS.Application.Fss;

public interface IFssDevice: ISingletonDependency
{
    /// <summary>
    /// 取出全局客户端列表
    /// </summary>
    Task<List<ClientDTO>> GetClientListAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 取出全局客户端数量
    /// </summary>
    Task<long> GetClientCountAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 复制任务组信息
    /// </summary>
    Task<ApiResponseJson<int>> CopyTaskGroupAsync(ValueTask<string> fssServerTask, int taskGroupId);
    /// <summary>
    /// 删除任务组
    /// </summary>
    Task<ApiResponseJson> DeleteTaskGroupAsync(ValueTask<string> fssServerTask, int taskGroupId);
    /// <summary>
    /// 获取任务组信息
    /// </summary>
    Task<TaskGroupDTO> GetTaskGroupInfoAsync(ValueTask<string> fssServerTask, int taskGroupId);
    /// <summary>
    /// 同步缓存到数据库
    /// </summary>
    Task SyncCacheToDbAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 获取全部任务组列表
    /// </summary>
    Task<List<TaskGroupDTO>> GetTaskGroupListAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 获取任务组数量
    /// </summary>
    Task<long> GetTaskGroupCountAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 获取未执行的任务数量
    /// </summary>
    Task<int> GetTaskGroupUnRunCountAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 添加任务组信息
    /// </summary>
    Task<ApiResponseJson<int>> AddTaskGroupAsync(ValueTask<string> fssServerTask, TaskGroupDTO dto);
    /// <summary>
    /// 保存TaskGroup
    /// </summary>
    Task SaveTaskGroupAsync(ValueTask<string> fssServerTask, TaskGroupDTO dto);
    /// <summary>
    /// 今日执行失败数量
    /// </summary>
    Task<int> TodayTaskFailCountAsync(ValueTask<string> fssServerTask);
    /// <summary>
    /// 获取进行中的任务
    /// </summary>
    Task<List<TaskDTO>> GetTaskUnFinishListAsync(ValueTask<string> fssServerTask, int top);
    /// <summary>
    /// 获取在用的任务
    /// </summary>
    Task<PageList<TaskDTO>> GetEnableTaskListAsync(ValueTask<string> fssServerTask, EumTaskType? status, int pageSize, int pageIndex);
    /// <summary>
    /// 获取指定任务组的任务列表
    /// </summary>
    Task<PageList<TaskDTO>> GetTaskListAsync(ValueTask<string> fssServerTask, int groupId, int pageSize, int pageIndex);
    /// <summary>
    /// 获取已完成的任务列表
    /// </summary>
    Task<PageList<TaskDTO>> GetTaskFinishListAsync(ValueTask<string> fssServerTask, int pageSize, int pageIndex);
    /// <summary>
    /// 取消任务
    /// </summary>
    Task CancelTask(ValueTask<string> fssServerTask, int taskGroupId);
    /// <summary>
    /// 获取日志
    /// </summary>
    Task<PageList<RunLogDTO>> GetRunLogListAsync(ValueTask<string> fssServerTask, string jobName, LogLevel? logLevel, int pageSize, int pageIndex);
}