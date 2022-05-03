using FOPS.Application.Fss;
using FOPS.Application.Fss.Entity;
using FS.Core;
using FS.Core.Http;
using FS.Core.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FOPS.Infrastructure.Device
{
    public class FssDevice : IFssDevice
    {
        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        public async Task<List<ClientDTO>> GetClientListAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync<ApiResponseJson<List<ClientDTO>>>($"{fssServer}/meta/GetClientList", "{}", new(), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 取出全局客户端数量
        /// </summary>
        public async Task<long> GetClientCountAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetClientCount", "{}", ApiResponseJson<long>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 复制任务组信息
        /// </summary>
        public async Task<ApiResponseJson<int>> CopyTaskGroupAsync(ValueTask<string> fssServerTask, int taskGroupId)
        {
            var fssServer = await fssServerTask;
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/CopyTaskGroup", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson<int>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 删除任务组
        /// </summary>
        public async Task<ApiResponseJson> DeleteTaskGroupAsync(ValueTask<string> fssServerTask, int taskGroupId)
        {
            var fssServer = await fssServerTask;
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/DeleteTaskGroup", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取任务组信息
        /// </summary>
        public async Task<TaskGroupDTO> GetTaskGroupInfoAsync(ValueTask<string> fssServerTask, int taskGroupId)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupInfo", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson<TaskGroupDTO>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 同步缓存到数据库
        /// </summary>
        public async Task SyncCacheToDbAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/SyncCacheToDb", "{}", ApiResponseJson<List<TaskGroupDTO>>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取全部任务组列表
        /// </summary>
        public async Task<List<TaskGroupDTO>> GetTaskGroupListAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupList", "{}", ApiResponseJson<List<TaskGroupDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 获取任务组数量
        /// </summary>
        public async Task<long> GetTaskGroupCountAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupCount", "{}", ApiResponseJson<long>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 获取未执行的任务数量
        /// </summary>
        public async Task<int> GetTaskGroupUnRunCountAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupUnRunCount", "{}", ApiResponseJson<int>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 添加任务组信息
        /// </summary>
        public async Task<ApiResponseJson<int>> AddTaskGroupAsync(ValueTask<string> fssServerTask, TaskGroupDTO dto)
        {
            var fssServer = await fssServerTask;
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/AddTaskGroup", JsonConvert.SerializeObject(dto), ApiResponseJson<int>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        public async Task SaveTaskGroupAsync(ValueTask<string> fssServerTask, TaskGroupDTO dto)
        {
            var fssServer = await fssServerTask;
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/SaveTaskGroup", JsonConvert.SerializeObject(dto), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        public async Task<int> TodayTaskFailCountAsync(ValueTask<string> fssServerTask)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/TodayTaskFailCount", "{}", ApiResponseJson<int>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 获取进行中的任务
        /// </summary>
        public async Task<List<TaskDTO>> GetTaskUnFinishListAsync(ValueTask<string> fssServerTask, int top)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskUnFinishList", JsonConvert.SerializeObject(new { Top = top }), ApiResponseJson<List<TaskDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 获取在用的任务
        /// </summary>
        public async Task<PageList<TaskDTO>> GetEnableTaskListAsync(ValueTask<string> fssServerTask, EumTaskType? status, int pageSize, int pageIndex)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetEnableTaskList", JsonConvert.SerializeObject(new { Status = status, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<PageList<TaskDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new(), 0);
        }

        /// <summary>
        /// 获取指定任务组的任务列表
        /// </summary>
        public async Task<PageList<TaskDTO>> GetTaskListAsync(ValueTask<string> fssServerTask, int groupId, int pageSize, int pageIndex)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskList", JsonConvert.SerializeObject(new { GroupId = groupId, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<PageList<TaskDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<TaskDTO>(), 0);
        }

        /// <summary>
        /// 获取已完成的任务列表
        /// </summary>
        public async Task<PageList<TaskDTO>> GetTaskFinishListAsync(ValueTask<string> fssServerTask, int pageSize, int pageIndex)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskFinishList", JsonConvert.SerializeObject(new { PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<PageList<TaskDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<TaskDTO>(), 0);
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        public async Task CancelTask(ValueTask<string> fssServerTask, int taskGroupId)
        {
            var fssServer = await fssServerTask;
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/CancelTask", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        public async Task<PageList<RunLogDTO>> GetRunLogListAsync(ValueTask<string> fssServerTask, string jobName, LogLevel? logLevel, int pageSize, int pageIndex)
        {
            var fssServer = await fssServerTask;
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetRunLogList", JsonConvert.SerializeObject(new { JobName = jobName, LogLevel = logLevel, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<PageList<RunLogDTO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<RunLogDTO>(), 0);
        }
    }
}