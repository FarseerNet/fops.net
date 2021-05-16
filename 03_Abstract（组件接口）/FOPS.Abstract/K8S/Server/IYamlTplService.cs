using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FS.DI;

namespace FOPS.Abstract.K8S.Server
{
    public interface IYamlTplService: ITransientDependency
    {
        /// <summary>
        /// Yaml模板列表
        /// </summary>
        Task<List<YamlTplVO>> ToListAsync();

        /// <summary>
        /// Yaml模板信息
        /// </summary>
        Task<YamlTplVO> ToInfoAsync(int id);

        /// <summary>
        /// Yaml模板数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加Yaml模板
        /// </summary>
        Task<int> AddAsync(YamlTplVO vo);

        /// <summary>
        /// 修改Yaml模板
        /// </summary>
        Task UpdateAsync(int id, YamlTplVO vo);

        /// <summary>
        /// 删除Yaml模板
        /// </summary>
        Task DeleteAsync(int id);
    }
}