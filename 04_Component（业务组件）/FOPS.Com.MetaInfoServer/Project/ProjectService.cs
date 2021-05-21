using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.MetaInfoServer.Project.Dal;
using FS.Extends;
using Newtonsoft.Json;

namespace FOPS.Com.MetaInfoServer.Project
{
    public class ProjectService : IProjectService
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        public async Task<List<ProjectVO>> ToListAsync()
        {
            var lstPo = await MetaInfoContext.Data.Project.ToListAsync();
            var lst   = new List<ProjectVO>();
            foreach (var po in lstPo)
            {
                var vo = po.Map<ProjectVO>();
                try
                {
                    vo.DicClusterVer = JsonConvert.DeserializeObject<Dictionary<int, ClusterVer>>(po.ClusterVer);
                }
                catch
                {
                    vo.DicClusterVer = new Dictionary<int, ClusterVer>();
                }

                lst.Add(vo);
            }

            return lst;
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        public async Task<ProjectVO> ToInfoAsync(int id)
        {
            var po = await MetaInfoContext.Data.Project.Where(o => o.Id == id).ToEntityAsync();
            var vo = po.Map<ProjectVO>();
            try
            {
                vo.DicClusterVer = JsonConvert.DeserializeObject<Dictionary<int, ClusterVer>>(po.ClusterVer);
            }
            catch
            {
                vo.DicClusterVer = new Dictionary<int, ClusterVer>();
            }

            return vo;
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectVO ToInfo(int id)
        {
            var po = MetaInfoContext.Data.Project.Where(o => o.Id == id).ToEntity();
            var vo = po.Map<ProjectVO>();
            try
            {
                vo.DicClusterVer = JsonConvert.DeserializeObject<Dictionary<int, ClusterVer>>(po.ClusterVer);
            }
            catch
            {
                vo.DicClusterVer = new Dictionary<int, ClusterVer>();
            }

            return vo;
        }

        /// <summary>
        /// 项目数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.Project.CountAsync();

        /// <summary>
        /// 添加项目
        /// </summary>
        public async Task<int> AddAsync(ProjectVO vo)
        {
            var po = vo.Map<ProjectPO>();
            po.ClusterVer = vo.DicClusterVer != null ? JsonConvert.SerializeObject(vo.DicClusterVer) : "{}";
            await MetaInfoContext.Data.Project.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        public Task UpdateAsync(int id, ProjectVO vo)
        {
            var po = vo.Map<ProjectPO>();
            po.ClusterVer = vo.DicClusterVer != null ? JsonConvert.SerializeObject(vo.DicClusterVer) : "{}";
            return MetaInfoContext.Data.Project.Where(o => o.Id == id).UpdateAsync(po);
        }

        /// <summary>
        /// 修改镜像版本
        /// </summary>
        public Task UpdateAsync(int id, string dockerVer)
        {
            return MetaInfoContext.Data.Project.Where(o => o.Id == id).UpdateAsync(new ProjectPO
            {
                DockerVer = dockerVer
            });
        }

        /// <summary>
        /// 修改集群的镜像版本
        /// </summary>
        public Task UpdateAsync(int id, Dictionary<int, ClusterVer> dicClusterVer)
        {
            return MetaInfoContext.Data.Project.Where(o => o.Id == id).UpdateAsync(new ProjectPO
            {
                ClusterVer = JsonConvert.SerializeObject(dicClusterVer)
            });
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Project.Where(o => o.Id == id).DeleteAsync();
    }
}