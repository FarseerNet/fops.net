using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.Build.Dal;
using FS.Extends;

namespace FOPS.Com.BuilderServer.Build
{
    /// <summary>
    /// 构建
    /// </summary>
    public class BuildService : IBuildService
    {
        public IProjectService   ProjectService   { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }

        /// <summary>
        /// 创建构建任务
        /// </summary>
        public async Task<int> Add(int projectId, int clusterId)
        {
            // 获取数据库中最后一个编译版本号
            var buildNumber = await BuilderContext.Data.Build.Where(o => o.ProjectId == projectId).Desc(o => o.Id).GetValueAsync(o => o.BuildNumber.GetValueOrDefault());
            var po = new BuildPO
            {
                ProjectId   = projectId,
                ClusterId   = clusterId,
                BuildNumber = ++buildNumber,
                Status      = EumBuildStatus.None,
                IsSuccess   = false,
                CreateAt    = DateTime.Now,
                FinishAt    = DateTime.Now,
            };

            await BuilderContext.Data.Build.InsertAsync(po);
            return buildNumber;
        }

        /// <summary>
        /// 主动取消任务
        /// </summary>
        public Task Cancel(int id)
        {
            BuildLogService.Write(id, "手动取消");
            return BuilderContext.Data.Build.Where(o => o.Id == id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = false,
                FinishAt  = DateTime.Now,
            });
        }

        

        /// <summary>
        /// 设置任务成功
        /// </summary>
        public Task Success(int clusterId, ProjectVO project, int buildId)
        {
            // 构建ID没有传的时候，通过版本号获取
            if (buildId == 0)
            {
                var buildNumber = project.DockerVer.ConvertType(0);
                buildId = BuilderContext.Data.Build.Where(o => o.BuildNumber == buildNumber && o.ProjectId == project.Id).GetValue(o => o.Id.GetValueOrDefault());
            }

            // 修改集群的镜像版本
            if (!project.DicClusterVer.ContainsKey(clusterId)) project.DicClusterVer[clusterId] = new();
            project.DicClusterVer[clusterId].DockerVer       = project.DockerVer;
            project.DicClusterVer[clusterId].DeploySuccessAt = DateTime.Now;
            project.DicClusterVer[clusterId].BuildSuccessId  = buildId;
            return ProjectService.UpdateAsync(project.Id, project.DicClusterVer);
        }

        /// <summary>
        /// 当前构建的队列数量
        /// </summary>
        public Task<int> CountAsync() => BuilderContext.Data.Build.Where(o => o.Status != EumBuildStatus.Finish).CountAsync();

        /// <summary>
        /// 获取构建队列前30
        /// </summary>
        /// <returns></returns>
        public Task<List<BuildVO>> ToBuildingList(int pageSize, int pageIndex) => BuilderContext.Data.Build.Select(o => new {o.Id, o.Status, o.BuildNumber, o.IsSuccess, o.ProjectId, o.CreateAt, o.FinishAt}).Desc(o => o.Id).ToListAsync(pageSize, pageIndex).MapAsync<BuildVO, BuildPO>();

        /// <summary>
        /// 查看构建信息
        /// </summary>
        public Task<BuildVO> ToInfoAsync(int id) => BuilderContext.Data.Build.Where(o => o.Id == id).ToEntityAsync().MapAsync<BuildVO, BuildPO>();
    }
}