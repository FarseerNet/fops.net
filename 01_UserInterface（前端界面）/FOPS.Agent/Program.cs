// See https://aka.ms/new-console-template for more information

using FS.Utils.Component;

Console.WriteLine("Hello, World!");

// 获取CPU核心数及名称
// cat /proc/cpuinfo | grep name | cut -f2 -d: | uniq -c
//       4  Intel(R) Xeon(R) E-2124G CPU @ 3.40GHz
ShellTools.Run("cat", "/proc/cpuinfo | grep name | cut -f2 -d: | uniq -c", null, null);

// 获取CPU使用率
// cat /proc/stat
// 在 /proc /stat中有cpu运行的几个值
// 分别为user,sys,nice,idle
// 读这个文件两次(注意，中间要sleep(1)秒)
// 得到
// user1, sys1, nice1, idle1;
// user2,sys2,nice2,idle2;
// 计算公式如下(注意：要是后读的减去先读的)
// u    = user2 - user1;
// sys  = sys2  -sys1;
// nice = nice2 -nice1;
// idle = idle2 -idle1;
// 
// result = (u +sys +nice) /(u +sys +nice +idle)；
// 这样就得到结果


// 获取内存
// cat /proc/meminfo
// MemTotal:       65589520 kB
// MemFree:          613288 kB
// MemAvailable:   26541612 kB
ShellTools.Run("cat", "/proc/meminfo", null, null);