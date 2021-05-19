using System;
using System.Collections.Generic;

namespace FOPS.Abstract.K8S.Entity
{
    public class RunShellResult
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 输出结果
        /// </summary>
        public List<string> Output { get; set; }

        /// <summary>
        /// 按<br />拼接成一条消息
        /// </summary>
        public string OutputBr => String.Join("<br />", Output);
    }
}