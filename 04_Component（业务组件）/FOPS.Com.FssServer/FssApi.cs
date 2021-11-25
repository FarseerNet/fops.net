using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Enum;
using FOPS.Abstract.Fss.Server;
using FS.Core;
using FS.Core.Http;
using FS.Core.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FOPS.Com.FssServer
{
    public class FssApi : IFssApi
    {
        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        public async Task<List<ClientVO>> GetClientListAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync<ApiResponseJson<List<ClientVO>>>($"{fssServer}/meta/GetClientList", "{}", new(), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 取出全局客户端数量
        /// </summary>
        public async Task<long> GetClientCountAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetClientCount", "{}", ApiResponseJson<long>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 复制任务组信息
        /// </summary>
        public async Task<ApiResponseJson<int>> CopyTaskGroupAsync(ILocalStorageService localStorageService, int taskGroupId)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/CopyTaskGroup", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson<int>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 删除任务组
        /// </summary>
        public async Task<ApiResponseJson> DeleteTaskGroupAsync(ILocalStorageService localStorageService, int taskGroupId)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/DeleteTaskGroup", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取任务组信息
        /// </summary>
        public async Task<TaskGroupVO> GetTaskGroupInfoAsync(ILocalStorageService localStorageService, int taskGroupId)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupInfo", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson<TaskGroupVO>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 同步缓存到数据库
        /// </summary>
        public async Task SyncCacheToDbAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/SyncCacheToDb", "{}", ApiResponseJson<List<TaskGroupVO>>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取全部任务组列表
        /// </summary>
        public async Task<List<TaskGroupVO>> GetTaskGroupListAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupList", "{}", ApiResponseJson<List<TaskGroupVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 获取任务组数量
        /// </summary>
        public async Task<long> GetTaskGroupCountAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupCount", "{}", ApiResponseJson<long>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 获取未执行的任务数量
        /// </summary>
        public async Task<int> GetTaskGroupUnRunCountAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskGroupUnRunCount", "{}", ApiResponseJson<int>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 添加任务组信息
        /// </summary>
        public async Task<ApiResponseJson<int>> AddTaskGroupAsync(ILocalStorageService localStorageService, TaskGroupVO vo)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            return await HttpPostJson.TryPostAsync($"{fssServer}/meta/AddTaskGroup", JsonConvert.SerializeObject(vo), ApiResponseJson<int>.Error("出错了"), 2000);
        }

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        public async Task SaveTaskGroupAsync(ILocalStorageService localStorageService, TaskGroupVO vo)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/SaveTaskGroup", JsonConvert.SerializeObject(vo), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        public async Task<int> TodayTaskFailCountAsync(ILocalStorageService localStorageService)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/TodayTaskFailCount", "{}", ApiResponseJson<int>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : 0;
        }

        /// <summary>
        /// 获取进行中的任务
        /// </summary>
        public async Task<List<TaskVO>> GetTaskUnFinishListAsync(ILocalStorageService localStorageService, int top)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskUnFinishList", JsonConvert.SerializeObject(new { Top = top }), ApiResponseJson<List<TaskVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new();
        }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        public async Task<DataSplitList<TaskVO>> GetAllTaskListAsync(ILocalStorageService localStorageService, EumTaskType? status, int pageSize, int pageIndex)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetAllTaskList", JsonConvert.SerializeObject(new { Status = status, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<DataSplitList<TaskVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new(), 0);
        }

        /// <summary>
        /// 获取指定任务组的任务列表
        /// </summary>
        public async Task<DataSplitList<TaskVO>> GetTaskListAsync(ILocalStorageService localStorageService, int groupId, int pageSize, int pageIndex)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskList", JsonConvert.SerializeObject(new { GroupId = groupId, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<DataSplitList<TaskVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<TaskVO>(), 0);
        }

        /// <summary>
        /// 获取失败的任务
        /// </summary>
        public async Task<DataSplitList<TaskVO>> GetTaskFailListAsync(ILocalStorageService localStorageService, int pageSize, int pageIndex)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskFailList", JsonConvert.SerializeObject(new { PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<DataSplitList<TaskVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<TaskVO>(), 0);
        }

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        public async Task<DataSplitList<TaskVO>> GetTaskUnRunListAsync(ILocalStorageService localStorageService, int pageSize, int pageIndex)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetTaskUnRunList", JsonConvert.SerializeObject(new { PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<DataSplitList<TaskVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<TaskVO>(), 0);
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        public async Task CancelTask(ILocalStorageService localStorageService, int taskGroupId)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            await HttpPostJson.TryPostAsync($"{fssServer}/meta/CancelTask", JsonConvert.SerializeObject(new { Id = taskGroupId }), ApiResponseJson.Error("出错了"), 2000);
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        public async Task<DataSplitList<RunLogVO>> GetRunLogListAsync(ILocalStorageService localStorageService, string jobName, LogLevel? logLevel, int pageSize, int pageIndex)
        {
            var fssServer = await localStorageService.GetItemAsStringAsync("FssServer");
            var result    = await HttpPostJson.TryPostAsync($"{fssServer}/meta/GetRunLogList", JsonConvert.SerializeObject(new { JobName = jobName, LogLevel = logLevel, PageSize = pageSize, PageIndex = pageIndex }), ApiResponseJson<DataSplitList<RunLogVO>>.Error("出错了"), 2000);
            return result is { Status: true } ? result.Data : new(new List<RunLogVO>(), 0);
        }
    }
}