## 概述

FOPS的目的是提供给开发人员用于维护`K8S的yaml管理`
、`Git管理`、`容器管理`、`系统构建`、`FSS调度管理`
的平台。

将目前主流的系统，以更加简洁的方式提供WEB端操作

本平台采用`.NET5 + Blazor`
技术实现

![Image text](https://gitee.com/FarseerNet/FOPS/raw/main/07_Solution%20Items%EF%BC%88%E9%A1%B9%E7%9B%AE%E6%96%87%E4%BB%B6%EF%BC%89/index.png)

## 解决什么

    通常在我们的项目（特别是微服务）越来越多，几十上百项目的时候
    要对项目从开发、GIT提交到构建再到持续部署，利用现有开源的项目维护时，
    发现在各自系统都需要维护一套基础信息，比如项目名称、如何构建（维护、打包、上传镜像）、如何部署等。
    这对运维来说是一项非常繁琐且无意义的基础工作。
    就拿我自己来说，新建一个ASP.NET项目，我需要在：
        1、VS中添加一个项目
        2、到Jenkins中添加项目
        3、在Jenkins中编写构建步骤：编译、打包、上传镜像
          （可能可以利用之前项目脚本，或简单的通过环境变量，但起码还是要稍微修改或添加一下）
        4、编写Dockerfile（尽管只是简单的复制再修改）
        5、编写K8s的yaml脚本（尽管只是简单的复制再修改）
        6、到K8S平台，发布yaml脚本
    以上6个步骤，除了第1项是必须的，2-6步骤都是繁琐的添加、修改工作。
    以前我的项目一般就2，3个，所以在维护这些基础信息相对也比较简单。
    最近在工作的单位中，要维护好几十个服务再一个项目中。
    因此才会衍生出本系统来。

## 功能

1、`FSS调度平台`
的Web控制台操作

2、K8S
Yaml模板管理：

    当我们的项目越来越多（微服务），则对应的`yaml文件`也会越来越多。
    如果要对yaml文件修改时，则会很麻烦。
    本平台采用模板的方式，然后将项目绑定到yaml文件。
    可实现一键远程更新、批量更新所有项目的yaml。

3、轻量级构建

    我们为您定义了标准的`构建步骤`：
    Git拉取-->编译-->打包镜像-->上传镜像-->更新k8s镜像版本等一系列步骤。
    用于取代jenkins之类的发布系统，支持原生的dotnet publish命令或自定义Shell脚本

4、轻松管理

    通过本系统，您只需要在这里维护一套基础信息，而不用到Jenkins、K8S中各自维护一套。
    本系统通过打通这些组件，您只需要简单的维护好项目信息，就可以对K8S、CD、容器串联起来。

## 菜单列表

```
├─ 控制台
│  └─ 大盘          （数据统计、监控）
├─ FSS             （FSS调度平台）
│  ├─ 任务组        （FSS调度平台-任务组管理）
│  ├─ 失败任务       （FSS调度平台-查询失败的任务）
│  ├─ 未执行任务     （FSS调度平台-监控到时间未执行的任务）
│  ├─ 服务端节点     （FSS调度平台-当前节点列表）
│  └─ 客户端列表     （FSS调度平台-当前注册到FSS平台的客户端）
├─ 构建
│  └─ 立即          （git拉取、编译、打包镜像、上传镜像、更新k8s镜像版本）
├─ 容器
│  ├─ 模板管理       （管理镜像打包时的Dockerfile）
│  ├─ 仓库管理       （维护镜像仓库，用于构建时，上传到仓库中）
├─ K8S
│  ├─ 集群管理       （可添加开发环境、测试环境、预发布环境、生产环境的K8S集群）
│  ├─ 模板管理       （添加yaml脚本，可自定义变量，用于发布时替换）
│  ├─ 模板发布       （根据项目列表，并绑定yaml模板后，可针对单个或全部项目远程更新yaml）
│  └─ Single发布     (可直接使用临时yaml，进行远程更新、创建 yaml）
└─ 设置
│  ├─ 项目组设置      （项目组的基础信息）
│  ├─ 项目设置        （项目的基础信息）
│  ├─ Git设置        （项目的git地址信息）
│  ├─ 管理员设置
│  │   ├─ 管理员设置
│  │   └─ 修改密码
```

## Mysql表结构

```sql
SET NAMES utf8mb4;
SET
FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admin
-- ----------------------------
DROP TABLE IF EXISTS `admin`;
CREATE TABLE `admin`
(
    `id`            int(11) NOT NULL AUTO_INCREMENT,
    `user_name`     varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '管理员名称',
    `user_pwd`      varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '管理员密码',
    `is_enable`     bit(1)                                                 NOT NULL DEFAULT b'1' COMMENT '是否启用',
    `last_login_at` timestamp(6)                                           NOT NULL DEFAULT CURRENT_TIMESTAMP(
            6) COMMENT '上次登陆时间',
    `last_login_ip` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '上次登陆IP',
    `create_at`     timestamp(6)                                           NOT NULL DEFAULT CURRENT_TIMESTAMP(
            6) COMMENT '创建时间',
    `create_user`   varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '创建人',
    `create_id`     int(11) NOT NULL DEFAULT '0' COMMENT '创建人ID',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for basic_git
-- ----------------------------
DROP TABLE IF EXISTS `basic_git`;
CREATE TABLE `basic_git`
(
    `id`        int(11) NOT NULL AUTO_INCREMENT,
    `name`      varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT 'Git名称',
    `hub`       varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '托管地址',
    `user_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '账户名称',
    `user_pwd`  varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '账户密码',
    `pull_at`   timestamp(6)                                            NOT NULL DEFAULT CURRENT_TIMESTAMP(
            6) COMMENT '拉取时间',
    `branch`    varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT 'main' COMMENT '分支',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for basic_project
-- ----------------------------
DROP TABLE IF EXISTS `basic_project`;
CREATE TABLE `basic_project`
(
    `id`                 int(11) NOT NULL AUTO_INCREMENT,
    `name`               varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci   NOT NULL DEFAULT '' COMMENT '项目名称',
    `group_id`           int(11) NOT NULL DEFAULT '0' COMMENT '项目组ID',
    `git_id`             int(11) NOT NULL DEFAULT '0' COMMENT 'GIT',
    `path`               varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '项目路径',
    `k8s_tpl_deployment` int(11) NOT NULL DEFAULT '0' COMMENT 'K8SDeployment模板',
    `k8s_tpl_ingress`    int(11) NOT NULL DEFAULT '0' COMMENT 'K8SIngress模板',
    `k8s_tpl_service`    int(11) NOT NULL DEFAULT '0' COMMENT 'K8SService模板',
    `k8s_tpl_config`     int(11) NOT NULL DEFAULT '0' COMMENT 'K8SConfig模板',
    `k8s_tpl_variable`   varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT 'K8S模板自定义变量(K1=V1,K2=V2)',
    `docker_ver`         varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci   NOT NULL DEFAULT '0' COMMENT '镜像版本',
    `cluster_ver`        varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '{}' COMMENT '集群版本',
    `dockerfile_tpl`     int(11) NOT NULL DEFAULT '0' COMMENT 'DockerfileTpl模板',
    `docker_hub`         int(11) NOT NULL DEFAULT '0' COMMENT '镜像仓库',
    `entry_point`        varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci   NOT NULL DEFAULT '' COMMENT '程序入口名称',
    `entry_port`         int(11) NOT NULL DEFAULT '80' COMMENT '程序启动端口',
    `domain`             varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '访问域名',
    `build_type`         tinyint(2) NOT NULL DEFAULT '0' COMMENT '构建方式',
    `shell_script`       longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Shell脚本',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for basic_project_group
-- ----------------------------
DROP TABLE IF EXISTS `basic_project_group`;
CREATE TABLE `basic_project_group`
(
    `id`          int(11) NOT NULL AUTO_INCREMENT,
    `name`        varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '项目名称',
    `cluster_ids` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '集群ID列表',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for build
-- ----------------------------
DROP TABLE IF EXISTS `build`;
CREATE TABLE `build`
(
    `id`           int(11) NOT NULL AUTO_INCREMENT,
    `project_id`   int(11) NOT NULL DEFAULT '0' COMMENT '项目ID',
    `build_number` int(11) NOT NULL DEFAULT '0' COMMENT '构建号',
    `status`       tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态',
    `is_success`   bit(1) NOT NULL DEFAULT b'0' COMMENT '是否成功',
    `create_at`    datetime(6) NOT NULL COMMENT '开始时间',
    `finish_at`    datetime(6) NOT NULL COMMENT '完成时间',
    `cluster_id`   int(11) NOT NULL DEFAULT '0' COMMENT '集群ID',
    PRIMARY KEY (`id`) USING BTREE,
    KEY            `status` (`status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=249 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for docker_file_tpl
-- ----------------------------
DROP TABLE IF EXISTS `docker_file_tpl`;
CREATE TABLE `docker_file_tpl`
(
    `id`       int(11) NOT NULL AUTO_INCREMENT,
    `name`     varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '模板名称',
    `template` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '模板内容',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for docker_hub
-- ----------------------------
DROP TABLE IF EXISTS `docker_hub`;
CREATE TABLE `docker_hub`
(
    `id`        int(11) NOT NULL AUTO_INCREMENT,
    `name`      varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '镜像名称',
    `hub`       varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '托管地址',
    `user_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '账户名称',
    `user_pwd`  varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci  NOT NULL DEFAULT '' COMMENT '账户密码',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for k8s_cluster
-- ----------------------------
DROP TABLE IF EXISTS `k8s_cluster`;
CREATE TABLE `k8s_cluster`
(
    `id`               int(11) NOT NULL AUTO_INCREMENT,
    `name`             varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '集群名称',
    `config`           longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '本地kubectl配置',
    `runtime_env_type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '集群环境类型',
    `sort`             int(11) NOT NULL DEFAULT '0' COMMENT '排序',
    PRIMARY KEY (`id`) USING BTREE,
    KEY                `sort` (`sort`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for k8s_yaml_tpl
-- ----------------------------
DROP TABLE IF EXISTS `k8s_yaml_tpl`;
CREATE TABLE `k8s_yaml_tpl`
(
    `id`            int(11) NOT NULL AUTO_INCREMENT,
    `k8s_kind_type` tinyint(4) NOT NULL DEFAULT '0' COMMENT 'k8s类型',
    `name`          varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '模板名称',
    `template`      text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '模板内容',
    PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

SET
FOREIGN_KEY_CHECKS = 1;

```

## K8S脚本

```yamlapiVersion: apps/v1
kind: Deployment
metadata:
  name: fops
  labels:
    app: fops
    k8s.kuboard.cn/layer: web
spec:
  replicas: 1
  revisionHistoryLimit: 1
  minReadySeconds: 10     # 这里需要估一个比较合理的值，从容器启动到应用正常提供服务
  strategy:               # k8s 默认的 strategy 就是 RollingUpdate， 这里写明出来可以调节细节参数
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1         # 更新时允许最大激增的容器数，默认 replicas 的 1/4 向上取整
      maxUnavailable: 0   # 更新时允许最大 unavailable 容器数，默认 replicas 的 1/4 向下取整
  selector:
    matchLabels:
      app: fops
  template:
    metadata:
      name: fops
      labels:
        app: fops
    spec:
      containers:
        - name: fops
          image: farseernet/fops:latest
          imagePullPolicy: IfNotPresent
          imagePullSecrets:
            - name: regsecret
          ports:
            - containerPort: 80
          envFrom:  #以密文的方式，把配置项写到env
            - secretRef:
                name: fops
          env:
            - name: TZ
              value: Asia/Shanghai
          volumeMounts:
            - name: fops
              mountPath: /var/lib/fops/kube/ # K8S集群管理的配置路径
              subPath: kube
            - name: fops
              mountPath: /var/lib/fops/log/ # 构建日志
              subPath: log
            - name: fops
              mountPath: /var/lib/fops/dist/  # 编译后存放的路径
              subPath: dist
            - name: fops
              mountPath: /var/lib/fops/git/   # GIT仓库存放的路径
              subPath: git
            - name: dockersock
              mountPath: /var/run/docker.sock
            - name: dockerbin
              mountPath: /usr/bin/docker
      volumes:
        - name: fops
          persistentVolumeClaim:
            claimName: pvc-fops
        - name: dockersock
          hostPath:
            path: /var/run/docker.sock
        - name: dockerbin
          hostPath:
            path: /usr/bin/docker
---
apiVersion: v1
data:
  Database__Items__0__PassWord: ''
  Database__Items__0__Server: ''
  Database__Items__0__UserID: ''
  Logging__LogLevel__Default: ''
  Redis__0__Password: ''
  Redis__0__Server: ''
kind: Secret
metadata:
  name: fops
  namespace: default
type: Opaque
---
apiVersion: v1
kind: Service
metadata:
  name: fops
spec:
  selector:
    app: fops
  type: ClusterIP
  ports:
    - name: http # 请为所有端口指定名称
      port: 80 # 暴露在集群内的服务端口
      protocol: TCP
      targetPort: 80
---
apiVersion: extensions/v1beta1
kind: Ingress
metadata:
  name: fops
spec:
  rules:
    - host: fops.xxxxxxx # 访问的域名
      http:
        paths:
          - path: /
            backend:
              serviceName: fops
              servicePort: 80
```