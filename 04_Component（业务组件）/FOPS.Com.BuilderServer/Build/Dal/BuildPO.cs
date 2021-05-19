using System;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Enum;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.BuilderServer.Build.Dal
{
    [Map(typeof(BuildVO))]
    public class BuildPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        
        /// <summary>
        /// 项目ID
        /// </summary>
        [Field(Name = "project_id")]
        public int? ProjectId { get; set; }
        
        /// <summary>
        /// 构建号
        /// </summary>
        [Field(Name = "build_number")]
        public int? BuildNumber { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        [Field(Name = "status")]
        public EumBuildStatus? Status { get; set; }
        
        /// <summary>
        /// 是否成功
        /// </summary>
        [Field(Name = "is_success")]
        public bool? IsSuccess { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        [Field(Name = "create_at")]
        public DateTime? CreateAt { get; set; }
        
        /// <summary>
        /// 耗时
        /// </summary>
        [Field(Name = "use_time")]
        public long? UseTime { get; set; }
        
        /// <summary>
        /// 完成时间
        /// </summary>
        [Field(Name = "finish_at")]
        public DateTime? FinishAt { get; set; }
    }
}