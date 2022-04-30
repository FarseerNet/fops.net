/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1.mysql
 Source Server Type    : MySQL
 Source Server Version : 80027
 Source Host           : localhost:3306
 Source Schema         : fops

 Target Server Type    : MySQL
 Target Server Version : 80027
 File Encoding         : 65001

 Date: 04/12/2021 15:05:30
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admin
-- ----------------------------
DROP TABLE IF EXISTS `admin`;
CREATE TABLE `admin` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '管理员名称',
  `user_pwd` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '管理员密码',
  `is_enable` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否启用',
  `last_login_at` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) COMMENT '上次登陆时间',
  `last_login_ip` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '上次登陆IP',
  `create_at` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) COMMENT '创建时间',
  `create_user` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '创建人',
  `create_id` int NOT NULL DEFAULT '0' COMMENT '创建人ID',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for basic_git
-- ----------------------------
DROP TABLE IF EXISTS `basic_git`;
CREATE TABLE `basic_git` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT 'Git名称',
  `hub` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '托管地址',
  `user_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '账户名称',
  `user_pwd` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '账户密码',
  `pull_at` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) COMMENT '拉取时间',
  `branch` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'main' COMMENT '分支',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for basic_project
-- ----------------------------
DROP TABLE IF EXISTS `basic_project`;
CREATE TABLE `basic_project` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '项目名称',
  `group_id` int NOT NULL DEFAULT '0' COMMENT '项目组ID',
  `git_id` int NOT NULL DEFAULT '0' COMMENT 'GIT',
  `path` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '项目路径',
  `k8s_tpl_deployment` int NOT NULL DEFAULT '0' COMMENT 'K8SDeployment模板',
  `k8s_tpl_ingress` int NOT NULL DEFAULT '0' COMMENT 'K8SIngress模板',
  `k8s_tpl_service` int NOT NULL DEFAULT '0' COMMENT 'K8SService模板',
  `k8s_tpl_config` int NOT NULL DEFAULT '0' COMMENT 'K8SConfig模板',
  `k8s_tpl_variable` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT 'K8S模板自定义变量(K1=V1,K2=V2)',
  `docker_ver` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '镜像版本',
  `cluster_ver` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '{}' COMMENT '集群版本',
  `dockerfile_tpl` int NOT NULL DEFAULT '0' COMMENT 'DockerfileTpl模板',
  `docker_hub` int NOT NULL DEFAULT '0' COMMENT '镜像仓库',
  `entry_point` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '程序入口名称',
  `entry_port` int NOT NULL DEFAULT '80' COMMENT '程序启动端口',
  `domain` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '访问域名',
  `build_type` tinyint NOT NULL DEFAULT '0' COMMENT '构建方式',
  `shell_script` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Shell脚本',
  `appId` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '应用ID',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for basic_project_group
-- ----------------------------
DROP TABLE IF EXISTS `basic_project_group`;
CREATE TABLE `basic_project_group` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '项目名称',
  `cluster_ids` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '集群ID列表',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for build
-- ----------------------------
DROP TABLE IF EXISTS `build`;
CREATE TABLE `build` (
  `id` int NOT NULL AUTO_INCREMENT,
  `project_id` int NOT NULL DEFAULT '0' COMMENT '项目ID',
  `build_number` int NOT NULL DEFAULT '0' COMMENT '构建号',
  `status` tinyint NOT NULL DEFAULT '0' COMMENT '状态',
  `is_success` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否成功',
  `create_at` datetime(6) NOT NULL COMMENT '开始时间',
  `finish_at` datetime(6) NOT NULL COMMENT '完成时间',
  `cluster_id` int NOT NULL DEFAULT '0' COMMENT '集群ID',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `status` (`status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=769 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for docker_file_tpl
-- ----------------------------
DROP TABLE IF EXISTS `docker_file_tpl`;
CREATE TABLE `docker_file_tpl` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '模板名称',
  `template` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '模板内容',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for docker_hub
-- ----------------------------
DROP TABLE IF EXISTS `docker_hub`;
CREATE TABLE `docker_hub` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '镜像名称',
  `hub` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '托管地址',
  `user_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '账户名称',
  `user_pwd` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '账户密码',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for k8s_cluster
-- ----------------------------
DROP TABLE IF EXISTS `k8s_cluster`;
CREATE TABLE `k8s_cluster` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '集群名称',
  `config` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '本地kubectl配置',
  `runtime_env_type` tinyint NOT NULL DEFAULT '0' COMMENT '集群环境类型',
  `sort` int NOT NULL DEFAULT '0' COMMENT '排序',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `sort` (`sort`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for k8s_yaml_tpl
-- ----------------------------
DROP TABLE IF EXISTS `k8s_yaml_tpl`;
CREATE TABLE `k8s_yaml_tpl` (
  `id` int NOT NULL AUTO_INCREMENT,
  `k8s_kind_type` tinyint NOT NULL DEFAULT '0' COMMENT 'k8s类型',
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '模板名称',
  `template` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '模板内容',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;

SET FOREIGN_KEY_CHECKS = 1;
