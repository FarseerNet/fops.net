## 概述
提供给开发人员、管理员维护的运维系统。

以简洁的方式提供WEB端操作

本平台采用`.NET5 + Blazor`技术实现

![Image text](https://gitee.com/FarseerNet/FOPS/raw/main/07_Solution%20Items%EF%BC%88%E9%A1%B9%E7%9B%AE%E6%96%87%E4%BB%B6%EF%BC%89/index.png)

### 功能

1、`持续集成`，从git仓库拉取代码、`编译`、`打包`成`docker`镜像，并发布到`K8s`。

2、`链路跟踪`，查看整个链路的完整生命周期的耗时调用。（包括：`WebApi`、`Consumer`、`FssJob`、`BackgroundService`）

3、`慢查询`，支持`db`、`Redis`、`http`、`es`、`mq` 耗时高的语句。

4、`FSS调度管理`，支持任务的维护、查看进度、执行日志。

5、`yaml管理`，将yaml脚本维护到FOPS，并支持`远程更新`到K8S。

### 待实现

1、`服务器环境`，查看服务器的CPU、内存、硬盘、进程等环境数据。

2、`日志统计`，支持查看所有应用产生的日志，并提供状态统计，及告警。

3、`MQ积压统计`，支持查看目前队列的积压情况，并提供统计，及告警。

4、`健康检查`，提供各个`应用`、`db`、`Redis`、`http`、`es`、`mq`健康检查，及告警。

### Docker部署

```
docker run -d --name fops -p 88:88 fops fops:latest \
-e Database__fops="DataType=MySql,Server=mysql:3306,UserID=root,PassWord=steden@123,Catalog=fops,PoolMaxSize=50,PoolMinSize=1" \ # 数据库连接
-e FSS__本地="http://fss:888" \ # fss server 地址
-e ElasticSearch__LinkTrack="Server=http://es:9200,Username=es,Password=123456" \ # 全链路跟踪存储的es地址
--restart=always
```